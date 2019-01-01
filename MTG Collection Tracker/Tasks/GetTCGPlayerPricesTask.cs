using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MTG_Librarian
{
    public class GetTCGPlayerPricesTask : BackgroundTask
    {
        private List<FullInventoryCard> cardsToPrice = new List<FullInventoryCard>();
        public Dictionary<int, double?> productIdDictionary = new Dictionary<int, double?>();

        public GetTCGPlayerPricesTask(ArrayList cards)
        {
            bool isPlural = cards.Count > 1;
            Caption = $"Getting price{(isPlural ? "s" : "")} for {cards.Count} card{(isPlural ? "s" : "")}";
            foreach (var card in cards)
                cardsToPrice.Add(card as FullInventoryCard);
        }

        public override void Run()
        {
            RunState = RunState.Running;
            RunWorkerAsync();
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
                    if (productIdDictionary.ContainsKey(result.productId))
                        productIdDictionary[result.productId] = result.marketPrice;

                RunState = responseObject.results.Count() == 0 ? RunState.Failed : RunState.Completed;
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
                RunState = RunState.Failed;
            }
        }

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
    }
}
