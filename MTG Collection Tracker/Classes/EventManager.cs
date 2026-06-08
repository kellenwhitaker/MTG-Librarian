using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public static class EventManager
    {
        public static void CardFocused(object sender, CardFocusedEventArgs e)
        {
            Globals.States.CardFocusedUuid = e.ScryfallId;
        }

        public static void CardSelected(object sender, CardSelectedEventArgs e)
        {
            if (e.MagicCards.Count == 1)
            {
                ScryfallMagicCardBase card = null;
                if (e.MagicCards[0] is FullInventoryCard)
                    card = e.MagicCards[0] as ScryfallMagicCardBase;
                else if (e.MagicCards[0] is OLVCardItem cardItem)
                    card = cardItem.MagicCard;
                Globals.Forms.CardInfoForm.CardSelected(card);
                CardFocused(sender, new CardFocusedEventArgs { ScryfallId = card.ScryfallId });
                CardManager.RetrieveImage(card, "A");
                if (card is FullInventoryCard)
                    Globals.Forms.MainForm.UpdateStatusBarTotals(e.MagicCards);
            }
            else if (e.MagicCards.Count > 1)
            {
                if (e.MagicCards[0] is FullInventoryCard)
                {
                    Globals.Forms.MainForm.UpdateStatusBarTotals(e.MagicCards);
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
                    context.Add(new DbCardImage { ScryfallId = args.uuid, Side = args.Side, CardImageBytes = args.Data });
                    context.SaveChanges();
                }
                var img = ImageExtensions.FromByteArray(args.Data);
                OnCardImageRetrieved(new CardImageRetrievedEventArgs { uuid = args.uuid, MultiverseId = args.MultiverseId, CardImage = img });
            }
            catch (Exception ex) { DebugOutput.WriteLine(ex.ToString()); }
        }

        public static event EventHandler<CardImageRetrievedEventArgs> CardImageRetrieved;

        public static void OnCardImageRetrieved(CardImageRetrievedEventArgs args)
        {
            CardImageRetrieved?.Invoke(Globals.Forms.MainForm, args);
        }

        public static event EventHandler DefaultCurrencyChanged;

        public static void OnDefaultCurrencyChanged()
        {
            DefaultCurrencyChanged?.Invoke(Globals.Forms.MainForm, null);
        }

        public static event EventHandler<InventoryChangedEventArgs> InventoryChanged;

        public static void OnInventoryChanged(InventoryChangedEventArgs args)
        {
            InventoryChanged?.Invoke(Globals.Forms.MainForm, args);
        }

        private delegate void ScryfallSearchEndedDelegate(object sender,  ScryfallSearchEndedEventArgs e);

        public static void ScryfallSearchEnded(object sender, ScryfallSearchEndedEventArgs e)
        {
            if (Globals.Forms.MainForm.InvokeRequired)
                Globals.Forms.MainForm.BeginInvoke(new ScryfallSearchEndedDelegate(ScryfallSearchEnded), sender, e);
            else
            {
                if (e.Results.Count > 0)
                {
                    var DefaultCurrency = SettingsManager.ApplicationSettings.DefaultCurrency;
                    var cardItems = new List<OLVCardItem>();
                    using (var context = new ScryfallCardsDbContext())
                    {
                        foreach (var card in e.Results)
                        {
                            var inventory = from c in context.Library
                                            where c.ScryfallId == card.ScryfallId
                                            select c;
                            foreach (var item in inventory)
                            {
                                if (!item.Virtual)
                                    card.CopiesOwned += item.Count.GetValueOrDefault();
                            }
                            string priceString;
                            string finish = "nonfoil";
                            if (card.finishes.Count() == 1)
                                finish = card.finishes[0];
                            if (card.prices.TryGetValue($"{DefaultCurrency.ToLower()}{((finish != "nonfoil") ? $"_{finish}" : "")}", out priceString))
                                card.Price = Convert.ToDouble(priceString);
                            cardItems.Add(new OLVCardItem(card));
                        }
                    }
                    Globals.Forms.DBViewForm.addingToCLV = true;
                    Globals.Forms.DBViewForm.cardListView.AddObjects(cardItems);
                    Globals.Forms.DBViewForm.addingToCLV = false;
                    Globals.Forms.DBViewForm.Text = $"Catalog | Query returned with {Globals.Forms.DBViewForm.cardListView.Objects.Count()} / {e.TotalCards} results: {e.Query}".Replace("&", "&&").Replace("%3A", "=");
                    Globals.Forms.DBViewForm.SearchHasMoreResults = e.Waiting;
                }
                else
                {

                    Globals.Forms.DBViewForm.Text = $"Catalog | Query returned no results: {e.Query}".Replace("&", "&&").Replace("%3A", "=");
                    Globals.Forms.DBViewForm.cardListView.EmptyListMsg = "Query returned no results.";
                }
            }
        }

        private delegate void CardsUpdatedFromScryfallDelegate(object sender, CardsUpdatedFromScryfallEventArgs e);
        public static void CardsUpdatedFromScryfall(object sender, CardsUpdatedFromScryfallEventArgs e)
        {
            var DefaultCurrency = SettingsManager.ApplicationSettings.DefaultCurrency;
            if (Globals.Forms.MainForm.InvokeRequired)
                Globals.Forms.MainForm.BeginInvoke(new CardsUpdatedFromScryfallDelegate(CardsUpdatedFromScryfall), sender, e);
            else
            {
                foreach (var form in Globals.Forms.OpenCollectionForms)
                {
                    var cardsToRefresh = new List<FullInventoryCard>();
                    foreach (var scryfallCard in e.Cards)
                    {
                        var matches = new List<FullInventoryCard>();
                        foreach (var row in form.cardListView.Objects)
                            if (row is FullInventoryCard card && card.ScryfallId == scryfallCard.ScryfallId)
                                matches.Add(row as FullInventoryCard);
                        if (matches.Count() > 0)
                        {
                            foreach (var match in matches)
                            {
                                match.CopyFromMagicCard(scryfallCard);
                                string priceString;
                                if (match.prices.TryGetValue($"{DefaultCurrency.ToLower()}{(match.Foil ? "_foil" : "")}", out priceString))
                                    match.Price = Convert.ToDouble(priceString);
                                cardsToRefresh.Add(match);
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
                var cardsNeedingRefresh = new List<OLVCardItem>();
                foreach (var obj in Globals.Forms.DBViewForm.cardListView.Objects)
                    if ((obj as OLVCardItem).MagicCard.set == e.SetCode)
                        cardsNeedingRefresh.Add(obj as OLVCardItem);

                if (cardsNeedingRefresh.Count > 0)
                    Globals.Forms.DBViewForm.cardListView.RefreshObjects(cardsNeedingRefresh);

                foreach (var form in Globals.Forms.OpenCollectionForms)
                {
                    var matches = new List<FullInventoryCard>();
                    foreach (var row in form.cardListView.Objects)
                        if (row is FullInventoryCard card && card.set == e.SetCode)
                            matches.Add(row as FullInventoryCard);
                    if (matches.Count > 0)
                        form.cardListView.RefreshObjects(matches);
                }
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
            //else if (e.Items[0] is OLVSetItem setItem) // TODO: must be redone
                //CardManager.AddMagicCardsToCollection(setItem.Cards, e.TargetCollection);
            //else if (e.Items[0] is OLVRarityItem rarityItem)
                //CardManager.AddMagicCardsToCollection(rarityItem.Cards, e.TargetCollection);
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