using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;

namespace MTG_Librarian
{
    public class GetTCGPlayerPricesTask : BackgroundTask
    {
        #region Fields

        private List<FullInventoryCard> cardsToPrice;
        public Dictionary<int, double?> productIdDictionary = new Dictionary<int, double?>();

        #endregion Fields

        #region Constructors

        public GetTCGPlayerPricesTask(List<FullInventoryCard> cards)
        {
            bool isPlural = cards.Count > 1;
            Caption = $"Getting price{(isPlural ? "s" : "")} for {cards.Count} card{(isPlural ? "s" : "")}";
            cardsToPrice = cards;
            TotalWorkUnits = 1;
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
                string token = ThirdPartyAPI.TCGPlayer.GetBearerToken();
                var client = new RestClient("https://api.tcgplayer.com/v1.19.0");
                var builder = new StringBuilder();
                foreach (var card in cardsToPrice)
                    if (!productIdDictionary.ContainsKey(card.tcgplayerProductId))
                        productIdDictionary.Add(card.tcgplayerProductId, null);

                int count = 0;
                foreach (int id in productIdDictionary.Keys)
                {
                    if (count == 0)
                        builder.Append(id);
                    else
                        builder.Append($",{id}");
                    count++;
                }
                string productIds = builder.ToString();
                var request = new RestRequest($"pricing/product/{productIds}", Method.GET);
                request.AddHeader("Authorization", $"bearer {token}");
                string responseContent = client.Execute(request).Content;
                var responseObject = JsonConvert.DeserializeObject<ProductPricesResponseObject>(responseContent);
                foreach (var result in responseObject.results)
                    if (result.subTypeName == "Normal" && productIdDictionary.ContainsKey(result.productId))
                        productIdDictionary[result.productId] = result.marketPrice;

                RunState = responseObject.results.Count() == 0 ? RunState.Failed : RunState.Completed;
                CompletedWorkUnits = TotalWorkUnits;
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
                RunState = RunState.Failed;
            }
            finally { watch.Stop(); }
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