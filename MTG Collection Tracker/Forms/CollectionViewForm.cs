using BrightIdeasSoftware;
using KW.WinFormsUI.Docking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
// TODO: when dragging over a cardListView, system drawn cells are drawn with a white background
namespace MTG_Librarian
{
    public partial class CollectionViewForm : DockContent
    {
        private string DefaultCurrency = SettingsManager.ApplicationSettings.DefaultCurrency;
        private bool ignoreNextCardListViewSelectionChanged = false;
        private bool ignoreNextSideboardListViewSelectionChanged = false;
        #region Properties

        public string DocumentName => Collection?.CollectionName;
        public CardCollection Collection { get; set; }

        public override string Text
        {
            get => Collection?.CollectionName + " - " + Collection?.Platform;
            set
            {
                if (Collection != null)
                    Collection.CollectionName = value;
            }
        }

        #endregion Properties

        #region Fields

        private readonly System.Timers.Timer TextChangedWaitTimer = new System.Timers.Timer();
        private ComboBox EditorComboBox;

        #endregion Fields

        #region Constructors

        public CollectionViewForm()
        {
            InitializeComponent();
            cardListView.SetDoubleBuffered();
            
            cardListView.FormatRow += (sender, e) =>
            {
                if (e.Model is InventoryTotalsItem)
                {
                    e.Item.BackColor = Color.DodgerBlue;
                    e.Item.ForeColor = Color.White;
                }
            };

            sideboardListView.FormatRow += (sender, e) =>
            {
                if (e.Model is InventoryTotalsItem)
                {
                    e.Item.BackColor = Color.DodgerBlue;
                    e.Item.ForeColor = Color.White;
                }
            };

            cardListView.AllColumns.FirstOrDefault(x => x.AspectName == "PaddedName").Renderer = new CardInstanceNameRenderer();
            cardListView.AllColumns.FirstOrDefault(x => x.AspectName == "ManaCost").Renderer = new ManaCostRenderer();
            cardListView.AllColumns.FirstOrDefault(x => x.AspectName == "Delta").Renderer = new DeltaRenderer();
            cardListView.AllColumns.FirstOrDefault(y => y.AspectName == "Percent").Renderer = new DeltaRenderer();
            sideboardListView.AllColumns.FirstOrDefault(x => x.AspectName == "PaddedName").Renderer = new CardInstanceNameRenderer();
            sideboardListView.AllColumns.FirstOrDefault(x => x.AspectName == "ManaCost").Renderer = new ManaCostRenderer();
            sideboardListView.AllColumns.FirstOrDefault(x => x.AspectName == "Delta").Renderer = new DeltaRenderer();
            sideboardListView.AllColumns.FirstOrDefault(y => y.AspectName == "Percent").Renderer = new DeltaRenderer();
            sideboardListView.SmallImageList = Globals.ImageLists.SmallIconList;
            cardListView.VirtualListDataSource = new MyCustomSortingDataSource(cardListView);
            sideboardListView.VirtualListDataSource = new MyCustomSortingDataSource(sideboardListView);
            var decoration = new EditingCellBorderDecoration { UseLightbox = false, BorderPen = new Pen(Brushes.DodgerBlue, 3), BoundsPadding = new Size(1, 0) };
            cardListView.AddDecoration(decoration);
            sideboardListView.AddDecoration(decoration);
            cardListView.UseFiltering = true;
            sideboardListView.UseFiltering = true;
            var dropSink = cardListView.DropSink as SimpleDropSink;
            dropSink.CanDropOnItem = false;
            dropSink.Billboard.BackColor = Color.DodgerBlue;
            dropSink.Billboard.TextColor = Color.White;

            dropSink = sideboardListView.DropSink as SimpleDropSink;
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

        #region Methods

        #region Filters

        private delegate void UpdateModelFilterDelegate();
        private void UpdateModelFilter()
        {
            if (InvokeRequired)
                BeginInvoke(new UpdateModelFilterDelegate(UpdateModelFilter));
            else
            {
                var filter = GetCardFilter();
                var selectedObjects = new List<object>();
                foreach (object o in cardListView.SelectedObjects)
                    selectedObjects.Add(o);

                cardListView.ModelFilter = new ModelFilter(filter);
                cardListView.SelectedObjects = selectedObjects;
                cardListView.RefreshSelectedObjects();

                selectedObjects = new List<object>();
                foreach (object o in sideboardListView.SelectedObjects)
                    selectedObjects.Add(o);

                sideboardListView.ModelFilter = new ModelFilter(filter);
                sideboardListView.SelectedObjects = selectedObjects;
                sideboardListView.RefreshSelectedObjects();

                UpdateTotals();
            }
        }

        private Predicate<object> GetCardFilter()
        {
            return new Predicate<object>(x => x is InventoryTotalsItem)
              .Or(
                  GetManaCostFilter()
                  .And(GetCommentsFilter())
                  .And(GetCardNameFilter())
                  .And(GetCardTextFilter())
                  .And(GetRarityFilter())
                  .And(GetSetFilter())
                  .And(GetTypeFilter())
              );
        }

        private Predicate<object> GetCommentsFilter()
        {
            return x => commentsFilterTextBox.Text == ""
                ? true
                : (x as FullInventoryCard).Tags?.ToUpper().Contains(commentsFilterTextBox.Text.ToUpper()) ?? false;
        }

        private Predicate<object> GetCardNameFilter()
        {
            return x => cardNameFilterTextBox.Text == ""
                ? true
                : (x as FullInventoryCard).Name?.ToUpper().Contains(cardNameFilterTextBox.Text.ToUpper()) ?? false;
        }

        private Predicate<object> GetCardTextFilter()
        {
            return x => cardTextFilterTextBox.Text == ""
                ? true
                : (x as FullInventoryCard).text?.ToUpper().Contains(cardTextFilterTextBox.Text.ToUpper()) ?? false;
        }

        private Predicate<object> GetManaCostFilter()
        {
            Predicate<object> combinedFilter = x => true;

            if (whiteManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).mana_cost?.Contains("W") ?? false);
            if (blueManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).mana_cost?.Contains("U") ?? false);
            if (blackManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).mana_cost?.Contains("B") ?? false);
            if (redManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).mana_cost?.Contains("R") ?? false);
            if (greenManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).mana_cost?.Contains("G") ?? false);
            if (colorlessManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as FullInventoryCard).mana_cost?.Contains("C") ?? false);
            if (genericManaButton.Checked) combinedFilter = combinedFilter.And(x => ((x as FullInventoryCard).mana_cost?.Contains("X") ?? false)
                || ((x as FullInventoryCard).mana_cost?.Any(c => char.IsDigit(c)) ?? false));
            return combinedFilter;
        }

        private Predicate<object> GetRarityFilter()
        {
            string rarityFilterText = rarityFilterComboBox.Text.ToUpper();
            return x => rarityFilterText == "ALL RARITIES" || rarityFilterText == ""
                ? true
                : (x as FullInventoryCard).rarity?.ToUpper() == rarityFilterText;
        }

        private Predicate<object> GetTypeFilter()
        {
            return x => typeFilterTextBox.Text == ""
                ? true
                : (x as FullInventoryCard).type_line?.ToUpper().Contains(typeFilterTextBox.Text.ToUpper()) ?? false;
        }

        private Predicate<object> GetSetFilter()
        {
            return x => setFilterTextBox.Text == ""
                ? true
                : (x as FullInventoryCard).set_name?.ToUpper().Contains(setFilterTextBox.Text.ToUpper()) ?? false;
        }

        #endregion Filters
        
        private void UpdateTotals(FastObjectListView listView)
        {
            var totalsRow = (listView.Objects as ArrayList)[0] as InventoryTotalsItem;
            totalsRow.Price = 0;
            totalsRow.Cost = 0;
            totalsRow.Count = 0;
            foreach (var row in listView.FilteredObjects)
            {
                if (row is FullInventoryCard card)
                {
                    int cardCount = card.Count.HasValue ? card.Count.Value : 1;
                    totalsRow.Count += cardCount;
                    if (card.Price.HasValue)
                        totalsRow.Price += card.Price.Value * cardCount;
                    if (card.Cost.HasValue)
                        totalsRow.Cost += card.Cost.Value * cardCount;
                }
            }
            listView.RefreshObject(totalsRow);
        }
        public void UpdateTotals()
        {            
            UpdateTotals(cardListView);

            if (Collection.Type == "deck")
                UpdateTotals(sideboardListView);
       }         

        public void LoadCollection()
        {
            if (Collection != null)
            {

                if (Collection.Type == "collection")
                {
                    tabControl.Appearance = TabAppearance.FlatButtons;
                    tabControl.Alignment = TabAlignment.Top;
                    tabControl.ItemSize = new Size(0, 1);
                    tabControl.SizeMode = TabSizeMode.Fixed;
                    sideboardListView.Visible = false;
                    cardListView.Dock = DockStyle.Fill;
                }
                else if (Collection.Type == "deck")
                {
                    CountColumn.DisplayIndex = sideboardCountColumn.DisplayIndex = 1;
                    tcgplayerMarketPriceColumn.DisplayIndex = sideboardPriceColumn.DisplayIndex = 2;
                    ManaCost.DisplayIndex = sideboardManaCostColumn.DisplayIndex =  3;
                    cardListView.ShowGroups = sideboardListView.ShowGroups = true;
                    cardListView.AlwaysGroupByColumn = CardName;
                    sideboardListView.AlwaysGroupByColumn = sideboardCardNameColumn;
                    CardName.GroupKeyGetter = sideboardCardNameColumn.GroupKeyGetter = delegate (object rowObject) {
                        if (rowObject is FullInventoryCard card)
                        {
                            return card.ListViewGroupKey;
                        }
                        else if (rowObject is InventoryTotalsItem)
                            return "";
                        return null;
                    };

                    CardName.GroupKeyToTitleConverter = delegate (object groupKey) {
                        if (groupKey.ToString() == "")
                            return "Mainboard";
                        else
                        {              
                            var count = 0;
                            foreach (var item in cardListView.Objects)
                            {
                                if (item is FullInventoryCard card && card.ListViewGroupKey == groupKey.ToString())
                                    count += card.Count.Value;
                            }
                            return $"{groupKey.ToString()} ({count})";
                        }
                    };

                    sideboardCardNameColumn.GroupKeyToTitleConverter = delegate (object groupKey) {
                        if (groupKey.ToString() == "")
                            return "Sideboard";
                        else
                        {
                            var count = 0;
                            foreach (var item in sideboardListView.Objects)
                            {
                                if (item is FullInventoryCard card && card.ListViewGroupKey == groupKey.ToString())
                                    count += card.Count.Value;
                            }
                            return $"{groupKey.ToString()} ({count})";
                        }
                    };
                }

                var DefaultPaperCurrency = SettingsManager.ApplicationSettings.DefaultPaperCurrency;
                using (ScryfallCardsDbContext context = new ScryfallCardsDbContext())
                {
                    var items = from c in context.LibraryView
                                where c.CollectionId == Collection.Id
                                select c;

                    foreach (var fullCard in items)
                    {
                        string priceString = "";
                        string finish = fullCard.Finish;
                        string key = "";
                        if (fullCard.Platform == "MTGO")
                            key = "tix";
                        else if (fullCard.Platform == "Paper")
                            key = $"{DefaultPaperCurrency.ToLower()}{(finish != "nonfoil" ? $"_{finish}" : "")}";
                        if (fullCard.prices != null && fullCard.prices.TryGetValue(key, out priceString) && !string.IsNullOrEmpty(priceString))
                        {
                            fullCard.Price = Convert.ToDouble(priceString);
                        }
                    }
                    cardListView.AddObject(new InventoryTotalsItem { DisplayName = "        Totals:" });
                    if (Collection.Type == "deck")
                    {
                        sideboardListView.AddObject(new InventoryTotalsItem { DisplayName = "        Totals:" });
                        foreach (var card in items)
                        {
                            if (card.Board == "sideboard")
                                sideboardListView.AddObject(card);
                            else
                                cardListView.AddObject(card);
                        }
                    }
                    else
                        cardListView.AddObjects(items.ToList());
                }
                if (cardListView.PrimarySortColumn == null) // not yet sorted
                    cardListView.Sort(cardListView.AllColumns.FirstOrDefault(x => x.AspectName == "TimeAdded"), SortOrder.Ascending);

                UpdateTotals();
            }
        }
        // TODO: handle for sideboardListView as well
        public void AddFullInventoryCard(FullInventoryCard cardInstance)
        {
            cardListView.AddObject(cardInstance);
            cardListView.EnsureModelVisible(cardInstance);
            UpdateTotals();
        }

        public void AddFullInventoryCards(List<FullInventoryCard> cards, string board)
        {
            DefaultCurrency = SettingsManager.ApplicationSettings.DefaultCurrency;
            if (cards.Count > 0)
            {
                if (board == "sideboard")
                {
                    sideboardListView.AddObjects(cards);
                    sideboardListView.EnsureModelVisible(cards[cards.Count - 1]);
                }
                else
                {
                    cardListView.AddObjects(cards);
                    cardListView.EnsureModelVisible(cards[cards.Count - 1]);
                }
                UpdateTotals();
            }
        }

        private void RemoveFullInventoryCards(List<int> cardIds, FastObjectListView listView)
        {
            var cardsToRemove = listView.Objects.Cast<object>().Where(x => x is FullInventoryCard).Cast<FullInventoryCard>().Where(x => cardIds.Contains(x.InventoryId)).ToList();
            if (cardsToRemove.Count > 0)
                RemoveFullInventoryCards(cardsToRemove, listView);
        }

        private void RemoveFullInventoryCards(List<FullInventoryCard> cardsToRemove, FastObjectListView listView)
        {
            var inventoryCardsStillSelected = listView.SelectedObjects.Cast<object>().Where(x => x is FullInventoryCard).Cast<FullInventoryCard>().ToList();
            foreach (var card in cardsToRemove)
                if (inventoryCardsStillSelected.Contains(card))
                    inventoryCardsStillSelected.Remove(card);
            int indexAfterLast = cardsToRemove.Max(x => listView.IndexOf(x)) + 1;
            object objectAfterLast = null;
            if (indexAfterLast > -1 && indexAfterLast < listView.Objects.Count())
                objectAfterLast = listView.GetModelObject(indexAfterLast);
            listView.RemoveObjects(cardsToRemove);
            listView.SelectedObjects = inventoryCardsStillSelected;
            if (listView.SelectedObject == null)
            {
                if (objectAfterLast != null)
                    listView.SelectedObject = objectAfterLast;
                else
                {
                    if (listView.Objects.Count() > -1)
                        listView.SelectedIndex = listView.Objects.Count() - 1;
                }
            }
            UpdateTotals();
        }

        public void RemoveFullInventoryCards(List<int> cardIds, string board)
        {
            if (cardIds.Count > 0)
            {
                if (board == "mainboard")
                {
                    ignoreNextCardListViewSelectionChanged = true;
                    RemoveFullInventoryCards(cardIds, cardListView);
                }
                else
                {
                    ignoreNextSideboardListViewSelectionChanged = true;
                    RemoveFullInventoryCards(cardIds, sideboardListView);
                }
            }

        }
        public void RemoveFullInventoryCards(List<FullInventoryCard> cardsToRemove, string board)
        {
            if (cardsToRemove.Count > 0)
            {
                if (board == "mainboard")
                {
                    ignoreNextCardListViewSelectionChanged = true;
                    RemoveFullInventoryCards(cardsToRemove, cardListView);
                }
                else
                {
                    ignoreNextSideboardListViewSelectionChanged = true;
                    RemoveFullInventoryCards(cardsToRemove, sideboardListView);
                }
            }
        }

        private DialogResult ConfirmCardDeletion(string message = "The highlighted card(s) will be deleted. Are you sure you wish to continue?")
        {
            return MessageBox.Show(message, "Pending delete", MessageBoxButtons.YesNo);
        }

        #endregion Methods

        #region Events

        #region OLV Events
        private void fastObjectListView1_ModelCanDrop(object sender, ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            if (e.SourceModels[0] is OLVCardItem)
                e.InfoMessage = $"Add {e.SourceModels.Count} card{(e.SourceModels.Count == 1 ? "" : "s")} to {DocumentName}";
            else if (e.SourceModels[0] is FullInventoryCard fullInventoryCard)
            {
                if (e.ListView != e.SourceListView)
                {
                    var destination = DocumentName;
                    if (Collection.Type == "deck")
                        destination += e.ListView == cardListView ? "- mainboard" : "- sideboard";
                    e.InfoMessage = $"Add {e.SourceModels.Count} card{(e.SourceModels.Count == 1 ? "" : "s")} to {destination}";
                }
            }
            else
                e.Effect = DragDropEffects.None;
        }
        private void fastObjectListView1_ModelDropped(object sender, ModelDropEventArgs e)
        {
            var sourceForm = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.cardListView == e.SourceListView || x.sideboardListView == e.SourceListView);
            var args = new CardsDroppedEventArgs
            {
                Items = e.SourceModels as ArrayList,
                TargetCollection = Collection,
                SourceForm = sourceForm,
                TargetBoard = e.ListView == cardListView ? "mainboard" : "sideboard"
            };
            if (sourceForm != null)
                args.SourceBoard = e.SourceListView == sourceForm.cardListView ? "mainboard" : "sideboard";
            OnCardsDropped(args);
        }
        private void fastObjectListView1_CellEditFinished(object sender, CellEditEventArgs e)
        {
            var listView = sender as FastObjectListView;
            listView.Focus();
            if (e.RowObject is FullInventoryCard editedCard)
            {
                var args = new CardsUpdatedEventArgs { Items = new ArrayList { editedCard }, CollectionViewForm = this, Board = listView == cardListView ? "mainboard" : "sideboard" };
                var rowItem = listView.ModelToItem(editedCard);
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

                if (listView.SelectedObjects != null && 
                    !(!listView.SelectedObjects.Contains(editedCard) && e.Column.CheckBoxes))
                    foreach (FullInventoryCard card in listView.SelectedObjects)
                    {
                        if (card != editedCard)
                        {
                            if (e.Column.AspectName == "Tags")
                                card.Tags = editedCard.Tags;
                            else if (e.Column.AspectName == "Count")
                                card.Count = editedCard.Count;
                            else if (e.Column.AspectName == "Cost")
                                card.Cost = editedCard.Cost;
                            else if (e.Column.AspectName == "Foil")
                                card.Foil = editedCard.Foil;
                            else if (e.Column.AspectName == "Condition")
                                card.Condition = editedCard.Condition;
                            else if (e.Column.AspectName == "Finish")
                                card.Finish = editedCard.Finish;
                            args.Items.Add(card);
                        }
                    }
                OnCardsUpdated(args);
            }
        }
        private void fastObjectListView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            var listView = sender as FastObjectListView;
            if (listView.SelectedObjects?.Count > 0 && !(listView.SelectedObjects?.Count == 1 && listView.SelectedObject is InventoryTotalsItem))
            {
                if (e.KeyChar == '=' || e.KeyChar == '+')
                {
                    e.Handled = true;
                    foreach (var item in listView.SelectedObjects)
                        if (item is FullInventoryCard card)
                            card.Count++;
                    OnCardsUpdated(new CardsUpdatedEventArgs { Items = listView.SelectedObjects as ArrayList, CollectionViewForm = this, Board = listView == cardListView ? "mainboard" : "sideboard" });
                }
                else if (e.KeyChar == '-' || e.KeyChar == '_')
                {
                    e.Handled = true;
                    bool pendingDeletion = false;
                    foreach (var item in listView.SelectedObjects)
                    {
                        if (item is FullInventoryCard card)
                        {
                            card.Count--;
                            if (card.Count < 1)
                                pendingDeletion = true;
                        }
                    }
                    if (pendingDeletion)
                    {
                        if (ConfirmCardDeletion("Some of the highlighted cards will be deleted. Are you sure you wish to continue?") != DialogResult.Yes)
                        {
                            foreach (var item in listView.SelectedObjects)
                                if (item is FullInventoryCard card)
                                    card.Count++;
                            return;
                        }
                    }
                    OnCardsUpdated(new CardsUpdatedEventArgs { Items = listView.SelectedObjects as ArrayList, CollectionViewForm = this, Board = listView == cardListView ? "mainboard" : "sideboard" });
                }
                else if (e.KeyChar == '\r')
                {
                    e.Handled = true;
                    if (listView.SelectedObjects.Count > 0)
                    {
                        var cardsToPrice = new Dictionary<string, FullInventoryCard>();
                        foreach (var row in listView.SelectedObjects)
                            if (row is FullInventoryCard card && card.ScryfallId != null)
                                if (!cardsToPrice.ContainsKey(card.ScryfallId))
                                    cardsToPrice.Add(card.ScryfallId, row as FullInventoryCard);
                        if (cardsToPrice.Count > 0)
                            CardManager.FetchPrices(cardsToPrice.Values.ToList());
                        else
                            MessageBox.Show("No valid product IDs were found. Please try updating the set(s)");
                    }
                }
            }
        }

        private void fastObjectListView1_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Column != null && e.Column.IsEditable && e.Model is FullInventoryCard card)
            {
                if (e.ClickCount == 2 && !e.Column.CheckBoxes)
                {
                    e.ListView.StartCellEdit(e.Item, e.Item.SubItems.IndexOf(e.SubItem));
                    e.Handled = true;
                }
                else if (e.ClickCount == 1 && e.Column.CheckBoxes)
                {
                    var hotItemRect = e.HitTest.SubItem.Bounds;
                    int boxWidth = CheckBoxRenderer.CheckBoxWidth;
                    int boxHeight = CheckBoxRenderer.CheckBoxHeight;
                    int boxLeft = hotItemRect.Left + (hotItemRect.Right - hotItemRect.Left - boxWidth) / 2;
                    int boxTop = hotItemRect.Top + (hotItemRect.Bottom - hotItemRect.Top - boxHeight) / 2;
                    if (e.Location.X > boxLeft && e.Location.X < boxLeft + boxWidth     // checkbox clicked
                        && e.Location.Y > boxTop && e.Location.Y < boxTop + boxHeight)
                    {
                        e.ListView.ToggleSubItemCheckBox(card, e.Column);
                    }
                }
            }
        }

        private void fastObjectListView1_SelectionChanged(object sender, EventArgs e)
        {
            var listView = sender as FastObjectListView;
            if (listView == cardListView && ignoreNextCardListViewSelectionChanged)
            {
                ignoreNextCardListViewSelectionChanged = false;
                return;
            }
            else if (listView == sideboardListView && ignoreNextSideboardListViewSelectionChanged)
            {
                ignoreNextSideboardListViewSelectionChanged = false;
                return;
            }
            if (listView.SelectedObject is InventoryTotalsItem)
            {
                listView.SelectedObject = null;
                return;
            }
            if (listView.SelectedObjects != null && listView.SelectedObjects.Count > 0 && listView.SelectedObjects[0] is ScryfallMagicCardBase)
                OnCardSelected(new CardSelectedEventArgs { MagicCards = listView.SelectedObjects });
        }

        private void cardListView_SubItemChecking(object sender, SubItemCheckingEventArgs e)
        {
            if (e.RowObject is FullInventoryCard card)
            {
                if (!(card.has_foil && card.has_nonfoil))
                {
                    e.Canceled = true;
                }
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
            var listView = sender as FastObjectListView;
            if (listView.SelectedObjects?.Count > 0)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (ConfirmCardDeletion() == DialogResult.Yes)
                    {
                        foreach (FullInventoryCard cardItem in listView.SelectedObjects)
                            cardItem.Count = 0;
                        OnCardsUpdated(new CardsUpdatedEventArgs { Items = listView.SelectedObjects as ArrayList, CollectionViewForm = this, Board = listView == cardListView ? "mainboard" : "sideboard" });
                    }
                }
            }
        }

        private void cardListView_CellEditStarting(object sender, CellEditEventArgs e)
        {
            if (e.RowObject is FullInventoryCard card)
            {
                var listView = sender as FastObjectListView;
                int costIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Cost").Index;
                int condIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Condition").Index;
                int finishIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Finish").Index;
                if (e.SubItemIndex == costIndex)
                {
                    var editor = new NumericUpDown
                    {
                        Bounds = e.CellBounds,
                        DecimalPlaces = 2,
                        Maximum = 9999999,
                        Value = decimal.TryParse(e.Value?.ToString(), out decimal cellValue) ? cellValue : 0.0M
                    };
                    e.Control = editor;
                }
                else if (e.SubItemIndex == condIndex)
                {
                    EditorComboBox = new ComboBox { Bounds = e.CellBounds, DropDownStyle = ComboBoxStyle.DropDownList };
                    EditorComboBox.Items.AddRange(new object[] { "", "NM", "LP", "MP", "HP", "DG" });
                    foreach (var item in EditorComboBox.Items)
                        if (item.ToString() == card.Condition)
                        {
                            EditorComboBox.SelectedItem = item;
                            break;
                        }
                    e.Control = EditorComboBox;
                }
                else if (e.SubItemIndex == finishIndex)
                {
                    EditorComboBox = new ComboBox { Bounds = e.CellBounds, DropDownStyle = ComboBoxStyle.DropDownList };
                    EditorComboBox.Items.AddRange(card.finishes);
                    foreach (var item in EditorComboBox.Items)
                        if (item.ToString() == card.Finish)
                        {
                            EditorComboBox.SelectedItem = item;
                            break;
                        }
                    e.Control = EditorComboBox;
                }
            }
        }

        private void cardListView_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.RowObject is FullInventoryCard card)
            {
                var listView = sender as FastObjectListView;
                int costIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Cost").Index;
                int condIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Condition").Index;
                int finishIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Finish").Index;
                if (e.SubItemIndex == costIndex)
                {
                    if (double.TryParse(e.NewValue?.ToString(), out double cellValue))
                        card.Cost = cellValue;
                    listView.RefreshObject(card);
                    e.Cancel = true;
                }
                else if (e.SubItemIndex == condIndex)
                {
                    if (EditorComboBox.SelectedIndex == 0)
                        card.Condition = null;
                    else
                        card.Condition = EditorComboBox.SelectedItem?.ToString();
                    listView.RefreshObject(card);
                    e.Cancel = true;
                }
                else if (e.SubItemIndex == finishIndex)
                {
                    string priceString = "";
                    var DefaultPaperCurrency = SettingsManager.ApplicationSettings.DefaultPaperCurrency;
                    card.Finish = EditorComboBox.SelectedItem?.ToString();
                    if (card.Platform == "MTGO")
                        card.prices.TryGetValue("tix", out priceString);
                    else if (card.Platform == "Paper")
                        card.prices.TryGetValue($"{DefaultPaperCurrency.ToLower()}{(card.Finish != "nonfoil" ? $"_{card.Finish}" : "")}", out priceString);
                    if (!string.IsNullOrEmpty(priceString))
                        card.Price = Convert.ToDouble(priceString);

                    listView.RefreshObject(card);
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
            if (cardListView.SelectedObjects?.Count < 1 || cardListView.SelectedObject is InventoryTotalsItem)
                e.Cancel = true;
            else
            {
                if (cardListView.SelectedObject is FullInventoryCard card && card.Count > 1)
                    splitToolStripMenuItem.Visible = true;
                else
                {
                    splitToolStripMenuItem.Visible = false;
                    combineToolStripMenuItem.Visible = false;
                    if (cardListView.SelectedObjects != null && cardListView.SelectedObjects.Count > 1)
                    {
                        var selectedCards = cardListView.SelectedObjects.Cast<object>().Where(x => x is FullInventoryCard).Cast<FullInventoryCard>();
                        var firstCard = selectedCards.First();
                        foreach (var selectedCard in selectedCards)
                            if (selectedCard.ScryfallId != firstCard.ScryfallId)
                                return;

                        combineToolStripMenuItem.Visible = true;
                    }
                }
            }
        }

        private void splitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cardListView.SelectedObject is FullInventoryCard card && card.Count > 1)
            {
                int total = card.Count.Value;
                var card1 = card.InventoryCard;
                card1.Count = total / 2;
                var card2 = card.InventoryCard;
                card2.Count = total - card1.Count;
                card2.InventoryId = 0;

                try
                {
                    using (var context = new ScryfallCardsDbContext())
                    {
                        context.Update(card1);
                        context.Add(card2);
                        context.SaveChanges();
                        card.Count = card1.Count;
                        cardListView.RefreshObject(card);
                        int selectedIndex = cardListView.SelectedIndex;
                        cardListView.AddObject(card2.ToFullCard(context));
                        cardListView.SelectedIndex = selectedIndex;
                    }
                }
                catch (Exception ex)
                {
                    DebugOutput.WriteLine(ex.ToString());
                    MessageBox.Show("Failed to split cards");
                }
            }
        }

        private void combineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cardListView.SelectedObjects != null && cardListView.SelectedObjects.Count > 1)
            {
                var selectedCards = cardListView.SelectedObjects.Cast<object>().Where(x => x is FullInventoryCard).Cast<FullInventoryCard>();
                var firstCard = selectedCards.First();
                foreach (var selectedCard in selectedCards)
                    if (selectedCard.ScryfallId != firstCard.ScryfallId)
                    {
                        MessageBox.Show("Unable to combine cards");
                        return;
                    }

                int totalCount = selectedCards.Sum(x => x.Count).Value;
                double avgCost = Math.Round(selectedCards.Sum(x => x.Count * x.Cost).Value / totalCount, 2);
                firstCard.Cost = avgCost;
                firstCard.Count = totalCount;
                using (var context = new ScryfallCardsDbContext())
                {
                    context.Update(firstCard.InventoryCard);
                    foreach (var selectedCard in selectedCards)
                        if (selectedCard != firstCard)
                        {
                            cardListView.RemoveObject(selectedCard);
                            context.Remove(selectedCard.InventoryCard);
                        }
                    context.SaveChanges();
                }
                cardListView.SelectedObject = firstCard;
                UpdateTotals();
            }
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

                private static int CompareText(string x, string y)
                {
                    if (x == null && y != null)
                        return -1;
                    else if (x != null && y == null)
                        return 1;
                    else if (x == null && y == null)
                        return 0;
                    else
                        return x.CompareTo(y);
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
                            result = (x as FullInventoryCard).Name.CompareTo((y as FullInventoryCard).Name);
                        else if (AspectName == "type")
                            result = (x as FullInventoryCard).type_line.CompareTo((y as FullInventoryCard).type_line);
                        else if (AspectName == "Edition")
                            result = (x as FullInventoryCard).set_name.CompareTo((y as FullInventoryCard).set_name);
                        else if (AspectName == "ManaCost")
                        {
                            string valueX = (x as FullInventoryCard).mana_cost ?? "";
                            string valueY = (y as FullInventoryCard).mana_cost ?? "";
                            result = valueX.CompareTo(valueY);
                        }
                        else if (AspectName == "TimeAdded")
                            result = (x as FullInventoryCard).SortableTimeAdded.CompareTo((y as FullInventoryCard).SortableTimeAdded);
                        else if (AspectName == "Foil")
                            result = (x as FullInventoryCard).Foil.CompareTo((y as FullInventoryCard).Foil);
                        else if (AspectName == "Cost")
                            result = CompareNullableDoubles((x as FullInventoryCard).Cost, (y as FullInventoryCard).Cost);
                        else if (AspectName == "Price")
                            result = CompareNullableDoubles((x as FullInventoryCard).Price, (y as FullInventoryCard).Price);
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
                            result = CompareText((x as FullInventoryCard).Tags, (y as FullInventoryCard).Tags);
                        else if (AspectName == "text")
                            result = CompareText((x as FullInventoryCard).text, (y as FullInventoryCard).text);
                        else if (AspectName == "Delta")
                            result = CompareNullableDoubles((x as FullInventoryCard).Delta, (y as FullInventoryCard).Delta);
                        else if (AspectName == "X")
                            result = CompareNullableDoubles((x as FullInventoryCard).Percent, (y as FullInventoryCard).Percent);
                        return SortOrder == SortOrder.Ascending ? result : -1 * result;
                    }
                }
            }
        }

        #endregion Classes

        private void CollectionViewForm_SizeChanged(object sender, EventArgs e)
        {
            cardListView.Width = Width / 2;
        }
    }
}