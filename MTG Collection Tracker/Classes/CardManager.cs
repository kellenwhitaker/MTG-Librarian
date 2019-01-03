using KW.WinFormsUI.Docking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public static class CardManager
    {
        public static CollectionViewForm LoadCollection(int id, DockState dockState = DockState.Document)
        {
            CollectionViewForm document = null;
            CardCollection collection;
            using (var context = new MyDbContext())
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

        public static InventoryCard AddMagicCardToCollection(MyDbContext context, MagicCard magicCard, CardCollection collection, int insertionIndex = 0)
        {
            var inventoryCard = new InventoryCard
            {
                DisplayName = magicCard.DisplayName,
                uuid = magicCard.uuid,
                multiverseId_Inv = magicCard.multiverseId,
                CollectionId = collection.Id,
                InsertionIndex = insertionIndex,
                Virtual = collection.Virtual
            };
            if (magicCard.isFoilOnly)
                inventoryCard.Foil = true;
            else
                inventoryCard.Foil = false;
            if (magicCard.PartB != null)
                inventoryCard.PartB_uuid = magicCard.PartB.uuid;
            context.Library.Add(inventoryCard);
            return inventoryCard;
        }

        public static void AddMagicCardsToCollection(List<OLVCardItem> cards, CardCollection collection)
        {
            var setItems = new Dictionary<string, OLVSetItem>();
            using (MyDbContext context = new MyDbContext())
            {
                var cardsAdded = new List<InventoryCard>();
                int insertionIndex = 0;
                foreach (OLVCardItem cardItem in cards)
                {
                    var card = cardItem.MagicCard;
                    var inventoryCard = AddMagicCardToCollection(context, card, collection, insertionIndex);
                    cardsAdded.Add(inventoryCard);
                    insertionIndex++;
                    if (!setItems.TryGetValue(card.Edition, out OLVSetItem setItem))
                        if ((setItem = Globals.Forms.DBViewForm.SetItems.FirstOrDefault(x => x.Name == card.Edition)) != null)
                            setItems.Add(card.Edition, setItem);
                }
                context.SaveChanges();
                var cvForm = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.Collection.Id == collection.Id);
                var fullCardsAdded = new List<FullInventoryCard>();
                foreach (InventoryCard card in cardsAdded)
                {
                    if (cvForm != null)
                    {
                        var fullCard = card.ToFullCard(context);
                        if (fullCard != null)
                            fullCardsAdded.Add(fullCard);
                    }
                    if (!card.Virtual && Globals.Collections.AllMagicCards.TryGetValue(card.uuid, out MagicCard magicCard))
                        magicCard.CopiesOwned++;
                }
                if (cvForm != null)
                    cvForm.AddFullInventoryCards(fullCardsAdded);
                Globals.Forms.DBViewForm.setListView.RefreshObjects(setItems.Values.ToArray());
            }
        }

        public static void MoveFullInventoryCardsToCollection(ArrayList fullInventoryCards, CollectionViewForm sourceCVForm, CardCollection collection)
        {
            var cardsList = new List<FullInventoryCard>();
            try
            {
                using (var context = new MyDbContext())
                {
                    foreach (FullInventoryCard fullInventoryCard in fullInventoryCards)
                    {
                        fullInventoryCard.CollectionId = collection.Id;
                        fullInventoryCard.Virtual = collection.Virtual;
                        context.Update(fullInventoryCard.InventoryCard);
                        cardsList.Add(fullInventoryCard);
                    }
                    context.SaveChanges();
                }
                sourceCVForm.RemoveFullInventoryCards(cardsList);
                Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.Collection.Id == collection.Id)?.AddFullInventoryCards(cardsList);
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
                MessageBox.Show("Could not move cards to collection.");
            }
        }

        public static void CountInventory()
        {
            using (var context = new MyDbContext())
            {
                var inventoryCards = from c in context.Library
                                     select c;

                foreach (var magicCard in Globals.Collections.AllMagicCards.Values)
                    magicCard.CopiesOwned = 0;

                foreach (var inventoryCard in inventoryCards)
                    if (!inventoryCard.Virtual && inventoryCard.Count.HasValue && Globals.Collections.AllMagicCards.TryGetValue(inventoryCard.uuid, out MagicCard magicCard))
                        magicCard.CopiesOwned += inventoryCard.Count.Value;
            }
        }

        private static void UpdateCardsInDB(MyDbContext context, ArrayList items)
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

        private static MagicCard UpdateCopiesOwned(MyDbContext context, FullInventoryCard card)
        {
            MagicCard magicCard = null;
            var allCopiesSum = context.LibraryView.Where(x => x.uuid == card.uuid && !x.Virtual).Sum(x => x.Count);
            if (allCopiesSum.HasValue && Globals.Collections.AllMagicCards.TryGetValue(card.uuid, out magicCard))
                magicCard.CopiesOwned = allCopiesSum.Value;

            return magicCard;
        }

        public static void UpdateCards(CardsUpdatedEventArgs e)
        {
            var setItems = new Dictionary<string, OLVSetItem>();
            using (MyDbContext context = new MyDbContext())
            {
                try
                {
                    UpdateCardsInDB(context, e.Items);
                    var inventoryCardsStillSelected = e.CollectionViewForm.cardListView.SelectedObjects.Cast<object>().Where(x => x is FullInventoryCard).Cast<FullInventoryCard>().ToList();
                    var inventoryCardsToRemove = new List<FullInventoryCard>();
                    var magicCardsCopiesUpdated = new List<MagicCard>();
                    foreach (FullInventoryCard card in e.Items)
                    {
                        if (!setItems.TryGetValue(card.Edition, out OLVSetItem setItem)) // get updated set item
                            if ((setItem = Globals.Forms.DBViewForm.SetItems.FirstOrDefault(x => x.Name == card.Edition)) != null)
                                setItems.Add(card.Edition, setItem);

                        var magicCard = UpdateCopiesOwned(context, card);
                        if (magicCard != null)
                            magicCardsCopiesUpdated.Add(magicCard);
                        if (card.Count < 1) // add to remove list or still selected list
                        {
                            inventoryCardsStillSelected.Remove(card);
                            inventoryCardsToRemove.Add(card);
                        }
                    }
                    e.CollectionViewForm.cardListView.RemoveObjects(inventoryCardsToRemove);
                    e.CollectionViewForm.cardListView.SelectedObjects = inventoryCardsStillSelected; // workaround: list view will not actually update selected items when removing from SelectedObjects
                    Globals.Forms.DBViewForm.cardListView.RefreshObjects(magicCardsCopiesUpdated);
                    Globals.Forms.DBViewForm.setListView.RefreshObjects(setItems.Values.ToArray());
                }
                catch (Exception ex)
                {
                    DebugOutput.WriteLine(ex.ToString());
                    foreach (FullInventoryCard card in e.Items)
                        context.Entry(card).Reload();

                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    e.CollectionViewForm.UpdateTotals();
                    e.CollectionViewForm.cardListView.Refresh();
                }
            }
        }
    }
}