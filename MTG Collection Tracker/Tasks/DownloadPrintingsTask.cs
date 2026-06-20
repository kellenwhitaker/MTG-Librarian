using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    internal class DownloadPrintingsTask : BackgroundTask
    {
        public ScryfallMagicCardBase Card = null;
        public DownloadPrintingsTask(ScryfallMagicCardBase card)
        {
            Card = card;
            Caption = $"Fetching printings: {Card.Name}";
            TotalWorkUnits = 1;
        }
        public override void Run()
        {
            base.Run();
        }

        protected override void OnDoWork(System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                var client = new RestClient(Card.prints_search_uri);
                var request = new RestRequest("", Method.Get);
                string responseContent = client.Execute(request).Content;
                var responseObject = JsonConvert.DeserializeObject<ScryfallCardList>(responseContent);
                if (responseObject == null) throw new InvalidDataException("Invalid JSON encountered");
                if (responseObject.Object == "error")
                {
                    DebugOutput.WriteLine($"error: {Card.prints_search_uri}");
                    DebugOutput.WriteLine(responseContent);
                    if (responseObject.status == 404)
                        RunState = RunState.Completed;
                    else
                        RunState = RunState.Failed;
                    return;
                }
                Card.printings = new List<ScryfallCard>();
                Card.printings.AddRange(responseObject.data);
                RunState = RunState.Completed;
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine($"Error fetching printings for {Card.Name}: {ex.Message}");
                RunState = RunState.Failed;
            }
        }
    }
}
