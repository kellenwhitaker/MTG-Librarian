using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MTG_Librarian
{
    public class UpdateCardsTask : BackgroundTask
    {
        #region Fields

        private List<FullInventoryCard> cardsToUpdate;
        public List<ScryfallMagicCard> CardsUpdated = new List<ScryfallMagicCard>();

        #endregion Fields

        #region Constructors

        public UpdateCardsTask(List<FullInventoryCard> cards)
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
                    string scryfallBaseUrl = "https://api.scryfall.com";
                    string scryfallUrl;
                    var client = new RestClient(scryfallBaseUrl);
                    foreach (var card in cardsToUpdate)
                    {
                        scryfallUrl = $"/cards/{card.ScryfallId}";
                        var request = new RestRequest(scryfallUrl, Method.Get);
                        string responseContent = client.Execute(request).Content;
                        var responseObject = JsonConvert.DeserializeObject<ScryfallCard>(responseContent);
                        if (responseObject == null) throw new InvalidDataException("Invalid JSON encountered");
                        DebugOutput.WriteLine(responseContent);
                        var scryfallMagicCard = responseObject.ToScryfallMagicCard();
                        context.Catalog.Update(scryfallMagicCard);
                        context.SaveChanges();
                        CardsUpdated.Add(scryfallMagicCard);
                        CompletedWorkUnits++;
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