using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MTG_Collection_Tracker
{
    public partial class MainForm : Form
    {
        private void InitUIWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Loading collections...", 1));
            navForm.LoadGroups();
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Loading catalog...", 2));
            dbViewForm.LoadSets();
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
            cardInfoForm.Show(dockPanel1, DockState.DockLeft);
            navForm.LoadTree();
            navForm.CollectionActivated += navFormCollectionActivated;
            navForm.Show(dockPanel1, DockState.DockRight);
            tasksForm.Show(dockPanel1, DockState.DockRight);
            navForm.Activate();
            dbViewForm.LoadTree();
            dbViewForm.Show(dockPanel1, DockState.DockBottom);
            dockPanel1.UpdateDockWindowZOrder(DockStyle.Left, true);
            dockPanel1.UpdateDockWindowZOrder(DockStyle.Right, true);
            CheckForNewSetsWorker.RunWorkerAsync();
            Show();
        }

        private List<CardSet> GetMTGJSONSets()
        {
            Regex matchCode_Date = new Regex("</strong><br>(.+)<br><a");
            string URL = "https://mtgjson.com/v4/sets.html";
            var doc = new HtmlWeb().Load(URL);
            var sets = new List<CardSet>();
            var tableCells = doc.DocumentNode.SelectNodes("//td[i[@class='set']]");
            CardSet set;
            foreach (var Cell in tableCells)
            {
                set = new CardSet { Name = Cell.Descendants().ElementAt(2).InnerText.Replace("&amp;", "&") };
                string InnerHTML = Cell.InnerHtml;
                MatchCollection matches = matchCode_Date.Matches(InnerHTML);
                string matchString;
                if (matches.Count > 0)
                {
                    matchString = matches[0].Groups[1].Value;
                    var code_Date = matchString.Split(new[] { '—' });
                    if (code_Date.Length > 0)
                        set.Code = code_Date[0].Trim();
                    if (code_Date.Length > 1)
                        set.ReleaseDate = code_Date[1].Trim();
                }
                string href = Cell.Descendants().Where(a => a.Attributes.Contains("href")).FirstOrDefault()?.Attributes.Where(a => a.Name == "href").FirstOrDefault()?.Value;
                if (href != null)
                    set.MTGJSONURL = "http://mtgjson.com/v4/" + href;
                sets.Add(set);
            }
            return sets;
        }



        private void checkForNewSetsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var sets = GetMTGJSONSets();
            sets.RemoveAll(x => x.Name == "Salvat 2011");
            using (var context = new MyDbContext())
            {
                var DBSets = from s in context.Sets
                             select s;

                foreach (var set in DBSets)
                {
                    sets.RemoveAll(x => x.Name == set.Name);
                }
            }
            e.Result = sets;
        }

        private void CheckForNewSetsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var sets = e.Result as List<CardSet>;
            if (sets.Count > 0)
            {
                string newSets = "";
                foreach (var set in sets)
                    newSets += "[" + set.Name + "] ";

                if (MessageBox.Show("The following new sets are available for download: \n\n" + newSets + "\n\nWould you like to download them now?", "New Sets Are Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (var set in sets)
                    {
                        DownloadSetTask task = new DownloadSetTask(set);
                        tasksForm.taskManager.AddTask(task);
                    }
                }
            }
        }
    }
}
