using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public partial class MainForm : Form
    {
        #region Methods

        private List<CardSet> GetMTGJSONSets()
        {
            const string URL = "https://mtgjson.com/json/SetList.json.zip";
            var sets = new List<CardSet>();
            try
            {
                var zip = DownloadZIP(URL);
                var unzipped = Unzipper.Unzip(zip);
                string json = System.Text.Encoding.UTF8.GetString(unzipped);
                MTGJSONSetListSet[] setList = JsonConvert.DeserializeObject<MTGJSONSetListSet[]>(json);
                if (setList == null) throw new InvalidDataException("Invalid JSON encountered");
                CardSet set;
                foreach (var mtgjsonSet in setList)
                {
                    var code = mtgjsonSet.code;
                    set = new CardSet { Name = mtgjsonSet.name, Code = code, MTGJSONURL = $"https://mtgjson.com/json/{code}.json.zip"};
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

        private static byte[] DownloadZIP(string url)
        {
            byte[] zip;
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 15);
                var httpResponseMessage = client.GetAsync(url).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                    zip = httpResponseMessage.Content.ReadAsByteArrayAsync().Result;
                else
                    throw new HttpRequestException($"{(int)httpResponseMessage.StatusCode}: {httpResponseMessage.StatusCode.ToString()}");
                return zip;
            }
        }

        #endregion Methods

        #region Events

        private void InitUIWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Loading catalog...", 1));
            Globals.Forms.DBViewForm.LoadSets();
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Loading collections...", 2));
            Globals.Forms.NavigationForm.LoadGroups();
            CardManager.CountInventory();
            AddSetIcons();
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Starting application...", 3));
            Thread.Sleep(100);
        }

        private void InitUIWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            splash.ProgressChanged(e.UserState as SplashProgressObject);
        }

        private void InitUIWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var settingsManager = new SettingsManager();
            settingsManager.LayoutDockPanel();
            Globals.Forms.NavigationForm.LoadTree();
            Globals.Forms.DBViewForm.LoadTree();
            Globals.Forms.NavigationForm.CollectionActivated += EventManager.NavigationFormCollectionActivated;
            Show();
            settingsManager.LayoutMainForm();
            CheckForNewSetsWorker.RunWorkerCompleted += CheckForNewSetsWorker_RunWorkerCompleted;
            CheckForNewSetsWorker.RunWorkerAsync();
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
                            builder.Append($"{set.Name}");
                        else
                            builder.Append($"\n{set.Name}");
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

        #endregion Events

        private class MTGJSONSetListSet
        {
            public string code;
            public MTGJSONSetListSetMetadata meta;
            public string name;
            public string releaseDate;
            public string type;
        }

        private class MTGJSONSetListSetMetadata
        {
            public string date;
            public string pricesDate;
            public string version;
        }
    }
}