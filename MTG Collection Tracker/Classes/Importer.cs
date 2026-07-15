using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace MTG_Librarian
{
    public class Importer
    {
        private string filePath;
        private string deckName;
        private string[] lines;
        private ScryfallCardsDbContext context;
        private List<CardImportObject> cards = new List<CardImportObject>();
        private List<CardImportObject> cardsObjectsAdded = new List<CardImportObject>();
        private List<InventoryCardBase> cardsAdded = new List<InventoryCardBase>();
        public List<CardImportObject> FailedCards { get; private set; } = new List<CardImportObject>();
        private List<string> scryfallIDs = new List<string>();
        public CardCollection NewCollection { get; private set; }
        public string Platform { get; set; }
        public int CardCount => cards.Count;
        public int NumLines => lines != null ? lines.Length : 0;
        public FileFormat FileFormat { get; set; }
        public Importer(string filePath, string deckName, FileFormat fileFormat)
        {
            this.filePath = filePath;
            this.deckName = deckName;
            this.FileFormat = fileFormat;

            lines = System.IO.File.ReadAllLines(filePath);
        }

        public bool Parse()
        {
            if (FileFormat == FileFormat.MTGODek)
            {
                return ParseMTGODek();
            }
            else if (FileFormat == FileFormat.MTGOText)
            {
                return ParseMTGOText();
            }
            else if (FileFormat == FileFormat.MTGAText)
            {
                return ParseArenaText();
            }
            return false;
        }
        public void BeginImport()
        {
            context = new ScryfallCardsDbContext();
            context.Database.BeginTransaction();
            var decksId = context.CollectionGroups.Where(d => d.GroupName == "Decks").Select(d => d.Id).FirstOrDefault();
            NewCollection = new CardCollection()
            {
                CollectionName = deckName,
                GroupId = decksId,
                GroupName = "Decks",
                Type = "deck",
                Platform = Platform
            };
            context.Collections.Add(NewCollection);
            context.SaveChanges();
        }
        public void CommitImport()
        {
            context.SaveChanges();
            UpdateCommander();
            context.Database.CommitTransaction();
            context.Dispose();
        }
        public void CancelImport()
        {
            context.Database.RollbackTransaction();
            context.Dispose();
        }
        private void UpdateCommander()
        {
            var commanderCard = cardsAdded.FirstOrDefault(c => c.Board == "commander");
            if (commanderCard != null)
            {
                commanderCard.Board = "mainboard";
                NewCollection.Commander = commanderCard.InventoryId;
                context.Update(NewCollection);
                context.Update(commanderCard);
                context.SaveChanges();
            }
            else if (cardsAdded.Sum(x => x.Count) == 100)
            {
                var sideboardCards = cardsObjectsAdded.Where(c => c.Board == "sideboard").ToList();
                var mainboardCards = cardsObjectsAdded.Where(c => c.Board == "mainboard").ToList();
                if (sideboardCards.Count == 1 && sideboardCards[0].Typeline.Contains("Legendary") && sideboardCards[0].Typeline.Contains("Creature"))
                {
                    var sideboardCard = cardsAdded.FirstOrDefault(c => c.Board == "sideboard");
                    if (sideboardCard != null)
                    {
                        NewCollection.Commander = sideboardCard.InventoryId;
                        sideboardCard.Board = "mainboard";
                        context.Update(NewCollection);
                        context.Update(sideboardCard);
                        context.SaveChanges();
                    }
                }
                else if (mainboardCards.Count == 1 && mainboardCards[0].Typeline.Contains("Legendary") && mainboardCards[0].Typeline.Contains("Creature"))
                {
                    var mainboardCard = cardsAdded.FirstOrDefault(c => c.Board == "mainboard");
                    if (mainboardCard != null)
                    {
                        NewCollection.Commander = mainboardCard.InventoryId;
                        context.Update(NewCollection);
                    }
                    foreach (var card in cardsAdded)
                    {
                        card.Board = "mainboard";
                        context.Update(card);
                    }
                    context.SaveChanges();
                }
            }
        }
        public bool ImportNextCard(out int delay)
        {           
            void AddOrInsertScryfallCard(ScryfallCard scryfallCard)
            {
                if (!scryfallIDs.Contains(scryfallCard.ScryfallId))
                {
                    scryfallIDs.Add(scryfallCard.ScryfallId);
                    context.Upsert(scryfallCard);
                    context.SaveChanges();
                }
            }

            bool ValidateCard(ScryfallCard scryfallCard)
            {
                return scryfallCard != null && scryfallCard.games.Contains(Platform.ToLower());
            }

            delay = 100;
            if (cards.Count == 0)
            {
                delay = 0;
                return false;
            }
            var card = cards[0];
            cards.RemoveAt(0);
            // Import the card into the collection
            if (card.MTGOId != 0)
            {
                var catalogCard = context.Catalog.FirstOrDefault(c => c.mtgo_id == card.MTGOId || c.mtgo_foil_id == card.MTGOId);
                if (ValidateCard(catalogCard))
                {
                    delay = 0;
                    AddCardObjectToInventory(card, catalogCard);
                    return true;
                }
                var scryfallCard = SearchScryfallByMtgoId(card.MTGOId);
                if (ValidateCard(scryfallCard))
                {
                    AddOrInsertScryfallCard(scryfallCard);
                    AddCardObjectToInventory(card, scryfallCard);
                    return true;
                }
            }
            else if (!string.IsNullOrEmpty(card.Set) || !string.IsNullOrEmpty(card.SetCode))
            {
                if (string.IsNullOrEmpty(card.SetCode))
                {
                    card.SetCode = context.Catalog.Where(c => c.set_name == card.Set).Select(c => c.set).FirstOrDefault();
                }
                if (!string.IsNullOrEmpty(card.CollectorNumber))
                {
                    var catalogCard = context.Catalog.FirstOrDefault(c => c.set == card.SetCode || c.set_name == card.Set && c.collector_number == card.CollectorNumber);
                    if (ValidateCard(catalogCard))
                    {
                        delay = 0;
                        AddCardObjectToInventory(card, catalogCard);
                        return true;
                    }
                    if (!string.IsNullOrEmpty(card.SetCode))
                    {
                        var scryfallCard = SearchScryfallBySetAndCollectorNumber(card.SetCode, card.CollectorNumber);
                        if (ValidateCard(scryfallCard))
                        {
                            AddOrInsertScryfallCard(scryfallCard);
                            AddCardObjectToInventory(card, scryfallCard);
                            return true;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(card.CardName))
                {
                    if (!string.IsNullOrEmpty(card.SetCode))
                    {
                        var scryfallCard = SearchScryfallBySetAndName(card.SetCode, card.CardName);
                        delay = 500;
                        if (ValidateCard(scryfallCard))
                        {
                            AddOrInsertScryfallCard(scryfallCard);
                            AddCardObjectToInventory(card, scryfallCard);
                            return true;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(card.CardName))
            {
                var scryfallCard = SearchScryfallByName(card.CardName);
                delay = 500;
                if (ValidateCard(scryfallCard))
                {
                    AddOrInsertScryfallCard(scryfallCard);
                    AddCardObjectToInventory(card, scryfallCard);
                    return true;
                }
            }
            FailedCards.Add(card);
            return true;
        }

        private ScryfallCard SearchScryfallByName(string cardName)
        {
            string scryfallBaseUrl = "https://api.scryfall.com";
            if (cardName.Contains("//"))
            {
                cardName = cardName.Split(new[] { "//" }, StringSplitOptions.None)[0].Trim();
            }
            cardName = cardName.Replace(" ", "+").Trim();
            var scryfallUrl = $"/cards/named?fuzzy={Uri.EscapeUriString(cardName)}&game%3A{Platform}";
            var client = new RestClient(scryfallBaseUrl);
            var request = new RestRequest(scryfallUrl, Method.Get)
            {
                Timeout = TimeSpan.FromSeconds(15)
            };
            request.AddHeader("Accept", "application/json");
            request.AddHeader("User-Agent", $"MTG Librarian/{SettingsManager.ApplicationSettings.ApplicationVersion}");
            var response = client.Execute(request);
            if (response.ResponseStatus == ResponseStatus.TimedOut || response.ResponseStatus == ResponseStatus.Error)
            {
                return null;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            string responseContent = response.Content;
            var responseObject = JsonConvert.DeserializeObject<ScryfallCard>(responseContent);
            return responseObject;
        }

        private ScryfallCard SearchScryfallBySetAndName(string setCode, string cardName)
        {
            string scryfallBaseUrl = "https://api.scryfall.com";
            if (cardName.Contains("//"))
            {
                cardName = cardName.Split(new[] { "//" }, StringSplitOptions.None)[0].Trim();
            }
            cardName = cardName.Replace(" ", "+").Trim();
            var scryfallUrl = $"/cards/named?set={setCode}&fuzzy={Uri.EscapeUriString(cardName)}";
            var client = new RestClient(scryfallBaseUrl);
            var request = new RestRequest(scryfallUrl, Method.Get)
            {
                Timeout = TimeSpan.FromSeconds(15)
            };
            request.AddHeader("Accept", "application/json");
            request.AddHeader("User-Agent", $"MTG Librarian/{SettingsManager.ApplicationSettings.ApplicationVersion}");
            var response = client.Execute(request);
            if (response.ResponseStatus == ResponseStatus.TimedOut || response.ResponseStatus == ResponseStatus.Error)
            {
                return null;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            string responseContent = response.Content;
            var responseObject = JsonConvert.DeserializeObject<ScryfallCard>(responseContent);
            return responseObject;
        }

        private ScryfallCard SearchScryfallBySetAndCollectorNumber(string setCode, string collectorNumber)
        {
            string scryfallBaseUrl = "https://api.scryfall.com";
            var scryfallUrl = $"/cards/{setCode}/{collectorNumber}";
            var client = new RestClient(scryfallBaseUrl);
            var request = new RestRequest(scryfallUrl, Method.Get)
            {
                Timeout = TimeSpan.FromSeconds(15)
            };
            request.AddHeader("Accept", "application/json");
            request.AddHeader("User-Agent", $"MTG Librarian/{SettingsManager.ApplicationSettings.ApplicationVersion}");
            var response = client.Execute(request);
            if (response.ResponseStatus == ResponseStatus.TimedOut || response.ResponseStatus == ResponseStatus.Error)
            {
                return null;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            string responseContent = response.Content;
            var responseObject = JsonConvert.DeserializeObject<ScryfallCard>(responseContent);
            return responseObject;
        }

        private void AddCardObjectToInventory(CardImportObject card, ScryfallCard catalogCard)
        {
            var inventoryCard = new InventoryCardBase
            {
                CollectionId = NewCollection.Id,
                ScryfallId = catalogCard.ScryfallId,
                Count = card.Quantity,
                Board = card.Board,
                Platform = NewCollection.Platform
            };

            card.ScryfallId = catalogCard.ScryfallId;
            card.Typeline = catalogCard.type_line;
            cardsObjectsAdded.Add(card);
            cardsAdded.Add(inventoryCard);
            context.Library.Add(inventoryCard);
        }
        private ScryfallCard SearchScryfallByMtgoId(int MTGOId)
        {
            string scryfallBaseUrl = "https://api.scryfall.com";
            var scryfallUrl = $"/cards/mtgo/{MTGOId}";
            var client = new RestClient(scryfallBaseUrl);
            var request = new RestRequest(scryfallUrl, Method.Get)
            {
                Timeout = TimeSpan.FromSeconds(15)
            };
            request.AddHeader("Accept", "application/json");
            request.AddHeader("User-Agent", $"MTG Librarian/{SettingsManager.ApplicationSettings.ApplicationVersion}");
            var response = client.Execute(request);
            if (response.ResponseStatus == ResponseStatus.TimedOut || response.ResponseStatus == ResponseStatus.Error)
            {
                return null;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            string responseContent = response.Content;
            var responseObject = JsonConvert.DeserializeObject<ScryfallCard>(responseContent);
            return responseObject;
        }
        private bool ParseMTGOText()
        {
            var quantityRegex = new Regex(@"(\d+) ");
            var nameRegex = new Regex(@"(\d+) (.+)");
            var setRegex = new Regex(@"\[([a-zA-Z0-9]+)\]");
            var collectorNumberRegex = new Regex(@"\[([a-zA-Z0-9]+)\] (\d+)");
            string board = "mainboard";
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    board = "sideboard";
                    continue;
                }
                var card = new CardImportObject();
                card.Board = board;
                var quantityMatch = quantityRegex.Match(line);
                if (quantityMatch.Success)
                {
                    card.Quantity = int.Parse(quantityMatch.Groups[1].Value);
                }
                var nameMatch = nameRegex.Match(line);
                if (nameMatch.Success)
                {
                    card.CardName = nameMatch.Groups[2].Value.Trim();
                }
                var setMatch = setRegex.Match(line);
                if (setMatch.Success)
                {
                    card.SetCode = setMatch.Groups[1].Value.Trim();
                }
                var collectorNumberMatch = collectorNumberRegex.Match(line);
                if (collectorNumberMatch.Success)
                {
                    card.CollectorNumber = collectorNumberMatch.Groups[2].Value.Trim();
                }
                if (!string.IsNullOrEmpty(card.CardName))
                {
                    cards.Add(card);
                }
            }
            return true;
        }

        private bool ParseArenaText()
        {
            var quantityRegex = new Regex(@"(\d+) ");
            var nameRegex = new Regex(@"(\d+) (.+)");
            var setRegex = new Regex(@"\(([^)]+)\)");
            var collectorNumberRegex = new Regex(@"\(([^)]+)\) (\d+)");
            string board = null;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.Trim().ToLower() == "sideboard")
                {
                    board = "sideboard";
                    continue;
                }
                else if (line.Trim().ToLower() == "deck")
                {
                    board = "mainboard";
                    continue;
                }
                else if (line.Trim().ToLower() == "commander")
                {
                    board = "commander";
                    continue;
                }
                var card = new CardImportObject();
                card.Board = board;
                var quantityMatch = quantityRegex.Match(line);
                if (quantityMatch.Success)
                {
                    card.Quantity = int.Parse(quantityMatch.Groups[1].Value);
                }
                var nameMatch = nameRegex.Match(line);
                if (nameMatch.Success)
                {
                    card.CardName = nameMatch.Groups[2].Value.Trim();
                }
                var setMatch = setRegex.Match(line);
                if (setMatch.Success)
                {
                    card.SetCode = setMatch.Groups[1].Value.Trim();
                }
                var collectorNumberMatch = collectorNumberRegex.Match(line);
                if (collectorNumberMatch.Success)
                {
                    card.CollectorNumber = collectorNumberMatch.Groups[2].Value.Trim();
                }
                if (!string.IsNullOrEmpty(card.CardName))
                {
                    cards.Add(card);
                }
            }
            return true;
        }
        private bool ParseMTGODek()
        {
            var nameRegex = new Regex($"Name=\"(.*?)\"");
            var idRegex = new Regex($"CatID=\"(\\d+)\"");
            var quantityRegex = new Regex($"Quantity=\"(\\d+)\"");
            var sideboardRegex = new Regex($"Sideboard=\"(.*?)\"");
            foreach (var line in lines)
            {                 
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (!line.Trim().StartsWith("<Cards"))
                    continue;

                var card = new CardImportObject();
                var match = nameRegex.Match(line);
                if (match.Success)
                {
                    card.CardName = match.Groups[1].Value;
                }
                match = idRegex.Match(line);
                if (match.Success)
                {
                    card.MTGOId = int.Parse(match.Groups[1].Value);
                }
                match = quantityRegex.Match(line);
                if (match.Success)
                {
                    card.Quantity = int.Parse(match.Groups[1].Value);
                }
                match = sideboardRegex.Match(line);
                if (match.Success)
                {
                    card.Board = match.Groups[1].Value.ToLower() == "true" ? "sideboard" : "mainboard";
                }

                if (card.MTGOId != 0 || !string.IsNullOrEmpty(card.CardName))
                {
                    cards.Add(card);
                }
            }

            return true;
        }
    }

    public enum FileFormat
    {
        MTGODek,
        MTGOText,
        MTGAText
    }

    public class CardImportObject
    {
        public int Quantity { get; set; }
        public string CardName { get; set; }
        public string Set { get; set; }
        public string SetCode { get; set; }
        public string CollectorNumber { get; set; }
        public int MTGOId { get; set; } = 0;
        public string ScryfallId { get; set; } = null;
        public string Board { get; set; } = "mainboard";
        public string Typeline { get; set; }
    }
}
