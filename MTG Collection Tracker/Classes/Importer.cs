using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace MTG_Librarian
{
    public class Importer
    {
        private string filePath;
        private string collectionName;
        private string[] lines;
        private ScryfallCardsDbContext context;
        private List<CardImportObject> cards = new List<CardImportObject>();
        private List<CardImportObject> cardsObjectsAdded = new List<CardImportObject>();
        public List<InventoryCardBase> cardsAdded = new List<InventoryCardBase>();
        public List<CardImportObject> FailedCards { get; private set; } = new List<CardImportObject>();
        private List<CardImportObject> uncataloguedCards = new List<CardImportObject>();
        private Dictionary<string, int> setCounts = new Dictionary<string, int>();
        private List<string> scryfallIDs = new List<string>();
        private List<CardImportObject> batchableCards = new List<CardImportObject>();
        private int batchNumber = 0;
        public CardCollection NewCollection { get; private set; }
        public CardCollection ExistingCollection { get; set; } = null;
        public Dictionary<string, CardCollection> Collections { get; private set; } = new Dictionary<string, CardCollection>();
        public string Platform { get; set; }
        public string CollectionType { get; set; } = "deck";
        public int CardCount => cards.Count;
        public int SetCount => setCounts.Count;
        public int UncataloguedCount => uncataloguedCards.Count;
        public int BatchableCount => batchableCards.Count;
        public int NumLines => lines != null ? lines.Length : 0;
        public bool MultipleCollections { get; set; } = false;
        public FileFormat FileFormat { get; set; }
        public Importer(string filePath, string collectionName, FileFormat fileFormat)
        {
            this.filePath = filePath;
            this.collectionName = collectionName;
            this.FileFormat = fileFormat;
        }
        public bool Parse()
        {
            if (FileFormat != FileFormat.CSV)
                lines = System.IO.File.ReadAllLines(filePath);

            if (FileFormat == FileFormat.MTGODek)
                return ParseMTGODek();
            else if (FileFormat == FileFormat.MTGOText)
                return ParseMTGOText();
            else if (FileFormat == FileFormat.MTGAText)
                return ParseArenaText();
            else if (FileFormat == FileFormat.CSV)
                return ParseCSV();

            return false;
        }
        public void BeginImport()
        {
            if (context == null)
            {
                context = new ScryfallCardsDbContext();
                context.Database.BeginTransaction();
            }
            if (!MultipleCollections)
            {
                if (CollectionType == "deck")
                {
                    var decksId = context.CollectionGroups.Where(d => d.GroupName == "Decks").Select(d => d.Id).FirstOrDefault();
                    NewCollection = new CardCollection()
                    {
                        CollectionName = collectionName,
                        GroupId = decksId,
                        GroupName = "Decks",
                        Type = "deck",
                        Platform = Platform,
                        Virtual = true
                    };
                }
                else if (CollectionType == "collection" && ExistingCollection == null)
                {
                    var collectionsId = context.CollectionGroups.Where(d => d.GroupName == "Collections").Select(d => d.Id).FirstOrDefault();
                    NewCollection = new CardCollection()
                    {
                        CollectionName = collectionName,
                        GroupId = collectionsId,
                        GroupName = "Collections",
                        Type = "collection",
                        Platform = Platform
                    };
                }
            }
            if (NewCollection != null)
            {
                context.Collections.Add(NewCollection);
                context.SaveChanges();
            }
        }
        public void CommitImport()
        {
            context.SaveChanges();
            if (CollectionType == "deck")
            {
                UpdateCommander();
                CardManager.UpdateColorIdentity(context, NewCollection);
            }
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
            var commanderTypes = new List<string> { "Creature", "Planeswalker", "Vehicle", "Spacecraft" };
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
                if (sideboardCards.Count == 1 && sideboardCards[0].Typeline.Contains("Legendary") && commanderTypes.Any(type => sideboardCards[0].Typeline.Contains(type)))
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
                else if (mainboardCards.Count == 1 && mainboardCards[0].Typeline.Contains("Legendary") && commanderTypes.Any(type => mainboardCards[0].Typeline.Contains(type)))
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
        private bool ValidateCard(ScryfallCard scryfallCard)
        {
            return scryfallCard != null && scryfallCard.games.Contains(Platform.ToLower());
        }
        private void AddScryfallIdToList(string scryfallId)
        {
            if (!string.IsNullOrEmpty(scryfallId) && !scryfallIDs.Contains(scryfallId))
                scryfallIDs.Add(scryfallId);
        }   
        public bool ImportNextCardUsingCatalog()
        {
            if (cards.Count == 0)
                return false;

            var card = cards[0];
            cards.RemoveAt(0);
            try
            {
                if (card.ScryfallId != null)
                {
                    var catalogCard = context.Catalog.AsNoTracking().FirstOrDefault(c => c.ScryfallId == card.ScryfallId);
                    AddScryfallIdToList(catalogCard?.ScryfallId);
                    if (ValidateCard(catalogCard))
                    {
                        AddCardObjectToInventory(card, catalogCard);
                        return true;
                    }
                }
                if (card.MTGOId != 0)
                {
                    var catalogCard = context.Catalog.AsNoTracking().FirstOrDefault(c => c.mtgo_id == card.MTGOId || c.mtgo_foil_id == card.MTGOId);
                    AddScryfallIdToList(catalogCard?.ScryfallId);
                    if (ValidateCard(catalogCard))
                    {
                        AddCardObjectToInventory(card, catalogCard);
                        return true;
                    }
                }
                else if (!string.IsNullOrEmpty(card.Set) || !string.IsNullOrEmpty(card.SetCode))
                {
                    if (string.IsNullOrEmpty(card.SetCode))
                    {
                        card.SetCode = context.Catalog.Where(c => c.set_name == card.Set).Select(c => c.set).FirstOrDefault();
                    }
                    if (card.SetCode?.Length == 2)
                        card.SetCode = Globals.Methods.ConvertGathererSetCodeToScryfall(card.SetCode);

                    if (!string.IsNullOrEmpty(card.CollectorNumber))
                    {
                        var catalogCard = context.Catalog.AsNoTracking().FirstOrDefault(c => (c.set == card.SetCode || c.set_name == card.Set) && c.collector_number == card.CollectorNumber);
                        AddScryfallIdToList(catalogCard?.ScryfallId);
                        if (ValidateCard(catalogCard))
                        {
                            DebugOutput.WriteLine($"Found catalog card for {catalogCard.Name} {catalogCard.set} {catalogCard.collector_number}");
                            AddCardObjectToInventory(card, catalogCard);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine($"Error importing card {card.CardName} {card.SetCode} {card.CollectorNumber}: {ex.Message}");
            }

            if (card.SetCode != null)
            {
                if (setCounts.ContainsKey(card.SetCode))
                    setCounts[card.SetCode]++;
                else
                    setCounts[card.SetCode] = 1;
            }

            uncataloguedCards.Add(card);
            return true;
        }
        private void AddOrInsertScryfallCard(ScryfallCard scryfallCard)
        {
            if (!scryfallIDs.Contains(scryfallCard.ScryfallId))
            {
                scryfallIDs.Add(scryfallCard.ScryfallId);
                context.Upsert(scryfallCard);
                context.SaveChanges();
            }
        }
        public void FillBatchableCards()
        {
            foreach (var card in uncataloguedCards)
            {
                if (card.MTGOId != 0 || !string.IsNullOrEmpty(card.ScryfallId) || (!string.IsNullOrEmpty(card.SetCode) && !string.IsNullOrEmpty(card.CollectorNumber)))
                {
                    batchableCards.Add(card);
                }
            }
            DebugOutput.WriteLine($"Batchable cards count: {batchableCards.Count}");
        }
        public bool ImportNextBatch()
        {
            try
            {
                const int batchSize = 75; // Scryfall /cards/collection is typically limited; 75 is safe
                string scryfallBaseUrl = "https://api.scryfall.com";
                var client = new RestClient(scryfallBaseUrl);

                var start = batchNumber * batchSize;
                if (start >= batchableCards.Count)
                    return false;

                var searchCollection = new ScryfallSearchCollection();
                var batchCards = uncataloguedCards
                    .Skip(start)
                    .Take(batchSize)
                    .ToList();

                if (batchCards.Count == 0)
                    return false;

                foreach (var card in batchCards)
                {
                    DebugOutput.WriteLine($"Processing card: {card.SetCode} {card.CollectorNumber}");
                    if (!string.IsNullOrEmpty(card.ScryfallId))
                    {
                        DebugOutput.WriteLine("Adding Scryfall ID to search collection: " + card.ScryfallId);
                        searchCollection.identifiers.Add(new ScryfallSearchCollectionIdentifier { id = card.ScryfallId });
                    }
                    else if (card.MTGOId != 0)
                        searchCollection.identifiers.Add(new ScryfallSearchCollectionIdentifier { mtgo_id = card.MTGOId });
                    else if (!string.IsNullOrEmpty(card.SetCode) && !string.IsNullOrEmpty(card.CollectorNumber))
                        searchCollection.identifiers.Add(new ScryfallSearchCollectionIdentifier { set = card.SetCode, collector_number = card.CollectorNumber });
                }
                var request = new RestRequest("/cards/collection", Method.Post);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("User-Agent", $"MTG Librarian/{SettingsManager.ApplicationSettings.ApplicationVersion}");
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(JsonConvert.SerializeObject(searchCollection, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));
                var response = client.Execute(request);
                var scryfallCards = response.Content != null
                    ? JsonConvert.DeserializeObject<ScryfallCardList>(response.Content)?.data
                    : null;
                if (scryfallCards == null)
                {
                    DebugOutput.WriteLine("ScryfallSearchCollection returned null for a batch.");
                    return false;
                }

                foreach (var sfCard in scryfallCards)
                {
                    if (sfCard == null) continue;
                    var scryfallMagicCard = sfCard.ToScryfallMagicCard();
                    DebugOutput.WriteLine(scryfallMagicCard.set + ", " + scryfallMagicCard.set_id);
                    if (!scryfallIDs.Contains(scryfallMagicCard.ScryfallId))
                    {
                        scryfallIDs.Add(scryfallMagicCard.ScryfallId);
                        context.Upsert(scryfallMagicCard);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
                return false;
            }
            batchNumber++;
            return true;
        }
        public bool ImportNextCard(out int delay, bool searchByName = true)
        {
            delay = 100;
            if (cards.Count == 0)
            {
                delay = 0;
                return false;
            }
            var card = cards[0];
            cards.RemoveAt(0);
            // Import the card into the collection
            try
            {
                if (card.ScryfallId != null)
                {
                    var catalogCard = context.Catalog.AsNoTracking().FirstOrDefault(c => c.ScryfallId == card.ScryfallId);
                    AddScryfallIdToList(catalogCard?.ScryfallId);
                    if (ValidateCard(catalogCard))
                    {
                        delay = 0;
                        AddCardObjectToInventory(card, catalogCard);
                        return true;
                    }
                    var scryfallCard = SearchScryfallByScryfallId(card.ScryfallId);
                    if (ValidateCard(scryfallCard))
                    {
                        AddOrInsertScryfallCard(scryfallCard);
                        AddCardObjectToInventory(card, scryfallCard);
                        return true;
                    }
                }
                if (card.MTGOId != 0)
                {
                    var catalogCard = context.Catalog.AsNoTracking().FirstOrDefault(c => c.mtgo_id == card.MTGOId || c.mtgo_foil_id == card.MTGOId);
                    AddScryfallIdToList(catalogCard?.ScryfallId);
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
                    if (card.SetCode.Length == 2)
                        card.SetCode = Globals.Methods.ConvertGathererSetCodeToScryfall(card.SetCode);
                    if (string.IsNullOrEmpty(card.SetCode))
                    {
                        card.SetCode = context.Catalog.Where(c => c.set_name == card.Set).Select(c => c.set).FirstOrDefault();
                    }
                    if (!string.IsNullOrEmpty(card.CollectorNumber))
                    {
                        var catalogCard = context.Catalog.AsNoTracking().FirstOrDefault(c => (c.set == card.SetCode || c.set_name == card.Set) && c.collector_number == card.CollectorNumber);
                        AddScryfallIdToList(catalogCard?.ScryfallId);
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
                                delay = 100;
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
                if (searchByName && !string.IsNullOrEmpty(card.CardName))
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
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine($"Error importing card {card.CardName} {card.SetCode} {card.CollectorNumber}: {ex.Message}");
            }
            FailedCards.Add(card);
            return true;
        }
        private ScryfallCard SearchScryfallByScryfallId(string scryfallId)
        {
            DebugOutput.WriteLine($"Searching Scryfall for Scryfall ID '{scryfallId}'");
            if (string.IsNullOrWhiteSpace(scryfallId))
                return null;

            string scryfallBaseUrl = "https://api.scryfall.com";
            var escapedId = Uri.EscapeUriString(scryfallId);
            var scryfallUrl = $"/cards/{escapedId}";

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
                DebugOutput.WriteLine($"Scryfall request for id '{scryfallId}' timed out or errored: {response?.ErrorMessage}");
                return null;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                DebugOutput.WriteLine($"Scryfall returned 404 for id '{scryfallId}'");
                return null;
            }

            var responseContent = response.Content;
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                DebugOutput.WriteLine($"Scryfall returned empty content for id '{scryfallId}'");
                return null;
            }

            try
            {
                var responseObject = JsonConvert.DeserializeObject<ScryfallCard>(responseContent);
                return responseObject;
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine($"Failed to deserialize Scryfall response for id '{scryfallId}': {ex.Message}");
                return null;
            }
        }
        public bool DownloadNextSet()
        {
            if (setCounts.Count == 0)
                return false;

            var setCode = setCounts.Keys.First();
            var setCount = setCounts.Values.First();
            if (setCount >= 20)
            {
                var scryfallCards = SearchScryfallBySet(setCode);
                if (scryfallCards != null && scryfallCards.Count > 0)
                {
                    foreach (var card in scryfallCards)
                    {
                        if (!scryfallIDs.Contains(card.ScryfallId))
                        {
                            scryfallIDs.Add(card.ScryfallId);
                            context.Upsert(card);
                        }
                    }
                    context.SaveChanges();
                }
            }
            setCounts.Remove(setCode);
            return true;
        }
        private List<ScryfallCard> SearchScryfallBySet(string setCode)
        {
            string scryfallBaseUrl = "https://api.scryfall.com";
            // Build initial search URL: search query for set:<setCode>
            var scryfallUrl = $"/cards/search?q=set%3A{setCode}";
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
            var pageResult = JsonConvert.DeserializeObject<ScryfallCardList>(responseContent);
            if (pageResult == null || pageResult.data == null)
                return null;

            // If multiple pages, follow next_page links and accumulate results
            var allCards = new List<ScryfallCard>();
            allCards.AddRange(pageResult.data);

            while (pageResult.has_more && !string.IsNullOrEmpty(pageResult.next_page))
            {
                try
                {
                    // next_page is a full URL; create a new client for it
                    client = new RestClient(pageResult.next_page);
                    request = new RestRequest(string.Empty, Method.Get)
                    {
                        Timeout = TimeSpan.FromSeconds(15)
                    };
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("User-Agent", $"MTG Librarian/{SettingsManager.ApplicationSettings.ApplicationVersion}");
                    response = client.Execute(request);
                    if (response.ResponseStatus == ResponseStatus.TimedOut || response.ResponseStatus == ResponseStatus.Error)
                    {
                        break;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        break;
                    }
                    responseContent = response.Content;
                    pageResult = JsonConvert.DeserializeObject<ScryfallCardList>(responseContent);
                    if (pageResult?.data != null)
                    {
                        allCards.AddRange(pageResult.data);
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    break;
                }
                Thread.Sleep(500); // Sleep for 500ms to avoid hitting rate limits
            }

            return allCards;
        }
        private ScryfallCard SearchScryfallByName(string cardName)
        {
            DebugOutput.WriteLine($"Searching Scryfall for card with name '{cardName}'");
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
            DebugOutput.WriteLine($"Searching Scryfall for card with set code '{setCode}' and name '{cardName}'");
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
            DebugOutput.WriteLine($"Searching Scryfall for card with set code '{setCode}' and collector number '{collectorNumber}'");
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
                ScryfallId = catalogCard.ScryfallId,
                Count = card.Quantity,
                Board = card.Board,
                Platform = Platform,
                Finish = card.Finish,
                Condition = card.Condition,
                Cost = card.Cost,
                TimeAdded = card.TimeAcquired,
                SoldTime = card.SoldTime,
                SoldPrice = card.SoldPrice,
                Virtual = card.Virtual
            };

            if (!MultipleCollections)
                inventoryCard.CollectionId = ExistingCollection != null ? ExistingCollection.Id : NewCollection.Id;
            else
                inventoryCard.CollectionId = card.CollectionId;
  
            if (catalogCard.finishes != null && catalogCard.finishes.Length == 1)
            {
                inventoryCard.Finish = catalogCard.finishes[0];
            }
            if (card.MTGOId != 0 && card.MTGOId == catalogCard.mtgo_id)
                inventoryCard.Finish = "nonfoil";
            else if (card.MTGOId != 0 && card.MTGOId == catalogCard.mtgo_foil_id)
                inventoryCard.Finish = "foil";

            card.ScryfallId = catalogCard.ScryfallId;
            if (CollectionType == "deck")
            {
                card.Typeline = catalogCard.type_line;
            }
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
        /// <summary>
        /// MTGGoldfish, MTGO, EchoMTG, MTG Collection Builder, Archidekt, Deckbox, Deckstats,
        /// Topdecked, UrzaGatherer, ManaBox, Tappedout, Dragon Shield, Moxfield, MTG Studio
        /// </summary>
        private bool ParseCSV()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Prevents an exception if a header name doesn't exist
                MissingFieldFound = null,

                // Prevents an exception if data cannot convert to the target type
                BadDataFound = null,
                PrepareHeaderForMatch = args => args.Header.ToLower().Replace("_", " "),
                ShouldSkipRecord = record => record.Row.ColumnCount < 2
            };
            
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();
                string cardNameField = null;
                string setCodeField = null;
                string setNameField = null;
                string quantityField = null;
                string collectorNumberField = null;
                string scryfallIdField = null;
                string finishField = null;
                string conditionField = null;
                string costField = null;
                string timeAcquiredField = null;
                string collectionField = null;
                string timeSoldField = null;
                string priceSoldField = null;
                if (csv.HeaderRecord.Contains("Card"))
                    cardNameField = "card";
                if (csv.HeaderRecord.Contains("Card Name") || csv.HeaderRecord.Contains("card_name"))
                    cardNameField = "card name";
                if (csv.HeaderRecord.Contains("name", StringComparer.OrdinalIgnoreCase))
                    cardNameField = "name";
                if (csv.HeaderRecord.Contains("Set ID"))
                    setCodeField = "set id";
                if (csv.HeaderRecord.Contains("SETCODE"))
                    setCodeField = "setcode";
                if (csv.HeaderRecord.Contains("SetAbbreviation"))
                    setCodeField = "setabbreviation";
                if (csv.HeaderRecord.Contains("Set") && !(csv.HeaderRecord.Contains("MTG CB ID") || csv.HeaderRecord.Contains("Total Value")))
                    setCodeField = "set";
                else if (csv.HeaderRecord.Contains("Set") && (csv.HeaderRecord.Contains("MTG CB ID") || csv.HeaderRecord.Contains("Total Value")))
                    setNameField = "set";
                if (csv.HeaderRecord.Contains("set_code") || csv.HeaderRecord.Contains("Set code", StringComparer.OrdinalIgnoreCase))
                    setCodeField = "set code";
                if (csv.HeaderRecord.Contains("Edition Code"))
                    setCodeField = "edition code";
                if (csv.HeaderRecord.Contains("Set Name", StringComparer.OrdinalIgnoreCase))
                    setNameField = "set name";
                if (csv.HeaderRecord.Contains("SETNAME", StringComparer.OrdinalIgnoreCase))
                    setNameField = "setname";
                if (csv.HeaderRecord.Contains("Edition Name"))
                    setNameField = "edition name";
                if (csv.HeaderRecord.Contains("Edition") && !csv.HeaderRecord.Contains("Last Modified"))
                    setNameField = "edition";
                else if (csv.HeaderRecord.Contains("Edition") && csv.HeaderRecord.Contains("Last Modified"))
                    setCodeField = "edition";
                if (csv.HeaderRecord.Contains("expansion"))
                    setNameField = "expansion";
                if (csv.HeaderRecord.Contains("Quantity", StringComparer.OrdinalIgnoreCase))
                    quantityField = "quantity";
                if (csv.HeaderRecord.Contains("Qty"))
                    quantityField = "qty";
                if (csv.HeaderRecord.Contains("Count"))
                    quantityField = "count";
                if (csv.HeaderRecord.Contains("amount"))
                    quantityField = "amount";
                if (csv.HeaderRecord.Contains("Collector Number", StringComparer.OrdinalIgnoreCase) || csv.HeaderRecord.Contains("collector_number"))
                    collectorNumberField = "collector number";
                if (csv.HeaderRecord.Contains("Collector #"))
                    collectorNumberField = "collector #";
                if (csv.HeaderRecord.Contains("CollectorNoSortable"))
                    collectorNumberField = "collectornosortable";
                if (csv.HeaderRecord.Contains("Card Number"))
                    collectorNumberField = "card number";
                if (csv.HeaderRecord.Contains("Number"))
                    collectorNumberField = "number";
                if (csv.HeaderRecord.Contains("Set Number"))
                    collectorNumberField = "set number";
                if (csv.HeaderRecord.Contains("Scryfall ID"))
                    scryfallIdField = "scryfall id";
                if (csv.HeaderRecord.Contains("ScryfallId"))
                    scryfallIdField = "scryfallid";
                if (csv.HeaderRecord.Contains("Foil", StringComparer.OrdinalIgnoreCase))
                    finishField = "foil";
                if (csv.HeaderRecord.Contains("is_foil"))
                    finishField = "is foil";
                if (csv.HeaderRecord.Contains("Finish", StringComparer.OrdinalIgnoreCase))
                    finishField = "finish";
                if (csv.HeaderRecord.Contains("Premium"))
                    finishField = "premium";
                if (csv.HeaderRecord.Contains("Printing"))
                    finishField = "printing";
                if (csv.HeaderRecord.Contains("condition", StringComparer.OrdinalIgnoreCase))
                    conditionField = "condition";
                if (csv.HeaderRecord.Contains("price_acquired"))
                    costField = "price acquired";
                if (csv.HeaderRecord.Contains("Purchase Price", StringComparer.OrdinalIgnoreCase))
                    costField = "purchase price";
                if (csv.HeaderRecord.Contains("ACQUIRED PRICE"))
                    costField = "acquired price";
                if (csv.HeaderRecord.Contains("Price Bought"))
                    costField = "price bought";
                if (csv.HeaderRecord.Contains("date_acquired"))
                    timeAcquiredField = "date acquired";
                if (csv.HeaderRecord.Contains("Date Added"))
                    timeAcquiredField = "date added";
                if (csv.HeaderRecord.Contains("added", StringComparer.OrdinalIgnoreCase))
                    timeAcquiredField = "added";
                if (csv.HeaderRecord.Contains("ACQUIRED DATE"))
                    timeAcquiredField = "acquired date";
                if (csv.HeaderRecord.Contains("Date Bought"))
                    timeAcquiredField = "date bought";
                if (csv.HeaderRecord.Contains("binder name", StringComparer.OrdinalIgnoreCase))
                    collectionField = "binder name";
                if (csv.HeaderRecord.Contains("folder name", StringComparer.OrdinalIgnoreCase))
                    collectionField = "folder name";
                if (csv.HeaderRecord.Contains("Sold Date"))
                    timeSoldField = "sold date";
                if (csv.HeaderRecord.Contains("Sold Price"))
                    priceSoldField = "sold price";

                while (csv.Read())
                {
                    var record = new CardImportObject
                    {
                        CardName = cardNameField != null ? csv.GetField(cardNameField) : null,
                        SetCode = setCodeField != null ? csv.GetField(setCodeField)?.ToLower() : null,
                        Set = setNameField != null ? csv.GetField(setNameField) : null,
                        Quantity = quantityField != null ? csv.GetField<int>(quantityField) : 1,
                        CollectorNumber = collectorNumberField != null ? csv.GetField(collectorNumberField) : null,
                        ScryfallId = scryfallIdField != null ? csv.GetField(scryfallIdField) : null,
                        Finish = finishField != null ? csv.GetField(finishField)?.ToLower() : "nonfoil",
                        Condition = conditionField != null ? csv.GetField(conditionField) : null,
                        SoldTime = timeSoldField != null ? csv.GetField<DateTime?>(timeSoldField) : null,
                        SoldPrice = priceSoldField != null ? csv.GetField<double?>(priceSoldField) : null
                    };

                    DebugOutput.WriteLine($"Parsed CSV record: {record.CardName}, Set: {record.SetCode}, Collector Number: {record.CollectorNumber}, Quantity: {record.Quantity}, Finish: {record.Finish}, Condition: {record.Condition}");
         
                    if (collectionField != null)
                    {
                        var cardCollectionName = csv.GetField(collectionField);
                        CardCollection existingCollection;
                        if (Collections.TryGetValue(cardCollectionName, out existingCollection))
                        {
                            record.CollectionId = existingCollection.Id;
                            record.Virtual = existingCollection.Virtual;
                        }
                        else
                        {
                            if (context == null)
                            {
                                context = new ScryfallCardsDbContext();
                                context.Database.BeginTransaction();
                            }
                            var match = (from c in context.Collections
                                         where c.CollectionName == cardCollectionName && c.Platform == Platform
                                         select c).FirstOrDefault();

                            if (match != null)
                            {
                                record.CollectionId = match.Id;
                                Collections[cardCollectionName] = match;
                                record.Virtual = match.Virtual;
                            }
                            else
                            {
                                var collectionsGroup = context.CollectionGroups.FirstOrDefault(g => g.GroupName == "Collections");
                                var newCollection = new CardCollection
                                {
                                    CollectionName = cardCollectionName,
                                    Platform = Platform,
                                    GroupId = collectionsGroup?.Id ?? 0,
                                    GroupName = "Collections",
                                    Type = "collection"
                                };
                                context.Collections.Add(newCollection);
                                context.SaveChanges();
                                record.CollectionId = newCollection.Id;
                                Collections[cardCollectionName] = newCollection;
                            }

                        }
                    }

                    if (record.CollectorNumber != null && record.CollectorNumber.Contains("/"))
                    {
                        var parts = record.CollectorNumber.Split('/');
                        if (parts.Length == 2)
                        {
                            record.CollectorNumber = parts[0].Trim();
                        }
                    }   

                    var condition = record.Condition?.ToLower().Replace("_", " ");
                    if (condition == "near mint" || condition == "excellent" || condition == "nearmint" 
                        || condition == "fi")
                        record.Condition = "NM";
                    else if (condition == "good (lightly played)" || condition == "good" || condition == "lightly played" 
                        || condition == "sl" || condition == "lightplayed" || condition == "go")
                        record.Condition = "LP";
                    else if (condition == "played" || condition == "moderately played" || condition == "fa")
                        record.Condition = "MP";
                    else if (condition == "heavily played" || condition == "po" || condition == "poor")
                        record.Condition = "HP";
                    else if (condition == "damaged" || condition == "dmg")
                        record.Condition = "DG";

                    if (costField != null)
                        record.Cost = csv.GetField<double?>(costField);

                    if (timeAcquiredField != null)
                    {
                        var timeAcquiredString = csv.GetField(timeAcquiredField);
                        if (DateTime.TryParse(timeAcquiredString, out DateTime timeAcquired))
                        {
                            record.TimeAcquired = timeAcquired;
                        }
                    }

                    if (record.Finish != null)
                    {
                        if (record.Finish == "regular" || record.Finish == "normal" || string.IsNullOrEmpty(record.Finish))
                            record.Finish = "nonfoil";
                        else if (record.Finish == "yes" || record.Finish == "1.0" || record.Finish == "1" 
                            || record.Finish == "f" || record.Finish == "surge foil" || record.Finish == "true")
                            record.Finish = "foil";
                        else if (record.Finish == "no" || record.Finish == "0.0" || record.Finish == "0" 
                            || record.Finish == "non-foil" || record.Finish == "-" || string.IsNullOrEmpty(record.Finish)
                            || record.Finish == "false")
                            record.Finish = "nonfoil";
                        else if (record.Finish == "f-etch")
                            record.Finish = "etched";
                    }
                    if (record.SetCode == "pc1")
                        record.SetCode = "hop";

                    if (csv.HeaderRecord.Contains("Tradelist Count") && !csv.HeaderRecord.Contains("Last Modified"))
                    {
                        var count = csv.GetField<int>("Tradelist Count");
                        record.Quantity += count;
                    }
                    else if (csv.HeaderRecord.Contains("Trade Quantity"))
                    {
                        var count = csv.GetField<int>("Trade Quantity");
                        record.Quantity += count;
                    }

                    if (csv.HeaderRecord.Contains("Special foil count"))
                    {
                        var specialCount = csv.GetField<int>("Special foil count");
                        var foilCount = csv.GetField<int>("Foil count");
                        var regularCount = record.Quantity - foilCount - specialCount;
                        if (regularCount > 0)
                        {
                            record.Quantity = regularCount;
                            record.Finish = "nonfoil";
                            record.Condition = null;
                            cards.Add(record);
                        }
                        if (foilCount > 0)
                        {
                            var foilRecord = new CardImportObject
                            {
                                CardName = record.CardName,
                                SetCode = record.SetCode,
                                Set = record.Set,
                                Quantity = foilCount,
                                CollectorNumber = record.CollectorNumber,
                                ScryfallId = record.ScryfallId,
                                Finish = "foil",
                                Condition = null,
                                Cost = record.Cost,
                                TimeAcquired = record.TimeAcquired
                            };
                            cards.Add(foilRecord);
                        }
                        if (specialCount > 0)
                        {
                            var specialRecord = new CardImportObject
                            {
                                CardName = record.CardName,
                                SetCode = record.SetCode,
                                Set = record.Set,
                                Quantity = specialCount,
                                CollectorNumber = record.CollectorNumber,
                                ScryfallId = record.ScryfallId,
                                Finish = "etched",
                                Condition = null,
                                Cost = record.Cost,
                                TimeAcquired = record.TimeAcquired
                            };
                            cards.Add(specialRecord);
                        }
                        continue;
                    }

                    if (csv.HeaderRecord.Contains("MTG CB ID"))
                    {
                        var regularCount = csv.GetField<int>("Quantity (Regular)");
                        var foilCount = csv.GetField<int>("Quantity (Foil)");
                        if (regularCount > 0)
                        {
                            record.Quantity = regularCount;
                            record.Finish = "nonfoil";
                            cards.Add(record);
                        }
                        if (foilCount > 0)
                        {
                            var foilRecord = new CardImportObject
                            {
                                CardName = record.CardName,
                                SetCode = record.SetCode,
                                Set = record.Set,
                                Quantity = foilCount,
                                CollectorNumber = record.CollectorNumber,
                                ScryfallId = record.ScryfallId,
                                Finish = "foil",
                                Condition = record.Condition,
                                Cost = record.Cost,
                                TimeAcquired = record.TimeAcquired
                            };
                            cards.Add(foilRecord);
                        }
                        continue;
                    }

                    cards.Add(record);
                }
            }
            return true;
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
                card.Virtual = NewCollection != null ? NewCollection.Virtual : ExistingCollection.Virtual;
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
                card.Virtual = NewCollection != null ? NewCollection.Virtual : ExistingCollection.Virtual;
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
                card.Virtual = NewCollection != null ? NewCollection.Virtual : ExistingCollection.Virtual;
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

        internal void CopyUncataloguedCards()
        {
            cards = uncataloguedCards;
        }
    }

    public enum FileFormat
    {
        MTGODek,
        MTGOText,
        MTGAText,
        CSV
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
        public string Finish { get; set; }
        public string Condition { get; set; }
        public double? Cost { get; set; }
        public DateTime? TimeAcquired { get; set; }
        public int CollectionId { get; set; }
        public double? SoldPrice { get; set; }
        public DateTime? SoldTime { get; set; }
        public bool Virtual { get; set; }
    }
}
