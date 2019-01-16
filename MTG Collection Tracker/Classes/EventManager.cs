using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public static class EventManager
    {
        public static void CardFocused(object sender, CardFocusedEventArgs e)
        {
            Globals.States.CardFocusedUuid = e.uuid;
        }

        public static void CardSelected(object sender, CardSelectedEventArgs e)
        {
            var card = e.MagicCard;
            Globals.Forms.CardInfoForm.CardSelected(card);
            CardFocused(sender, new CardFocusedEventArgs { uuid = card.uuid });

            using (CardImagesDbContext context = new CardImagesDbContext(card.Edition))
            {
                var imageBytes = (from i in context.CardImages
                                  where i.uuid == card.uuid
                                  select i).FirstOrDefault()?.CardImageBytes;

                if (imageBytes != null)
                {
                    var img = ImageExtensions.FromByteArray(imageBytes);
                    OnCardImageRetrieved(new CardImageRetrievedEventArgs { uuid = card.uuid, CardImage = img });
                }
                else
                {
                    string displayName;
                    if (card is FullInventoryCard fullInventoryCard)
                        displayName = fullInventoryCard.DisplayName;
                    else
                        displayName = card.DisplayName;
                    Globals.Forms.TasksForm.TaskManager.AddTask(new DownloadResourceTask { AddFirst = true, Caption = $"Card Image: {displayName}", URL = $"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={card.multiverseId}&type=card", TaskObject = new BasicCardArgs { uuid = card.uuid, MultiverseId = card.multiverseId, Edition = card.Edition }, OnTaskCompleted = ImageDownloadCompleted });
                }
            }
        }

        public static void ImageDownloadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = e.Result as CardResourceArgs;
            try
            {
                using (CardImagesDbContext context = new CardImagesDbContext(args.Edition))
                {
                    context.Add(new DbCardImage { uuid = args.uuid, CardImageBytes = args.Data });
                    context.SaveChanges();
                }
                var img = ImageExtensions.FromByteArray(args.Data);
                OnCardImageRetrieved(new CardImageRetrievedEventArgs { uuid = args.uuid, MultiverseId = args.MultiverseId, CardImage = img });
            }
            catch (Exception ex) { DebugOutput.WriteLine(ex.ToString()); }
        }

        static public event EventHandler<CardImageRetrievedEventArgs> CardImageRetrieved;

        private static void OnCardImageRetrieved(CardImageRetrievedEventArgs args)
        {
            CardImageRetrieved?.Invoke(Globals.Forms.MainForm, args);
        }

        private delegate void PricesFetchedDelegate(object sender, PricesUpdatedEventArgs e);

        public static void PricesFetched(object sender, PricesUpdatedEventArgs e)
        {
            if (Globals.Forms.MainForm.InvokeRequired)
                Globals.Forms.MainForm.BeginInvoke(new PricesFetchedDelegate(PricesFetched), sender, e);
            else
            {
                using (var context = new MyDbContext())
                {
                    MagicCard dbMatch;
                    foreach (var price in e.Prices)
                    {
                        if (price.Value.HasValue && (dbMatch = Globals.Collections.MagicCardCache.FirstOrDefault(x => x.Value.tcgplayerProductId == price.Key).Value) != null)
                        {
                            dbMatch.tcgplayerMarketPrice = price.Value.Value;
                            context.Update(dbMatch);
                        }
                    }
                    context.SaveChanges();
                }

                foreach (var form in Globals.Forms.OpenCollectionForms)
                {
                    var cardsToRefresh = new List<FullInventoryCard>();
                    foreach (var price in e.Prices)
                    {
                        if (price.Value.HasValue)
                        {
                            var matches = new List<FullInventoryCard>();
                            foreach (var row in form.cardListView.Objects)
                                if (row is FullInventoryCard card && card.tcgplayerProductId == price.Key)
                                    matches.Add(row as FullInventoryCard);
                            if (matches.Count() > 0)
                            {
                                foreach (var match in matches)
                                {
                                    match.tcgplayerMarketPrice = price.Value.Value;
                                    cardsToRefresh.Add(match);
                                }
                            }
                        }
                    }
                    if (cardsToRefresh.Count > 0)
                    {
                        form.cardListView.RefreshObjects(cardsToRefresh);
                        form.UpdateTotals();
                    }
                }
            }
        }

        private delegate void SetDownloadedDelegate(object sender, SetDownloadedEventArgs e);

        public static void SetDownloaded(object sender, SetDownloadedEventArgs e)
        {
            if (Globals.Forms.MainForm.InvokeRequired)
                Globals.Forms.MainForm.BeginInvoke(new SetDownloadedDelegate(SetDownloaded), sender, e);
            else
            {
                Globals.Forms.MainForm.AddSetIcon(e.SetCode);
                Globals.Forms.DBViewForm.LoadSet(e.SetCode);
                foreach (var form in Globals.Forms.OpenCollectionForms)
                {
                    var cardsNeedingUpdates = form.cardListView.Objects.Cast<object>().Where(x => x is FullInventoryCard card && card.SetCode == e.SetCode).Cast<FullInventoryCard>().ToList();
                    foreach (var card in cardsNeedingUpdates)
                        card.CopyFromMagicCard(Globals.Collections.MagicCardCache[card.uuid]);
                    form.cardListView.RefreshObjects(cardsNeedingUpdates);
                }
                if (Globals.Forms.TasksForm.TaskManager.TaskCount == 0)
                    Globals.Forms.DBViewForm.SortCardListView();

                CardManager.CountInventory();
            }
        }

        private static void CardsDropped(object sender, CardsDroppedEventArgs e)
        {
            if (e.Items[0] is OLVCardItem)
            {
                var cardItems = new List<OLVCardItem>();
                foreach (OLVCardItem cardItem in e.Items)
                    cardItems.Add(cardItem);
                CardManager.AddMagicCardsToCollection(cardItems, e.TargetCollection);
            }
            else if (e.Items[0] is OLVSetItem setItem)
                CardManager.AddMagicCardsToCollection(setItem.Cards, e.TargetCollection);
            else if (e.Items[0] is OLVRarityItem rarityItem)
                CardManager.AddMagicCardsToCollection(rarityItem.Cards, e.TargetCollection);
            else if (e.Items[0] is FullInventoryCard)
                CardManager.MoveFullInventoryCardsToCollection(e.Items, e.SourceForm as CollectionViewForm, e.TargetCollection);
        }

        public static void NavigationFormCardsDropped(object sender, CardsDroppedEventArgs e)
        {
            CardsDropped(sender, e);
        }

        public static void CollectionViewFormCardsDropped(object sender, CardsDroppedEventArgs e)
        {
            CardsDropped(sender, e);
        }

        public static void CollectionViewFormCardsUpdated(object sender, CardsUpdatedEventArgs e)
        {
            if (e.CollectionViewForm != null)
                CardManager.UpdateCards(e);
        }

        public static void DBViewFormCardActivated(object sender, CardsActivatedEventArgs args)
        {
            if (Globals.Forms.DockPanel.ActiveDocument != null)
                CollectionViewFormCardsDropped(sender, new CardsDroppedEventArgs { Items = args.CardItems, TargetCollection = (Globals.Forms.DockPanel.ActiveDocument as CollectionViewForm).Collection });
        }

        public static void NavigationFormCollectionActivated(object sender, CollectionActivatedEventArgs e)
        {
            if (e.NavigatorCollection?.Id is int id)
            {
                if (Globals.Forms.DockPanel.Documents.FirstOrDefault(x => (x as CollectionViewForm).Collection.Id == id) is CollectionViewForm document)
                    document.Activate();
                else
                    CardManager.LoadCollection(e.NavigatorCollection.CardCollection);
            }
        }
    }
}