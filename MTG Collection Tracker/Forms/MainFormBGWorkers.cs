using HtmlAgilityPack;
using RestSharp;
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
        private List<ScryfallCardSet> GetScryfallSets()
        {
            const string URL = "https://api.scryfall.com";
            var scryfallCardSets = new List<ScryfallCardSet>();
            try
            {
                var client = new RestClient(URL);
                var request = new RestRequest("sets", Method.Get);
                string responseContent = client.Execute(request).Content;
                var responseObject = JsonConvert.DeserializeObject<ScryfallSetList>(responseContent);
                if (responseObject == null) throw new InvalidDataException("Invalid JSON encountered");
                scryfallCardSets.AddRange(responseObject.data as ScryfallCardSet[]);
                while (responseObject.has_more)
                {
                    string relativeURL = responseObject.next_page.Substring(URL.Count());
                    request = new RestRequest(relativeURL, Method.Get);
                    responseContent = client.Execute(request).Content;
                    responseObject = JsonConvert.DeserializeObject<ScryfallSetList>(responseContent);
                    if (responseObject == null) throw new InvalidDataException("Invalid JSON encountered");
                    scryfallCardSets.AddRange(responseObject.data as ScryfallCardSet[]);
                }
            }
            catch (Exception ex) 
            {
                DebugOutput.WriteLine(ex.ToString());
                MessageBox.Show("Failed to gather list of available sets.");
            }
          
            return scryfallCardSets;
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
            SettingsManager.LayoutDockPanel();
            Globals.Forms.NavigationForm.LoadTree();
            Globals.Forms.DBViewForm.LoadTree();
            Globals.Forms.NavigationForm.CollectionActivated += EventManager.NavigationFormCollectionActivated;
            Show();
            SettingsManager.LayoutMainForm();
            CheckForNewSetsWorker.RunWorkerCompleted += CheckForNewSetsWorker_RunWorkerCompleted;
            CheckForNewSetsWorker.RunWorkerAsync();
        }

        private void checkForNewSetsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var sets = GetScryfallSets();
            try
            {
                using (var context = new ScryfallCardsDbContext())
                {
                    var DBSets = from s in context.Sets
                                 select s;

                    foreach (var set in DBSets)
                    {
                        sets.RemoveAll(x => x.code == set.code);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.ToString().Contains("no such table: Sets"))
                    MessageBox.Show(ex.ToString());
            }
            e.Result = sets;
        }

        private void CheckForNewSetsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var sets = e.Result as List<ScryfallCardSet>;
            if (sets.Count > 0)
            {
                mainStatusLabel.Top = statusBarActionButton.Top + 5;
                mainStatusLabel.Visible = true;
                var tasksToAdd = new List<BackgroundTask>();
                foreach (var set in sets)
                    tasksToAdd.Add(new DownloadSetTask(set));

                Globals.Forms.TasksForm.TaskManager.AddTasks(tasksToAdd);
                mainStatusLabel.Text = $"{sets.Count} set{(sets.Count > 1 ? "s" : "")} added to download queue.";
            }
        }

        #endregion Events
    }
}