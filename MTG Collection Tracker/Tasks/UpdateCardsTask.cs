using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MTG_Librarian
{
    public class UpdateCardsTask : BackgroundTask
    {
        #region Fields

        private List<ScryfallMagicCardBase> cardsToUpdate;
        public List<ScryfallMagicCard> CardsUpdated = new List<ScryfallMagicCard>();

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

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            try
            {
                using (var context = new ScryfallCardsDbContext())
                {
                    const int batchSize = 75; // Scryfall /cards/collection is typically limited; 75 is safe
                    string scryfallBaseUrl = "https://api.scryfall.com";
                    var client = new RestClient(scryfallBaseUrl);

                    
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
                        request.AddHeader("Content-Type", "application/json");
                        request.AddJsonBody(JsonConvert.SerializeObject(searchCollection));
                        var response = client.Execute(request);
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
                            try
                            {
                                if (sfCard == null) continue;
                                var scryfallMagicCard = sfCard.ToScryfallMagicCard();
                                context.Catalog.Update(scryfallMagicCard);
                                CardsUpdated.Add(scryfallMagicCard);
                                CompletedWorkUnits++;
                            }
                            catch (Exception innerEx)
                            {
                                // Log and continue with other cards
                                DebugOutput.WriteLine(innerEx.ToString());
                            }
                        }
                        context.SaveChanges();
                    }

                    RunState = RunState.Completed;
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