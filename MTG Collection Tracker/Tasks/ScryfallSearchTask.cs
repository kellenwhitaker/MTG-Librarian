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
        private string Query;
        private string scryfallUrl = null;
        public List<ScryfallMagicCard> Results = new List<ScryfallMagicCard>();
        public ScryfallSearchTask(string query)
        {
            Query = query;
            Caption = $"Search: {Query.Replace("%3A", "=")}";
            TotalWorkUnits = 5;
        }
        public override void Run()
        {
            base.Run();
        }
        private string AbbreviateLanguage(string lang)
        {
            string abbr;
            switch (lang)
            {
                case "English":
                    {
                        abbr = "EN";
                        break;
                    }
                case "Spanish":
                    {
                        abbr = "ES";
                        break;
                    }
                case "French":
                    {
                        abbr = "FR";
                        break;
                    }
                case "German":
                    {
                        abbr = "DE";
                        break;
                    }
                case "Italian":
                    {
                        abbr = "IT";
                        break;
                    }
                case "Portuguese":
                    {
                        abbr = "PT";
                        break;
                    }
                case "Japanese":
                    {
                        abbr = "JA";
                        break;
                    }
                case "Korean":
                    {
                        abbr = "KO";
                        break;
                    }
                case "Russian":
                    {
                        abbr = "RU";
                        break;
                    }
                case "Simplified Chinese":
                    {
                        abbr = "ZHS";
                        break;
                    }
                case "Traditional Chinese":
                    {
                        abbr = "ZHT";
                        break;
                    }
                case "Hebrew":
                    {
                        abbr = "HE";
                        break;
                    }
                case "Latin":
                    {
                        abbr = "LA";
                        break;
                    }
                case "Ancient Greek":
                    {
                        abbr = "GRC";
                        break;
                    }
                case "Arabic":
                    {
                        abbr = "AR";
                        break;
                    }
                case "Sanskrit":
                    {
                        abbr = "SA";
                        break;
                    }
                case "Phyrexian":
                    {
                        abbr = "PH";
                        break;
                    }
                case "Quenya":
                    {
                        abbr = "QYA";
                        break;
                    }
                default:
                    {
                        abbr = "";
                        break;
                    }
            }
            return abbr;
        }
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            try
            {
                string scryfallBaseUrl = "https://api.scryfall.com";
                string lang = SettingsManager.ApplicationSettings.DefaultSearchLanguage;
                string abbr = AbbreviateLanguage(lang);
                if (scryfallUrl == null) scryfallUrl = $"/cards/search?include_variations=true&unique=prints&q=lang%3A{abbr}+{Query}";
                Results.Clear();
                var client = new RestClient(scryfallBaseUrl);
                var request = new RestRequest(scryfallUrl, Method.Get);
                string responseContent = client.Execute(request).Content;
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
