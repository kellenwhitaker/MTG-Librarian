using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                Card.printings = new List<ScryfallCard>();
                string nextPageUri = Card.prints_search_uri;

                while (!string.IsNullOrEmpty(nextPageUri))
                {
                    var client = new RestClient(nextPageUri);
                    var request = new RestRequest("", Method.Get);
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("User-Agent", $"MTG Librarian/{SettingsManager.ApplicationSettings.ApplicationVersion}");
                    string responseContent = client.Execute(request).Content;
                    var responseObject = JsonConvert.DeserializeObject<ScryfallCardList>(responseContent);
                    if (responseObject == null) throw new InvalidDataException("Invalid JSON encountered");
                    if (responseObject.Object == "error")
                    {
                        DebugOutput.WriteLine($"error: {nextPageUri}");
                        DebugOutput.WriteLine(responseContent);
                        if (responseObject.status == 404)
                            RunState = RunState.Completed;
                        else
                            RunState = RunState.Failed;
                        return;
                    }
                    
                    Card.printings.AddRange(responseObject.data);
                    
                    // Check if there are more pages
                    if (responseObject.has_more && !string.IsNullOrEmpty(responseObject.next_page))
                    {
                        nextPageUri = responseObject.next_page;
                    }
                    else
                    {
                        nextPageUri = null;
                    }
                    Thread.Sleep(500); // Sleep for 500 milliseconds to avoid hitting rate limits
                }
                
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
