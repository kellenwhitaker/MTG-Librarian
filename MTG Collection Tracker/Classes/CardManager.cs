using KW.WinFormsUI.Docking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Web.UI.WebControls;

namespace MTG_Librarian
{
    public static class CardManager
    {
        private static string DefaultCurrency;
        public static CollectionViewForm LoadCollection(int id, DockState dockState = DockState.Document)
        {
            CollectionViewForm document = null;
            CardCollection collection;
            using (var context = new ScryfallCardsDbContext())
            {
                collection = (from c in context.Collections
                              where c.Id == id
                              select c).FirstOrDefault();
            }
            if (collection != null)
                document = LoadCollection(collection, dockState);

            return document;
        }

        public static CollectionViewForm LoadCollection(CardCollection collection, DockState dockState = DockState.Document)
        {
            var document = new CollectionViewForm { Collection = collection, Text = collection.CollectionName };
            document.LoadCollection();
            document.CardsDropped += EventManager.CollectionViewFormCardsDropped;
            document.CardsUpdated += EventManager.CollectionViewFormCardsUpdated;
            document.CardSelected += EventManager.CardSelected;
            document.cardListView.SmallImageList = Globals.ImageLists.SmallIconList;
            document.Show(Globals.Forms.DockPanel, dockState);
            Globals.Forms.DockPanel.ActiveDocumentPane.SetDoubleBuffered();
            return document;
        }

        public static InventoryCard AddMagicCardToCollection(ScryfallCardsDbContext context, ScryfallMagicCard magicCard, CardCollection collection, string board, int insertionIndex = 0)
        {
            var inventoryCard = new InventoryCard
            {
                DisplayName = magicCard.DisplayName,
                ScryfallId = magicCard.ScryfallId,
                CollectionId = collection.Id,
                InsertionIndex = insertionIndex,
                Virtual = collection.Virtual,
                Platform = collection.Platform,
                Board = board
            };

            if (magicCard.finishes.Length == 1)
                inventoryCard.Finish = magicCard.finishes[0];
            else if (magicCard.finishes.Contains("nonfoil"))
                inventoryCard.Finish = "nonfoil";

            if (magicCard.PartB != null)
                inventoryCard.PartB_ScryfallId = magicCard.PartB.ScryfallId;
            context.Upsert(magicCard);
            context.Library.Add(inventoryCard);
            return inventoryCard;
        }

        public static void AddMagicCardsToCollection(List<OLVCardItem> cards, CardCollection collection, string board)
        {
            DefaultCurrency = SettingsManager.ApplicationSettings.DefaultCurrency;
            var setItems = new Dictionary<string, OLVSetItem>();
            using (ScryfallCardsDbContext context = new ScryfallCardsDbContext())
            {
                int failedCards = 0;
                var cardsAdded = new List<InventoryCard>();
                int insertionIndex = 0;
                foreach (OLVCardItem cardItem in cards)
                {
                    var card = cardItem.MagicCard;
                    if (!card.games.Contains(collection.Platform.ToLower()))
                    {
                        failedCards++;
                        continue;
                    }
                    var inventoryCard = AddMagicCardToCollection(context, card, collection, board, insertionIndex);
                    cardsAdded.Add(inventoryCard);
                    insertionIndex++;
                    if (!setItems.TryGetValue(card.set_name, out OLVSetItem setItem))
                        if ((setItem = Globals.Forms.DBViewForm.SetItems.FirstOrDefault(x => x.Name == card.set_name)) != null)
                            setItems.Add(card.set_name, setItem);
                }
                context.SaveChanges();
                var cvForm = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.Collection.Id == collection.Id);
                var fullCardsAdded = new List<FullInventoryCard>();
                var DefaultPaperCurrency = SettingsManager.ApplicationSettings.DefaultPaperCurrency;
                foreach (InventoryCard card in cardsAdded)
                {
                    if (cvForm != null)
                    {
                        var fullCard = card.ToFullCard(context);
                        if (fullCard != null)
                        {
                            string priceString = "";
                            string finish = fullCard.Finish;
                            if (fullCard.Platform == "MTGO")
                                fullCard.prices.TryGetValue($"tix", out priceString);
                            else if (fullCard.Platform == "Paper")
                                fullCard.prices.TryGetValue($"{DefaultPaperCurrency.ToLower()}{(finish != "nonfoil" ? $"_{finish}" : "")}", out priceString);
                            if (!string.IsNullOrEmpty(priceString))
                                fullCard.Price = Convert.ToDouble(priceString);
                            fullCardsAdded.Add(fullCard);
                        }
                    }
                }
                context.SaveChanges();
                if (failedCards > 0)
                    MessageBox.Show($"{failedCards} card(s) were not added because they are not available on the collection's platform.");
                if (cvForm != null)
                    cvForm.AddFullInventoryCards(fullCardsAdded, board);
                EventManager.OnInventoryChanged(new InventoryChangedEventArgs { Cards = fullCardsAdded });
            }
        }
        public static void FetchPrices(List<FullInventoryCard> pricesToFetch)
        {
            var fetchPricesTask = new UpdateCardsTask(pricesToFetch.Cast<ScryfallMagicCardBase>().ToList()) { AddFirst = true };
            Globals.Forms.TasksForm.TaskManager.AddTask(fetchPricesTask);
        }

        public static void MoveFullInventoryCardsToCollection(ArrayList fullInventoryCards, CollectionViewForm sourceCVForm, CardCollection collection, string sourceBoard, string destinationBoard)
        {
            if (sourceCVForm.Collection.Platform != collection.Platform)
            {
                MessageBox.Show("Cannot move cards between collections of different platforms.");
                return;
            }
            var cardsList = new List<FullInventoryCard>();
            try
            {
                using (var context = new ScryfallCardsDbContext())
                {
                    foreach (FullInventoryCard fullInventoryCard in fullInventoryCards)
                    {
                        fullInventoryCard.CollectionId = collection.Id;
                        if (fullInventoryCard.Virtual != collection.Virtual)
                            fullInventoryCard.TimeAdded = DateTime.Now;
                        fullInventoryCard.Virtual = collection.Virtual;
                        fullInventoryCard.Board = destinationBoard;
                        context.Update(fullInventoryCard.InventoryCard);
                        cardsList.Add(fullInventoryCard);
                    }
                    context.SaveChanges();
                }

                if (string.IsNullOrEmpty(sourceBoard))
                {
                    var ids = new List<int>();
                    foreach (FullInventoryCard card in cardsList)
                        ids.Add(card.InventoryId);
                    sourceCVForm.RemoveFullInventoryCards(ids, "mainboard");
                    sourceCVForm.RemoveFullInventoryCards(ids, "sideboard");
                }

                sourceCVForm.RemoveFullInventoryCards(cardsList, sourceBoard);
                var destinationCVForm = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.Collection.Id == collection.Id);
                if (destinationCVForm != null)
                {
                    destinationCVForm.AddFullInventoryCards(cardsList, destinationBoard);
                    var destinationListView = destinationBoard == "sideboard" ? destinationCVForm.sideboardListView : destinationCVForm.cardListView;
                    destinationListView.SelectedObjects = cardsList;
                    destinationListView.Focus();
                }
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
                MessageBox.Show("Could not move cards to collection.");
            }
        }

        public static void CountInventory()
        {
            using (var context = new ScryfallCardsDbContext())
            {
                var inventoryCards = from c in context.Library
                                     select c;

                foreach (var magicCard in Globals.Collections.MagicCardCache.Values)
                    magicCard.CopiesOwned = 0;

                try
                {
                    foreach (var inventoryCard in inventoryCards)
                        if (!inventoryCard.Virtual && inventoryCard.Count.HasValue && Globals.Collections.MagicCardCache.TryGetValue(inventoryCard.ScryfallId, out ScryfallMagicCard magicCard))
                            magicCard.CopiesOwned += inventoryCard.Count.Value;
                }
                catch (Exception ex) 
                {
                    if (!ex.ToString().Contains("no such table"))
                    {
                        DebugOutput.WriteLine(ex.ToString());
                    }
                }
            }
        }

        private static void UpdateCardsInDB(ScryfallCardsDbContext context, ArrayList items)
        {
            foreach (FullInventoryCard card in items)
            {
                if (card.Count > 0)
                    context.Library.Update(card.InventoryCard);
                else
                    context.Library.Remove(card.InventoryCard);
            }
            context.SaveChanges();
        }

        public static void RetrieveImage(ScryfallMagicCardBase card, string side)
        {
            using (CardImagesDbContext context = new CardImagesDbContext(card.set_name))
            {
                var imageBytes = (from i in context.CardImages
                                  where i.ScryfallId == card.ScryfallId
                                  && i.Side == side
                                  select i).FirstOrDefault()?.CardImageBytes;

                if (imageBytes != null)
                {
                    var img = ImageExtensions.FromByteArray(imageBytes);
                    EventManager.OnCardImageRetrieved(new CardImageRetrievedEventArgs { uuid = card.ScryfallId, CardImage = img });
                }
                else
                {
                    string displayName;
                    if (card is FullInventoryCard fullInventoryCard)
                        displayName = fullInventoryCard.DisplayName;
                    else
                        displayName = card.DisplayName;

                    var imageUris = card.image_uris;
                    if (imageUris == null)
                    {
                        if (card.card_faces != null)
                        {
                            if (side == "A")
                                imageUris = card.card_faces[0].image_uris;
                            else
                                imageUris = card.card_faces[1].image_uris;
                        }
                    }
                    string imageUri = null;
                    if (imageUris != null)
                    {
                        if (!imageUris.TryGetValue("large", out imageUri))
                            if (!imageUris.TryGetValue("medium", out imageUri))
                                imageUris.TryGetValue("small", out imageUri);
                    }
                    
                    if (imageUri != null)
                        Globals.Forms.TasksForm.TaskManager.AddTask(new DownloadResourceTask { AddFirst = true, Caption = $"Card Image: {displayName}", URL = imageUri, TaskObject = new BasicCardArgs { uuid = card.ScryfallId, Side = side, Edition = card.set_name }, OnTaskCompleted = EventManager.ImageDownloadCompleted });
                }
            }
        }

        private static ScryfallMagicCard UpdateCopiesOwned(ScryfallCardsDbContext context, FullInventoryCard card)
        {
            ScryfallMagicCard magicCard = null;
            var allCopiesSum = context.LibraryView.Where(x => x.ScryfallId == card.ScryfallId && !x.Virtual).Sum(x => x.Count);
            if (allCopiesSum.HasValue && Globals.Collections.MagicCardCache.TryGetValue(card.ScryfallId, out magicCard))
                magicCard.CopiesOwned = allCopiesSum.Value;

            return magicCard;
        }

        public static void UpdateCards(CardsUpdatedEventArgs e)
        {
            var setItems = new Dictionary<string, OLVSetItem>();
            using (ScryfallCardsDbContext context = new ScryfallCardsDbContext())
            {
                try
                {
                    UpdateCardsInDB(context, e.Items);
                    EventManager.OnInventoryChanged(new InventoryChangedEventArgs { Cards = e.Items.Cast<FullInventoryCard>().ToList() });
                    var inventoryCardsToRemove = new List<FullInventoryCard>();
                    var magicCardsCopiesUpdated = new List<ScryfallMagicCard>();
                    foreach (FullInventoryCard card in e.Items)
                    {
                        if (!setItems.TryGetValue(card.set_name, out OLVSetItem setItem)) // get updated set item
                            if ((setItem = Globals.Forms.DBViewForm.SetItems.FirstOrDefault(x => x.Name == card.set_name)) != null)
                                setItems.Add(card.set_name, setItem);

                        var magicCard = UpdateCopiesOwned(context, card);
                        if (magicCard != null)
                            magicCardsCopiesUpdated.Add(magicCard);
                        if (card.Count < 1)
                            inventoryCardsToRemove.Add(card);
                    }
                    if (inventoryCardsToRemove.Count > 0)
                        e.CollectionViewForm.RemoveFullInventoryCards(inventoryCardsToRemove, e.Board);
                    else
                        e.CollectionViewForm.UpdateTotals();
                    Globals.Forms.DBViewForm.cardListView.RefreshObjects(magicCardsCopiesUpdated);
                    Globals.Forms.DBViewForm.setListView.RefreshObjects(setItems.Values.ToArray());
                }
                catch (Exception ex)
                {
                    DebugOutput.WriteLine(ex.ToString());
                    foreach (var item in e.Items)
                        if (item is FullInventoryCard card)
                            context.Entry(card).Reload();

                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    if (e.CollectionViewForm.Collection.Type == "deck")
                    {
                        e.CollectionViewForm.cardListView.BuildGroups();
                        e.CollectionViewForm.sideboardListView.BuildGroups();
                    }
                    e.CollectionViewForm.cardListView.Refresh();
                }
            }
        }
    }
}