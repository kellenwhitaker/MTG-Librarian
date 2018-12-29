using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using KW.WinFormsUI.Docking;

namespace MTG_Librarian
{
    public partial class MainForm : Form
    {
        private void InitUIWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Loading catalog...", 1));
            Globals.Forms.DBViewForm.LoadSets();
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Loading collections...", 2));
            Globals.Forms.NavigationForm.LoadGroups();
            CountInventory();
            AddSetIcons();
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Starting application...", 3));
            Thread.Sleep(100);
        }

        private void InitUIWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            splash.ProgressChanged(e.UserState as SplashProgressObject);
        }

        private void SetupDockPanel(DockState dockState, SortedDictionary<int, DockState> ZOrderDictionary)
        {
            var settings = ApplicationSettings.GetDockPaneSettings(dockState);
            if (!ZOrderDictionary.Keys.Any(x => x == settings.ZOrderIndex))
                ZOrderDictionary.Add(settings.ZOrderIndex, dockState);
            var dockWindow = Globals.Forms.DockPanel.DockWindows[dockState];
            var contentPanes = settings.ContentPanes;
            DockContent activatedContent = null;
            if (contentPanes != null && contentPanes.Count > 0)
            {
                foreach (var contentPane in contentPanes)
                {
                    var dockContent = ShowForm(contentPane, dockState);
                    if (contentPane.IsActivated)
                        activatedContent = dockContent;
                }
                activatedContent?.Activate();
            }
        }

        private DockContent ShowForm(ApplicationSettings.DockContentSettings contentSettings, DockState dockState)
        {
            DockContent dockContent;
            if (contentSettings.ContentType == ApplicationSettings.DockContentEnum.CardInfoForm)
                dockContent = Globals.Forms.CardInfoForm;
            else if (contentSettings.ContentType == ApplicationSettings.DockContentEnum.DBViewForm)
                dockContent = Globals.Forms.DBViewForm;
            else if (contentSettings.ContentType == ApplicationSettings.DockContentEnum.NavigatorForm)
                dockContent = Globals.Forms.NavigationForm;
            else if (contentSettings.ContentType == ApplicationSettings.DockContentEnum.TasksForm)
                dockContent = Globals.Forms.TasksForm;
            else
                dockContent = LoadCollection(contentSettings.DocumentId, dockState);

            dockContent?.Show(Globals.Forms.DockPanel, dockState);
            return dockContent;
        }

        private void RestoreZOrder(SortedDictionary<int, DockState> ZOrderDictionary)
        {
           foreach (KeyValuePair<int, DockState> pair in ZOrderDictionary)
                Globals.Forms.DockPanel.UpdateDockWindowZOrder(pair.Value.ToDockStyle(), true);
        }

        private void SetupDefaultDockConfiguration()
        {
            Globals.Forms.DBViewForm.Show(Globals.Forms.DockPanel, DockState.DockBottom);
            Globals.Forms.CardInfoForm.Show(Globals.Forms.DockPanel, DockState.DockLeft);
            Globals.Forms.TasksForm.Show(Globals.Forms.DockPanel, DockState.DockRight);
            Globals.Forms.NavigationForm.Show(Globals.Forms.DockPanel, DockState.DockRight);
            Globals.Forms.DockPanel.UpdateDockWindowZOrder(DockStyle.Left, true);
            Globals.Forms.DockPanel.UpdateDockWindowZOrder(DockStyle.Right, true);
            CardCollection mainCollection;
            using (var context = new MyDbContext())
                mainCollection = (from c in context.Collections
                                  where c.CollectionName == "Main"
                                  select c).FirstOrDefault();

            if (mainCollection != null)
                LoadCollection(mainCollection);
        }

        private void InitUIWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ApplicationSettings = new ApplicationSettings();
            Globals.Forms.DockPanel.SuspendLayout();
            Globals.Forms.DBViewForm.SuspendLayout();
            Globals.Forms.CardInfoForm.SuspendLayout();
            Globals.Forms.NavigationForm.SuspendLayout();
            Globals.Forms.TasksForm.SuspendLayout();
            if (!ApplicationSettings.InInitialState)
            {
                var ZOrderDictionary = new SortedDictionary<int, DockState>();
                SetupDockPanel(DockState.DockBottom, ZOrderDictionary);
                SetupDockPanel(DockState.DockLeft, ZOrderDictionary);
                SetupDockPanel(DockState.DockRight, ZOrderDictionary);
                SetupDockPanel(DockState.Document, ZOrderDictionary);
                SetupDockPanel(DockState.DockBottomAutoHide, ZOrderDictionary);
                SetupDockPanel(DockState.DockLeftAutoHide, ZOrderDictionary);
                SetupDockPanel(DockState.DockRightAutoHide, ZOrderDictionary);
                RestoreZOrder(ZOrderDictionary);
            }
            else
                SetupDefaultDockConfiguration();
            Globals.Forms.NavigationForm.LoadTree();
            Globals.Forms.DBViewForm.LoadTree();
            Globals.Forms.NavigationForm.CollectionActivated += navFormCollectionActivated;
            Globals.Forms.DockPanel.DockLeftPortion = ApplicationSettings.DockLeftPortion;
            Globals.Forms.DockPanel.DockRightPortion = ApplicationSettings.DockRightPortion;
            Globals.Forms.DockPanel.DockBottomPortion = ApplicationSettings.DockBottomPortion;
            Show();
            WindowState = ApplicationSettings.MainFormWindowState;
            Location = ApplicationSettings.MainFormLocation;
            Size = ApplicationSettings.MainFormSize;
            Globals.Forms.DockPanel.ResumeLayout();
            Globals.Forms.DBViewForm.ResumeLayout();
            Globals.Forms.CardInfoForm.ResumeLayout();
            Globals.Forms.NavigationForm.ResumeLayout();
            Globals.Forms.TasksForm.ResumeLayout();
            CheckForNewSetsWorker.RunWorkerCompleted += CheckForNewSetsWorker_RunWorkerCompleted;
            CheckForNewSetsWorker.RunWorkerAsync();
        }

        private List<CardSet> GetMTGJSONSets()
        {
            var matchCode_Date = new Regex("</strong><br>(.+)<br><a");
            const string URL = "https://mtgjson.com/sets.html";
            var sets = new List<CardSet>();
            try
            {
                var doc = new HtmlWeb().Load(URL);
                var tableCells = doc.DocumentNode.SelectNodes("//td[i[@class='set']]");
                CardSet set;
                foreach (var Cell in tableCells)
                {
                    set = new CardSet { ScrapedName = Cell.Descendants().ElementAt(2).InnerText.Replace("&amp;", "&") };
                    string InnerHTML = Cell.InnerHtml;
                    var matches = matchCode_Date.Matches(InnerHTML);
                    string matchString;
                    if (matches.Count > 0)
                    {
                        matchString = matches[0].Groups[1].Value;
                        var code_Date = matchString.Split(new[] { '—' });
                        if (code_Date.Length > 0)
                            set.Code = code_Date[0].Trim().ToLower();
                        if (code_Date.Length > 1)
                            set.ReleaseDate = code_Date[1].Trim();
                    }
                    string href = Cell.Descendants().FirstOrDefault(a => a.Attributes.Contains("href"))?.Attributes.FirstOrDefault(a => a.Name == "href")?.Value;
                    if (href != null)
                        set.MTGJSONURL = "http://mtgjson.com/" + href;
                    sets.Add(set);
                }
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
                MessageBox.Show("Failed to gather list of available sets.");
            }
            return sets;
        }

        private void checkForNewSetsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var sets = GetMTGJSONSets();
            sets.RemoveAll(x => x.Code == "ps11"); // redundant set
            using (var context = new MyDbContext())
            {
                var DBSets = from s in context.Sets
                             select s;

                foreach (var set in DBSets)
                {
                    sets.RemoveAll(x => x.Code == set.Code);
                }
            }
            e.Result = sets;
        }

        private void CheckForNewSetsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var sets = e.Result as List<CardSet>;
            if (sets.Count > 0)
            {
                var builder = new System.Text.StringBuilder();
                int count = 0;
                foreach (var set in sets)
                {
                    count++;
                    if (count < 11)
                    {
                        if (builder.Length == 0)
                            builder.Append($"{set.ScrapedName}");
                        else
                            builder.Append($"\n{set.ScrapedName}");
                    }
                    else
                    {
                        builder.Append("\n...");
                        break;
                    }
                }
                string newSets = builder.ToString();
                mainStatusLabel.Text = $"{sets.Count} new set{(sets.Count > 1 ? "s" : "")} available for download.";
                statusBarActionButton.Left = mainStatusLabel.Right;
                mainStatusLabel.Top = statusBarActionButton.Top + 5;
                mainStatusLabel.Visible = statusBarActionButton.Visible = true;
                var yourToolTip = new ToolTip
                {
                    IsBalloon = true,
                    ShowAlways = true,
                    AutoPopDelay = 30000
                };
                yourToolTip.SetToolTip(mainStatusLabel, builder.ToString());
                statusBarActionButtonClickDelegate = () => 
                {
                    var tasksToAdd = new List<BackgroundTask>();
                    foreach (var set in sets)
                        tasksToAdd.Add(new DownloadSetTask(set));

                    Globals.Forms.TasksForm.TaskManager.AddTasks(tasksToAdd);
                    mainStatusLabel.Text = $"{sets.Count} set{(sets.Count > 1 ? "s" : "")} added to download queue.";
                    statusBarActionButton.Visible = false;
                };
            }
        }
    }
}
