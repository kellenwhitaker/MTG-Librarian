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
            using (var context = new ScryfallCardsDbContext())
            {
                var match = (from m in context.Metadata
                            where m.Name == "LastCatalogUpdate"
                            select m).FirstOrDefault();
                var lastUpdate = DateTime.MinValue;
                if (match != null && match.Value != "")
                    DateTime.TryParse(match.Value, out lastUpdate);
                if (match == null || (DateTime.Now.Date - lastUpdate.Date).Days > 0)
                {
                    CheckForNewSetsWorker.RunWorkerCompleted += CheckForNewSetsWorker_RunWorkerCompleted;
                    CheckForNewSetsWorker.RunWorkerAsync();
                }
            }
        }

        private void checkForNewSetsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = new UpdateSetsResult
            {
                setsNeedingIcons = new List<ScryfallCardSet>(),
                setsNeedingRefresh = new List<ScryfallCardSet>()
            };
            var scryfallSets = GetScryfallSets();
            try
            {
                using (var context = new ScryfallCardsDbContext())
                {
                    var DBSets = from s in context.Sets
                                 select s;

                    foreach (var set in scryfallSets)
                    {
                        var dbSet = from s in DBSets
                                    where s.code == set.code
                                    select s;

                        var match = dbSet.FirstOrDefault();
                        if (match == null && set.code == set.SymbolCode)
                            result.setsNeedingIcons.Add(set);
                        else
                        {
                            (set.CommonIconBytes, set.UncommonIconBytes, set.RareIconBytes, set.MythicRareIconBytes) =
                                (match.CommonIconBytes, match.UncommonIconBytes, match.RareIconBytes, match.MythicRareIconBytes);
                            if (match.card_count != set.card_count)
                                result.setsNeedingRefresh.Add(match);
                        }
                    }
                    foreach (var set in DBSets)
                    {
                        if ((set.code == set.SymbolCode && set.LastUpdated.HasValue && set.LastUpdated.Value < DateTime.Parse(set.released_at) &&
                            (set.CommonIconBytes == null && set.UncommonIconBytes == null && set.RareIconBytes == null && set.MythicRareIconBytes == null)))
                        {
                            var match = from s in scryfallSets
                                        where s.code == set.code
                                        select s;
                            if (match.FirstOrDefault() != null)
                                result.setsNeedingIcons.Add(match.FirstOrDefault());
                        }
                    }
                  
                }
                using (var context = new ScryfallCardsDbContext())
                {
                    foreach (var set in scryfallSets)
                    {
                        set.LastUpdated = DateTime.Now;
                        context.Upsert(set);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                if (!ex.ToString().Contains("no such table: Sets"))
                    MessageBox.Show(ex.ToString());
            }
            e.Result = result;
            UpdateCards();
        }

        private void UpdateCards()
        {
            using (var context = new ScryfallCardsDbContext())
            {
                var cardsToUpdate = (from c in context.Catalog
                                    join s in context.Library 
                                    on c.ScryfallId equals s.ScryfallId
                                    select c);
                var cardsToUpdateDictionary = new Dictionary<string, ScryfallMagicCardBase>();
                foreach (var card in cardsToUpdate)
                    if (!cardsToUpdateDictionary.ContainsKey(card.ScryfallId))
                        cardsToUpdateDictionary.Add(card.ScryfallId, card);
                if (cardsToUpdate.Any())
                {
                    var updateCardsTask = new UpdateCardsTask(cardsToUpdateDictionary.Values.Cast<ScryfallMagicCardBase>().ToList());
                    Globals.Forms.TasksForm.TaskManager.AddTask(updateCardsTask);
                }
            }
        }

        private void CheckForNewSetsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            using (var context = new ScryfallCardsDbContext())
            {
                var match = (from m in context.Metadata
                             where m.Name == "LastCatalogUpdate"
                             select m).FirstOrDefault();
                if (match != null)
                    match.Value = DateTime.Now.ToString();
                else
                    context.Metadata.Add(new Metadata { Name = "LastCatalogUpdate", Value = DateTime.Now.ToString() });
                context.SaveChanges();
            }
            var result = e.Result as UpdateSetsResult;
            if (result.setsNeedingRefresh.Count > 0)
            {
                foreach (var set in result.setsNeedingRefresh)
                    Globals.Forms.DBViewForm.LoadSet(set.code);
            }
            if (result.setsNeedingIcons.Count > 0)
            {
                mainStatusLabel.Top = statusBarActionButton.Top + 5;
                mainStatusLabel.Visible = true;
                var tasksToAdd = new List<BackgroundTask>();
                foreach (var set in result.setsNeedingIcons)
                    tasksToAdd.Add(new DownloadSetTask(set));

                Globals.Forms.TasksForm.TaskManager.AddTasks(tasksToAdd);
                mainStatusLabel.Text = $"{result.setsNeedingIcons.Count} set{(result.setsNeedingIcons.Count > 1 ? "s" : "")} added to download queue.";
            }
        }

        private void UpdateMissingSetIconsWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            using (var context = new ScryfallCardsDbContext())
            {
                var DBSets = from s in context.Sets
                             where s.code == s.SymbolCode && s.CommonIconBytes == null && s.UncommonIconBytes == null && s.RareIconBytes == null && s.MythicRareIconBytes == null
                             select s;
         
                e.Result = DBSets.ToList();
            }
        }
        private void UpdateMissingSetIconsWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as List<ScryfallCardSet>;
            if (result.Count > 0)
            {
                mainStatusLabel.Top = statusBarActionButton.Top + 5;
                mainStatusLabel.Visible = true;
                var tasksToAdd = new List<BackgroundTask>();
                foreach (var set in result)
                    tasksToAdd.Add(new DownloadSetTask(set));

                Globals.Forms.TasksForm.TaskManager.AddTasks(tasksToAdd);
                mainStatusLabel.Text = $"{result.Count} set{(result.Count > 1 ? "s" : "")} added to download queue.";
            }
        }

        #endregion Events
        private class UpdateSetsResult
        {
            public List<ScryfallCardSet> setsNeedingIcons;
            public List<ScryfallCardSet> setsNeedingRefresh;
        }
    }
    
}