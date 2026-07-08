using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public class ScryfallSearchTask : BackgroundTask
    {
        public string Query;
        private string scryfallUrl = null;
        public List<ScryfallMagicCard> Results = new List<ScryfallMagicCard>();
        public int totalCards = 0;
        public string CompletedMessage;
        public ScryfallSearchTask(string query)
        {
            WorkerSupportsCancellation = true;
            Query = query;
            Caption = $"Search: {Query.Replace("%3A", ":")}";
            TotalWorkUnits = 1;
        }
        public override void Run()
        {
            base.Run();
        }
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            try
            {
                string scryfallBaseUrl = "https://api.scryfall.com";
                if (scryfallUrl == null) scryfallUrl = $"/cards/search?{Query}";
                Results.Clear();
                var client = new RestClient(scryfallBaseUrl);
                var request = new RestRequest(scryfallUrl, Method.Get)
                {
                    Timeout = TimeSpan.FromSeconds(15)
                };
                request.AddHeader("Accept", "application/json");
                request.AddHeader("User-Agent", $"MTG Librarian/{SettingsManager.ApplicationSettings.ApplicationVersion}");
                var response = client.Execute(request);
                if (response.ResponseStatus == ResponseStatus.TimedOut)
                {
                    RunState = RunState.Failed;
                    CompletedMessage = "Request timed out";
                    return;
                }
                string responseContent = response.Content;
                if (CancellationPending)
                {
                    RunState = RunState.Canceled;
                    return;
                }
                var responseObject = JsonConvert.DeserializeObject<ScryfallCardList>(responseContent);
                if (responseObject == null) throw new InvalidDataException("Invalid JSON encountered");
                if (responseObject.Object == "error")
                {
                    DebugOutput.WriteLine($"error: {Query}");
                    DebugOutput.WriteLine(responseContent);
                    if (responseObject.status == 404)
                        RunState = RunState.Completed;
                    else
                        RunState = RunState.Failed;
                    return;
                }
                DebugOutput.WriteLine(responseContent);
                totalCards = responseObject.total_cards;
                foreach (var item in responseObject.data)
                    Results.Add(item.ToScryfallMagicCard());
                if (responseObject.has_more)
                {
                    scryfallUrl = responseObject.next_page;
                    RunState = RunState.WaitingForInput;
                }
                else
                    RunState = RunState.Completed;
                CompletedWorkUnits = TotalWorkUnits;
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
                RunState = RunState.Failed;                
            }
            finally
            {
                watch.Stop();
            }
        }
    }
}
