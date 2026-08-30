using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MTG_Librarian
{
    public class UpdateCardsTask : BackgroundTask
    {
        #region Fields

        private List<ScryfallMagicCardBase> cardsToUpdate;
        public List<ScryfallMagicCard> CardsUpdated = new List<ScryfallMagicCard>();
        public bool WholeCatalogUpdate { get; set; } = true;

        #endregion Fields

        #region Constructors

        public UpdateCardsTask(List<ScryfallMagicCardBase> cards)
        {
            bool isPlural = cards.Count > 1;
            Caption = $"Getting update{(isPlural ? "s" : "")} for {cards.Count} card{(isPlural ? "s" : "")}";
            cardsToUpdate = cards;
            TotalWorkUnits = cards.Count;
        }

        #endregion Constructors

        #region Methods

        public override void Run()
        {
            base.Run();
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            if (cardsToUpdate.Any())
            {
                try
                {
                    using (var context = new ScryfallCardsDbContext())
                    {
                        foreach (var scryfallMagicCard in CardsUpdated)
                            context.Catalog.Update(scryfallMagicCard);

                        if (WholeCatalogUpdate)
                        {
                            var match = context.Metadata.FirstOrDefault(m => m.Name == "LastCatalogUpdate");
                            if (match != null)
                            {
                                match.Value = DateTime.Now.ToString();
                                context.Metadata.Update(match);
                            }
                            else
                                context.Metadata.Add(new Metadata { Name = "LastCatalogUpdate", Value = DateTime.Now.ToString() });
                        }
                        context.SaveChanges();
                    }
                    watch.Stop();
                    Globals.Forms.MainForm.notifyIcon.ShowBalloonTip(5000, "MTG Librarian", $"Finished updating {cardsToUpdate.Count} cards.", System.Windows.Forms.ToolTipIcon.Info);
                    if (CardsUpdated.Count != cardsToUpdate.Count)
                        RunState = RunState.Failed;
                    else
                    {
                        if (Globals.States.UpdatePricesMode)
                        {
                            CardManager.SaveCollectionSnapshots();
                            System.Windows.Forms.Application.Exit();
                        }
                        RunState = RunState.Completed;
                    }
                }
                catch (Exception ex)
                {
                    DebugOutput.WriteLine(ex.ToString());
                    RunState = RunState.Failed;
                }
            }
            else
            {
                if (Globals.States.UpdatePricesMode)
                {
                    System.Windows.Forms.Application.Exit();
                }
                RunState = RunState.Completed;
            }
        }
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            if (Globals.States.UpdatePricesMode && !cardsToUpdate.Any())
            {
                Thread.Sleep(10000);
                return;
            }

            try
            {
                const int batchSize = 75; // Scryfall /cards/collection is typically limited; 75 is safe
                string scryfallBaseUrl = "https://api.scryfall.com";
                var client = new RestClient(scryfallBaseUrl);

                Globals.Forms.MainForm.notifyIcon.ShowBalloonTip(5000, "MTG Librarian", $"Getting updates for {cardsToUpdate.Count} cards...", System.Windows.Forms.ToolTipIcon.Info);
                // Process in batches
                for (int start = 0; start < cardsToUpdate.Count; start += batchSize)
                {
                    var searchCollection = new ScryfallSearchCollection();
                    var batchIds = cardsToUpdate
                        .Skip(start)
                        .Take(batchSize)
                        .Select(c => c.ScryfallId)
                        .Where(id => !string.IsNullOrWhiteSpace(id))
                        .ToList();

                    if (batchIds.Count == 0)
                        continue;

                    foreach (var id in batchIds)
                        searchCollection.identifiers.Add(new ScryfallSearchCollectionIdentifier { id = id });

                    var request = new RestRequest("/cards/collection", Method.Post);
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("User-Agent", $"MTG Librarian/{SettingsManager.ApplicationSettings.ApplicationVersion}");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddJsonBody(JsonConvert.SerializeObject(searchCollection, Formatting.None, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }));

                    TimeSpan elapsed;
                    lock (Globals.States.LastScryfallSearchLockObject)
                    {
                        elapsed = DateTime.Now - Globals.States.LastScryfallSearchTime;
                    }
                    if (elapsed < TimeSpan.FromMilliseconds(500))
                    {
                        Thread.Sleep(500);
                    }
                    lock (Globals.States.LastScryfallSearchLockObject)
                    {
                        Globals.States.LastScryfallSearchTime = DateTime.Now;
                    }
                    var response = client.Execute(request);
                    if (response.StatusCode == (System.Net.HttpStatusCode)429)
                    {
                        DebugOutput.WriteLine("ScryfallSearchCollection returned 429 Too Many Requests. Waiting 30 seconds before retrying.");
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                        response = client.Execute(request);
                    }
                    var scryfallCards = response.Content != null
                        ? JsonConvert.DeserializeObject<ScryfallCardList>(response.Content)?.data
                        : null;
                    if (scryfallCards == null)
                    {
                        DebugOutput.WriteLine("ScryfallSearchCollection returned null for a batch.");
                        continue;
                    }

                    foreach (var sfCard in scryfallCards)
                    {
                        if (sfCard == null) continue;
                        var scryfallMagicCard = sfCard.ToScryfallMagicCard();
                        CardsUpdated.Add(scryfallMagicCard);
                        CompletedWorkUnits++;
                    }
                    Thread.Sleep(TimeSpan.FromMilliseconds(500)); // Sleep briefly to avoid hitting rate limits
                }
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
            }
        }

        #endregion Methods

        #region Classes

        private class ProductPricesResponseObject
        {
            public bool success;
            public string[] errors;
            public ProductPricesResponseObjectResult[] results;

            public class ProductPricesResponseObjectResult
            {
                public int productId;
                public double? lowPrice;
                public double? midPrice;
                public double? highPrice;
                public double? marketPrice;
                public double? directLowPrice;
                public string subTypeName;
            }
        }

        #endregion Classes
    }
}