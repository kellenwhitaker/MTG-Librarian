using BrightIdeasSoftware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KW.WinFormsUI.Docking;

namespace MTG_Librarian
{
    public partial class CollectionViewForm : DockContent
    {
        #region Properties

        public string DocumentName => Collection?.CollectionName;
        public CardCollection Collection { get; set; }

        public override string Text
        {
            get => Collection?.CollectionName;
            set
            {
                if (Collection != null)
                    Collection.CollectionName = value;
            }
        }

        #endregion Properties

        #region Fields

        private readonly System.Timers.Timer TextChangedWaitTimer = new System.Timers.Timer();

        #endregion Fields

        #region Constructors

        public CollectionViewForm()
        {
            InitializeComponent();
            cardListView.SetDoubleBuffered();
            cardListView.AllColumns.FirstOrDefault(x => x.AspectName == "PaddedName").Renderer = new CardInstanceNameRenderer();
            cardListView.AllColumns.FirstOrDefault(x => x.AspectName == "ManaCost").Renderer = new ManaCostRenderer();
            cardListView.VirtualListDataSource = new MyCustomSortingDataSource(cardListView);
            cardListView.AddDecoration(new EditingCellBorderDecoration { UseLightbox = false, BorderPen = new Pen(Brushes.DodgerBlue, 3), BoundsPadding = new Size(1, 0) });
            cardListView.UseFiltering = true;
            var dropSink = cardListView.DropSink as SimpleDropSink;
            dropSink.CanDropOnItem = false;
            dropSink.Billboard.BackColor = Color.DodgerBlue;
            dropSink.Billboard.TextColor = Color.White;
            DockAreas = DockAreas.Document | DockAreas.DockBottom;
            whiteManaButton.ImageList = blueManaButton.ImageList = blackManaButton.ImageList = redManaButton.ImageList = greenManaButton.ImageList
                                      = colorlessManaButton.ImageList = genericManaButton.ImageList = Globals.ImageLists.ManaIcons;
            (whiteManaButton.ImageKey, blueManaButton.ImageKey) = ("{W}", "{U}");
            (blackManaButton.ImageKey, redManaButton.ImageKey, greenManaButton.ImageKey) = ("{B}", "{R}", "{G}");
            (colorlessManaButton.ImageKey, genericManaButton.ImageKey) = ("{C}", "{X}");
            Globals.Forms.OpenCollectionForms.Add(this);
            TextChangedWaitTimer.Interval = 400;
            TextChangedWaitTimer.Elapsed += (sender, e) =>
            {
                TextChangedWaitTimer.Stop();
                UpdateModelFilter();
            };
        }

        #endregion Constructors

        #region Mehthods

        public void UpdateTotals()
        {
            var totalsRow = (cardListView.Objects as ArrayList)[0] as InventoryTotalsItem;
            totalsRow.tcgplayerMarketPrice = 0;
            totalsRow.Cost = 0;
            totalsRow.Count = 0;
            foreach (var row in cardListView.FilteredObjects)
            {
                if (row is FullInventoryCard card)
                {
                    int cardCount = card.Count.HasValue ? card.Count.Value : 1;
                    totalsRow.Count += cardCount;
                    if (card.tcgplayerMarketPrice.HasValue)
                        totalsRow.tcgplayerMarketPrice += card.tcgplayerMarketPrice.Value * cardCount;
                    if (card.Cost.HasValue)
                        totalsRow.Cost += card.Cost.Value * cardCount;
                }
            }
        }

        #region Filters

        private delegate void UpdateModelFilterDelegate();

        private void UpdateModelFilter()
        {
            if (InvokeRequired)
                BeginInvoke(new UpdateModelFilterDelegate(UpdateModelFilter));
            else
            {
                var selectedObjects = new List<object>();
                foreach (object o in cardListView.SelectedObjects)
                    selectedObjects.Add(o);

                cardListView.ModelFilter = new ModelFilter(GetCardFilter());
                cardListView.SelectedObjects = selectedObjects;
                cardListView.RefreshSelectedObjects();
                UpdateTotals();
            }
        }

        private Predicate<object> GetCardFilter()
        {
            return new Predicate<object>(x => x is InventoryTotalsItem)
                .Or(GetManaCostFilter()
                .And(GetSetFilter())
                .And(GetCardNameFilter())
                .And(GetRarityFilter()));
        }

        private Predicate<object> GetRarityFilter()
        {
            string rarityFilterText = rarityFilterComboBox.Text.ToUpper();
            return x => rarityFilterText == "ALL RARITIES" || rarityFilterText == "" ? true : (x as FullInventoryCard).rarity.ToUpper() == rarityFilterText;
        }

        private Predicate<object> GetCardNameFilter()
        {
            return x => cardNameFilterTextBox.UserText == "" ? true : (x as FullInventoryCard).name.ToUpper().Contains(cardNameFilterTextBox.UserText.ToUpper());
        }

        private Predicate<object> GetManaCostFilter()
        {
            Predicate<object> combinedFilter = x => true;

            if (whiteManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).manaCost?.Contains("W") ?? false);
            if (blueManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).manaCost?.Contains("U") ?? false);
            if (blackManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).manaCost?.Contains("B") ?? false);
            if (redManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).manaCost?.Contains("R") ?? false);
            if (greenManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).manaCost?.Contains("G") ?? false);
            if (colorlessManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).manaCost?.Contains("C") ?? false);
            if (genericManaButton.Checked) combinedFilter = combinedFilter.And(x => ((x as FullInventoryCard).manaCost?.Contains("X") ?? false) || ((x as FullInventoryCard).manaCost?.Any(c => char.IsDigit(c)) ?? false));
            return combinedFilter;
        }

        private Predicate<object> GetSetFilter()
        {
            return x => setFilterTextBox.UserText == "" ? true : (x as FullInventoryCard).Edition.ToUpper().Contains(setFilterTextBox.UserText.ToUpper());
        }

        #endregion Filters

        public void LoadCollection()
        {
            if (Collection != null)
            {
                using (MyDbContext context = new MyDbContext())
                {
                    var items = from c in context.LibraryView
                                where c.CollectionId == Collection.Id
                                select c;

                    cardListView.AddObject(new InventoryTotalsItem { DisplayName = "        Totals:" });
                    cardListView.AddObjects(items.ToList());
                }
                if (cardListView.PrimarySortColumn == null) // not yet sorted
                    cardListView.Sort(cardListView.AllColumns.FirstOrDefault(x => x.AspectName == "TimeAdded"), SortOrder.Ascending);

                UpdateTotals();
            }
        }

        public void AddFullInventoryCard(FullInventoryCard cardInstance)
        {
            cardListView.AddObject(cardInstance);
            UpdateTotals();
        }

        public void AddFullInventoryCards(List<FullInventoryCard> cards)
        {
            cardListView.AddObjects(cards);
            UpdateTotals();
        }

        public void RemoveFullInventoryCards(List<FullInventoryCard> cards)
        {
            cardListView.RemoveObjects(cards);
        }

        private DialogResult ConfirmCardDeletion(string message = "The highlighted card(s) will be deleted. Are you sure you wish to continue?")
        {
            return MessageBox.Show(message, "Pending delete", MessageBoxButtons.YesNo);
        }

        #endregion Mehthods

        #region Events

        #region OLV Events

        private void fastObjectListView1_ModelCanDrop(object sender, ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            if (e.SourceModels[0] is OLVCardItem)
                e.InfoMessage = $"Add {e.SourceModels.Count} card{(e.SourceModels.Count == 1 ? "" : "s")} to {DocumentName}";
            else if (e.SourceModels[0] is OLVSetItem setItem)
                e.InfoMessage = $"Add set [{setItem.Name}] to {DocumentName}";
            else if (e.SourceModels[0] is OLVRarityItem rarityItem)
                e.InfoMessage = $"Add {rarityItem.Rarity}s from [{(rarityItem.Parent as OLVSetItem).Name}] to {DocumentName}";
            else if (e.SourceModels[0] is FullInventoryCard fullInventoryCard && e.SourceListView != this.cardListView)
                e.InfoMessage = $"Add {e.SourceModels.Count} card{(e.SourceModels.Count == 1 ? "" : "s")} to {DocumentName}";
            else
                e.Effect = DragDropEffects.None;
        }

        private void fastObjectListView1_ModelDropped(object sender, ModelDropEventArgs e)
        {
            OnCardsDropped(new CardsDroppedEventArgs
            {
                Items = e.SourceModels as ArrayList,
                TargetCollection = Collection,
                SourceForm = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.cardListView == e.SourceListView)
            });
        }

        private void fastObjectListView1_CellEditFinished(object sender, CellEditEventArgs e)
        {
            if (e.RowObject is FullInventoryCard editedCard)
            {
                var args = new CardsUpdatedEventArgs { Items = new ArrayList { editedCard }, CollectionViewForm = this };
                var rowItem = cardListView.ModelToItem(editedCard);
                if (e.Column.AspectName == "Count" && editedCard.Count < 1)
                {
                    if (ConfirmCardDeletion() != DialogResult.Yes)
                    {
                        if (Int32.TryParse(e.Value.ToString(), out int oldCount))
                            editedCard.Count = oldCount;
                        else
                            editedCard.Count = 1;
                        return;
                    }
                }

                if (cardListView.SelectedObjects != null)
                    foreach (FullInventoryCard card in cardListView.SelectedObjects)
                    {
                        if (card != editedCard)
                        {
                            if (e.Column.AspectName == "Tags")
                                card.Tags = editedCard.Tags;
                            else if (e.Column.AspectName == "Count")
                                card.Count = editedCard.Count;
                            else if (e.Column.AspectName == "Cost")
                                card.Cost = editedCard.Cost;
                            args.Items.Add(card);
                        }
                    }
                OnCardsUpdated(args);
            }
        }

        private void fastObjectListView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cardListView.SelectedObjects?.Count > 0)
            {
                if (e.KeyChar == '=' || e.KeyChar == '+')
                {
                    e.Handled = true;
                    foreach (FullInventoryCard item in cardListView.SelectedObjects)
                        item.Count++;
                    OnCardsUpdated(new CardsUpdatedEventArgs { Items = cardListView.SelectedObjects as ArrayList, CollectionViewForm = this });
                }
                else if (e.KeyChar == '-' || e.KeyChar == '_')
                {
                    e.Handled = true;
                    bool pendingDeletion = false;
                    foreach (FullInventoryCard item in cardListView.SelectedObjects)
                    {
                        item.Count--;
                        if (item.Count < 1)
                            pendingDeletion = true;
                    }
                    if (pendingDeletion)
                    {
                        if (ConfirmCardDeletion("Some of the highlighted cards will be deleted. Are you sure you wish to continue?") != DialogResult.Yes)
                        {
                            foreach (FullInventoryCard item in cardListView.SelectedObjects)
                                item.Count++;
                            return;
                        }
                    }
                    OnCardsUpdated(new CardsUpdatedEventArgs { Items = cardListView.SelectedObjects as ArrayList, CollectionViewForm = this });
                }
                else if (e.KeyChar == '\r')
                {
                    e.Handled = true;
                    if (cardListView.SelectedObjects.Count > 0)
                    {
                        var cardsToPrice = new List<FullInventoryCard>();
                        foreach (var row in cardListView.SelectedObjects)
                            if (row is FullInventoryCard card && card.tcgplayerProductId != 0)
                                cardsToPrice.Add(row as FullInventoryCard);
                        if (cardsToPrice.Count > 0)
                            Globals.Forms.TasksForm.TaskManager.AddTask(new GetTCGPlayerPricesTask(cardsToPrice) { AddFirst = true });
                        else
                            MessageBox.Show("No valid product IDs were found. Please try updating the set(s)");
                    }
                }
            }
        }

        private void fastObjectListView1_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ClickCount == 2 && e.Column.IsEditable && e.Model is FullInventoryCard)
            {
                e.ListView.StartCellEdit(e.Item, e.Item.SubItems.IndexOf(e.SubItem));
                e.Handled = true;
            }
        }

        private void fastObjectListView1_SelectionChanged(object sender, EventArgs e)
        {
            if (cardListView.SelectedObject is MagicCardBase card)
                OnCardSelected(new CardSelectedEventArgs { MagicCard = card });
        }

        private void cardListView_SubItemChecking(object sender, SubItemCheckingEventArgs e)
        {
            if (e.RowObject is FullInventoryCard card)
            {
                if (card.isFoilOnly || !card.hasFoil)
                    e.Canceled = true;
                else
                {
                    card.Foil = e.NewValue == CheckState.Checked ? true : false;
                    var cellEditArgs = new CellEditEventArgs(e.Column, null, new Rectangle(), e.ListViewItem, e.SubItemIndex);
                    fastObjectListView1_CellEditFinished(sender, cellEditArgs);
                }
            }
        }

        private void cardListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (cardListView.SelectedObjects?.Count > 0)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (ConfirmCardDeletion() == DialogResult.Yes)
                    {
                        foreach (FullInventoryCard cardItem in cardListView.SelectedObjects)
                            cardItem.Count = 0;
                        OnCardsUpdated(new CardsUpdatedEventArgs { Items = cardListView.SelectedObjects as ArrayList, CollectionViewForm = this });
                    }
                }
            }
        }

        private void cardListView_CellEditStarting(object sender, CellEditEventArgs e)
        {
            if (e.RowObject is FullInventoryCard card)
            {
                int costIndex = cardListView.AllColumns.FirstOrDefault(x => x.AspectName == "Cost").DisplayIndex;
                if (e.SubItemIndex == costIndex)
                {
                    var editor = new NumericUpDown
                    {
                        Bounds = e.CellBounds,
                        DecimalPlaces = 2,
                        Value = decimal.TryParse(e.Value?.ToString(), out decimal cellValue) ? cellValue : 0.0M
                    };
                    e.Control = editor;
                }
            }
        }

        private void cardListView_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.RowObject is FullInventoryCard card)
            {
                int costIndex = cardListView.AllColumns.FirstOrDefault(x => x.AspectName == "Cost").DisplayIndex;
                if (e.SubItemIndex == costIndex)
                {
                    if (double.TryParse(e.NewValue?.ToString(), out double cellValue))
                        card.Cost = cellValue;
                    cardListView.RefreshObject(card);
                    e.Cancel = true;
                }
            }
        }

        #endregion OLV Events

        #region CollectionViewForm Events

        public event EventHandler<CardsDroppedEventArgs> CardsDropped;

        private void OnCardsDropped(CardsDroppedEventArgs args)
        {
            CardsDropped?.Invoke(this, args);
        }

        public event EventHandler<CardsUpdatedEventArgs> CardsUpdated;

        private void OnCardsUpdated(CardsUpdatedEventArgs args)
        {
            CardsUpdated?.Invoke(this, args);
        }

        public event EventHandler<CardSelectedEventArgs> CardSelected;

        private void OnCardSelected(CardSelectedEventArgs args)
        {
            CardSelected?.Invoke(this, args);
        }

        #endregion CollectionViewForm Events

        #region Menu Events

        private void deleteCardsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cardListView.SelectedObjects?.Count > 0)
            {
                if (ConfirmCardDeletion() == DialogResult.Yes)
                {
                    foreach (FullInventoryCard cardItem in cardListView.SelectedObjects)
                        cardItem.Count = 0;
                    OnCardsUpdated(new CardsUpdatedEventArgs { Items = cardListView.SelectedObjects as ArrayList, CollectionViewForm = this });
                }
            }
        }

        private void cardListViewMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (cardListView.SelectedObjects?.Count < 1)
                e.Cancel = true;
        }

        #endregion Menu Events

        #region Misc Events

        private void CollectionViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Globals.Forms.OpenCollectionForms.Remove(this);
        }

        private void cardNameFilterTextBox_TextChanged(object sender, EventArgs e)
        {
            TextChangedWaitTimer.Stop();
            TextChangedWaitTimer.Start();
        }

        private void whiteManaButton_Click(object sender, EventArgs e)
        {
            UpdateModelFilter();
        }

        private void rarityFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateModelFilter();
        }

        #endregion Misc Events

        #endregion Events

        #region Classes

        public class MyCustomSortingDataSource : FastObjectListDataSource
        {
            public MyCustomSortingDataSource(FastObjectListView listView) : base(listView)
            {
            }

            override public void Sort(OLVColumn column, SortOrder order)
            {
                var comparer = new FullInventoryCardComparer { SortOrder = order, AspectName = column.AspectName };
                FilteredObjectList.Sort(comparer);
                RebuildIndexMap();
            }

            private class FullInventoryCardComparer : IComparer
            {
                public SortOrder SortOrder;
                public string AspectName;

                private static int CompareNullableDoubles(double? x, double? y)
                {
                    if (!x.HasValue && y.HasValue)
                        return -1;
                    else if (x.HasValue && !y.HasValue)
                        return 1;
                    else if (!x.HasValue && !y.HasValue)
                        return 0;
                    else
                        return x.Value.CompareTo(y.Value);
                }

                public int Compare(object x, object y)
                {
                    if (x is InventoryTotalsItem)
                        return -1;
                    else if (y is InventoryTotalsItem)
                        return 1;
                    else
                    {
                        int result = 0;
                        if (AspectName == "number")
                            result = (x as FullInventoryCard).SortableNumber.CompareTo((y as FullInventoryCard).SortableNumber);
                        else if (AspectName == "PaddedName")
                            result = (x as FullInventoryCard).name.CompareTo((y as FullInventoryCard).name);
                        else if (AspectName == "type")
                            result = (x as FullInventoryCard).type.CompareTo((y as FullInventoryCard).type);
                        else if (AspectName == "Edition")
                            result = (x as FullInventoryCard).Edition.CompareTo((y as FullInventoryCard).Edition);
                        else if (AspectName == "ManaCost")
                        {
                            string valueX = (x as FullInventoryCard).manaCost ?? "";
                            string valueY = (y as FullInventoryCard).manaCost ?? "";
                            result = valueX.CompareTo(valueY);
                        }
                        else if (AspectName == "TimeAdded")
                            result = (x as FullInventoryCard).SortableTimeAdded.CompareTo((y as FullInventoryCard).SortableTimeAdded);
                        else if (AspectName == "Foil")
                            result = (x as FullInventoryCard).Foil.CompareTo((y as FullInventoryCard).Foil);
                        else if (AspectName == "Cost")
                            result = CompareNullableDoubles((x as FullInventoryCard).Cost, (y as FullInventoryCard).Cost);
                        else if (AspectName == "tcgplayerMarketPrice")
                            result = CompareNullableDoubles((x as FullInventoryCard).tcgplayerMarketPrice, (y as FullInventoryCard).tcgplayerMarketPrice);
                        else if (AspectName == "Count")
                        {
                            var cx = (x as FullInventoryCard).Count;
                            var cy = (y as FullInventoryCard).Count;
                            if (!cx.HasValue && cy.HasValue)
                                result = -1;
                            else if (cx.HasValue && !cy.HasValue)
                                result = 1;
                            else if (!cx.HasValue && !cy.HasValue)
                                result = 0;
                            else
                                result = cx.Value.CompareTo(cy.Value);
                        }
                        else if (AspectName == "Tags")
                        {
                            var tx = (x as FullInventoryCard).Tags;
                            var ty = (y as FullInventoryCard).Tags;
                            if (tx == null && ty != null)
                                result = -1;
                            else if (tx != null && ty == null)
                                result = 1;
                            else if (tx == null && ty == null)
                                result = 0;
                            else
                                result = tx.CompareTo(ty);
                        }

                        return SortOrder == SortOrder.Ascending ? result : -1 * result;
                    }
                }
            }
        }

        #endregion Classes
    }
}