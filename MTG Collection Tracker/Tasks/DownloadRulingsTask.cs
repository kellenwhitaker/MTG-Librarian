using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MTG_Librarian
{
    internal class DownloadRulingsTask : BackgroundTask
    {
        public ScryfallMagicCardBase Card = null;

        public DownloadRulingsTask(ScryfallMagicCardBase card)
        {
            Card = card;
            Caption = $"Fetching rulings: {Card.Name}";
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
                var client = new RestClient(Card.rulings_uri);
                var request = new RestRequest("", Method.Get);
                string responseContent = client.Execute(request).Content;
                var responseObject = JsonConvert.DeserializeObject<ScryfallRulingsList>(responseContent);
                if (responseObject == null) throw new InvalidDataException("Invalid JSON encountered");
                if (responseObject.Object == "error")
                {
                    DebugOutput.WriteLine($"error: {Card.rulings_uri}");
                    DebugOutput.WriteLine(responseContent);
                    if (responseObject.status == 404)
                        RunState = RunState.Completed;
                    else
                        RunState = RunState.Failed;
                    return;
                }

                Card.rulings = new List<ScryfallCardRuling>();
                Card.rulings.AddRange(responseObject.data);
                RunState = RunState.Completed;
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine($"Error fetching rulings for {Card.Name}: {ex.Message}");
                RunState = RunState.Failed;
            }
        }
    }
}
