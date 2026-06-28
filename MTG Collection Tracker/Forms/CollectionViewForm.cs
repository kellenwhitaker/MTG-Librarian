using BrightIdeasSoftware;
using KW.WinFormsUI.Docking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
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
                : (x as InventoryCard).Tags?.ToUpper().Contains(commentsFilterTextBox.Text.ToUpper()) ?? false;
        }

        private Predicate<object> GetCardNameFilter()
        {
            return x => cardNameFilterTextBox.Text == ""
                ? true
                : (x as InventoryCard).Name?.ToUpper().Contains(cardNameFilterTextBox.Text.ToUpper()) ?? false;
        }

        private Predicate<object> GetCardTextFilter()
        {
            return x => cardTextFilterTextBox.Text == ""
                ? true
                : (x as InventoryCard).text?.ToUpper().Contains(cardTextFilterTextBox.Text.ToUpper()) ?? false;
        }

        private Predicate<object> GetManaCostFilter()
        {
            Predicate<object> combinedFilter = x => true;

            if (whiteManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as InventoryCard).mana_cost?.Contains("W") ?? false);
            if (blueManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as InventoryCard).mana_cost?.Contains("U") ?? false);
            if (blackManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as InventoryCard).mana_cost?.Contains("B") ?? false);
            if (redManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as InventoryCard).mana_cost?.Contains("R") ?? false);
            if (greenManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as InventoryCard).mana_cost?.Contains("G") ?? false);
            if (colorlessManaButton.Checked) combinedFilter = combinedFilter.And(x => (x as InventoryCard).mana_cost?.Contains("C") ?? false);
            if (genericManaButton.Checked) combinedFilter = combinedFilter.And(x => ((x as InventoryCard).mana_cost?.Contains("X") ?? false)
                || ((x as InventoryCard).mana_cost?.Any(c => char.IsDigit(c)) ?? false));
            return combinedFilter;
        }

        private Predicate<object> GetRarityFilter()
        {
            string rarityFilterText = rarityFilterComboBox.Text.ToUpper();
            return x => rarityFilterText == "ALL RARITIES" || rarityFilterText == ""
                ? true
                : (x as InventoryCard).rarity?.ToUpper() == rarityFilterText;
        }

        private Predicate<object> GetTypeFilter()
        {
            return x => typeFilterTextBox.Text == ""
                ? true
                : (x as InventoryCard).type_line?.ToUpper().Contains(typeFilterTextBox.Text.ToUpper()) ?? false;
        }

        private Predicate<object> GetSetFilter()
        {
            return x => setFilterTextBox.Text == ""
                ? true
                : (x as InventoryCard).set_name?.ToUpper().Contains(setFilterTextBox.Text.ToUpper()) ?? false;
        }

        #endregion Filters
        
        public void UpdateTotals(FastObjectListView listView)
        {
            var totalsRow = (listView.Objects as ArrayList)[0] as InventoryTotalsItem;
            totalsRow.Price = 0;
            totalsRow.Cost = 0;
            totalsRow.Count = 0;
            foreach (var row in listView.FilteredObjects)
            {
                if (row is InventoryCardCluster cluster)
                {
                    int cardCount = cluster.Count;
                    totalsRow.Count += cardCount;
                    totalsRow.Price += cluster.Price * cardCount;
                    totalsRow.Cost += cluster.Cost * cardCount;
                }
                else if (row is InventoryCard card)
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
                    collapsedViewToolStripMenuItem.Visible = true;
                    CountColumn.DisplayIndex = sideboardCountColumn.DisplayIndex = 1;
                    tcgplayerMarketPriceColumn.DisplayIndex = sideboardPriceColumn.DisplayIndex = 2;
                    ManaCost.DisplayIndex = sideboardManaCostColumn.DisplayIndex =  3;
                    cardListView.ShowGroups = sideboardListView.ShowGroups = true;
                    cardListView.AlwaysGroupByColumn = CardName;
                    sideboardListView.AlwaysGroupByColumn = sideboardCardNameColumn;
                    CardName.GroupKeyGetter = sideboardCardNameColumn.GroupKeyGetter = delegate (object rowObject) {
                        if (rowObject is InventoryCard card)
                        {
                            if (card.Board == "mainboard" && Collection.Commander.HasValue && card.InventoryId == Collection.Commander.Value)
                                return "0Commander";
                            else
                                return card.ListViewGroupKey;
                        }
                        else if (rowObject is InventoryTotalsItem)
                            return "";
                        return null;
                    };

                    CardName.GroupKeyToTitleConverter = delegate (object groupKey) {
                        if (groupKey?.ToString() == "")
                            return "Mainboard";
                        else
                        { 
                            if (Collection.Commander.HasValue && groupKey.ToString() == "0Commander")
                                return "Commander";

                            var count = 0;
                            foreach (var item in cardListView.Objects)
                            {
                                if (item is InventoryCardCluster cluster && cluster.ListViewGroupKey == groupKey.ToString())
                                    count += cluster.Count;
                                else if (item is InventoryCard card && card.ListViewGroupKey == groupKey.ToString() && card.Count.HasValue)
                                    count += card.Count.Value;
                            }
                            return $"{groupKey?.ToString()} ({count})";
                        }
                    };

                    sideboardCardNameColumn.GroupKeyToTitleConverter = delegate (object groupKey) {
                        if (groupKey?.ToString() == "")
                            return "Sideboard";
                        else
                        {
                            var count = 0;
                            foreach (var item in sideboardListView.Objects)
                            {
                                if (item is InventoryCardCluster cluster && cluster.ListViewGroupKey == groupKey.ToString())
                                    count += cluster.Count;
                                else if (item is InventoryCard card && card.ListViewGroupKey == groupKey.ToString() && card.Count.HasValue)
                                    count += card.Count.Value;
                            }
                            return $"{groupKey?.ToString()} ({count})";
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
                        fullCard.Price = fullCard.FindPrice(DefaultPaperCurrency);                        
                    }
                    cardListView.AddObject(new InventoryTotalsItem { DisplayName = "        Totals:" });
                    if (Collection.Type == "deck")
                    {
                        sideboardListView.AddObject(new InventoryTotalsItem { DisplayName = "        Totals:" });
                        if (Collection.CollapsedView.HasValue && Collection.CollapsedView.Value)
                        {
                            var mainboardClusterDictionary = new Dictionary<string, InventoryCardCluster>();
                            var sideboardClusterDictionary = new Dictionary<string, InventoryCardCluster>();
                            foreach (var card in items)
                                if (card.Board == "sideboard")
                                {
                                    if (sideboardClusterDictionary.ContainsKey(card.Name))
                                        sideboardClusterDictionary[card.Name].Cards.Add(card);
                                    else
                                        sideboardClusterDictionary.Add(card.Name, new InventoryCardCluster(card));
                                }
                                else
                                {
                                    if (mainboardClusterDictionary.ContainsKey(card.Name))
                                        mainboardClusterDictionary[card.Name].Cards.Add(card);
                                    else
                                        mainboardClusterDictionary.Add(card.Name, new InventoryCardCluster(card));
                                }
                            cardListView.AddObjects(mainboardClusterDictionary.Values);
                            sideboardListView.AddObjects(sideboardClusterDictionary.Values);
                        }
                        else
                        {
                            foreach (var card in items)
                            {
                                if (card.Board == "sideboard")
                                    sideboardListView.AddObject(card);
                                else
                                    cardListView.AddObject(card);
                            }
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
        public void AddFullInventoryCard(InventoryCard cardInstance)
        {
            cardListView.AddObject(cardInstance);
            cardListView.EnsureModelVisible(cardInstance);
            UpdateTotals();
        }

        public void AddFullInventoryCards(List<InventoryCard> cards, string board)
        {
            DefaultCurrency = SettingsManager.ApplicationSettings.DefaultCurrency;
            if (cards.Count > 0)
            {
                if (board == "sideboard")
                {
                    if (Collection.CollapsedView.HasValue && Collection.CollapsedView.Value)
                    {
                        bool foundCluster;
                        foreach (var card in cards)
                        {
                            foundCluster = false;
                            foreach (var item in sideboardListView.Objects)
                                if (item is InventoryCardCluster cluster)
                                    if (cluster.Name == card.Name)
                                    {
                                        cluster.Cards.Add(card);
                                        sideboardListView.RefreshObject(cluster);
                                        foundCluster = true;
                                        break;
                                    }
                            if (!foundCluster)
                            {
                                var newCluster = new InventoryCardCluster(card);
                                sideboardListView.AddObject(newCluster);
                                sideboardListView.EnsureModelVisible(newCluster);
                            }
                        }
                        sideboardListView.BuildGroups();
                    }
                    else
                    {
                        sideboardListView.AddObjects(cards);
                        sideboardListView.EnsureModelVisible(cards[cards.Count - 1]);
                    }
                }
                else
                {
                    if (Collection.CollapsedView.HasValue && Collection.CollapsedView.Value)
                    {
                        bool foundCluster;
                        foreach (var card in cards)
                        {
                            foundCluster = false;
                            foreach (var item in cardListView.Objects)
                                if (item is InventoryCardCluster cluster)
                                    if (cluster.Name == card.Name)
                                    {
                                        cluster.Cards.Add(card);
                                        cardListView.RefreshObject(cluster);
                                        foundCluster = true;
                                        break;
                                    }
                            if (!foundCluster)
                            {
                                var newCluster = new InventoryCardCluster(card);
                                cardListView.AddObject(newCluster);
                                cardListView.EnsureModelVisible(newCluster);
                            }
                        }
                        cardListView.BuildGroups();
                    }
                    else
                    {
                        cardListView.AddObjects(cards);
                        cardListView.EnsureModelVisible(cards[cards.Count - 1]);
                    }
                }
                UpdateTotals();
            }
        }

        private void RemoveFullInventoryCards(List<int> cardIds, FastObjectListView listView)
        {
            var cardsToRemove = listView.Objects.Cast<object>().Where(x => x is InventoryCard).Cast<InventoryCard>().Where(x => cardIds.Contains(x.InventoryId)).ToList();
            if (cardsToRemove.Count > 0)
                RemoveFullInventoryCards(cardsToRemove, listView);
        }

        private void RemoveFullInventoryCards(List<InventoryCard> cardsToRemove, FastObjectListView listView)
        {
            var inventoryCardsStillSelected = listView.SelectedObjects.Cast<object>().Where(x => x is InventoryCard).Cast<InventoryCard>().ToList();
            var clustersToRemove = new List<InventoryCardCluster>();

            if (Collection.CollapsedView.HasValue && Collection.CollapsedView.Value)
            {
                foreach (var item in listView.Objects)
                    if (item is InventoryCardCluster cluster)
                    {
                        foreach (var card in cardsToRemove)
                            if (cluster.Cards.Contains(card))
                            {
                                clustersToRemove.Add(cluster);
                                break;
                            }
                    }
            }
            if (listView.SelectedObjects.Count > 0 && listView.SelectedObjects[0] is InventoryCard)
                foreach (var card in cardsToRemove)
                    if (inventoryCardsStillSelected.Contains(card))
                        inventoryCardsStillSelected.Remove(card);
            int indexAfterLast = cardsToRemove.Max(x => listView.IndexOf(x)) + 1;
            object objectAfterLast = null;
            if (indexAfterLast > -1 && indexAfterLast < listView.Objects.Count())
                objectAfterLast = listView.GetModelObject(indexAfterLast);
            if (clustersToRemove.Count > 0)
                listView.RemoveObjects(clustersToRemove);
            else
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
        public void RemoveFullInventoryCards(List<InventoryCard> cardsToRemove, string board)
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
            e.Effect = DragDropEffects.None;
            
            if (e.SourceModels == null || e.SourceModels.Count == 0)
                return;

            if (e.ListView == e.SourceListView)
                return;

            var firstModel = e.SourceModels[0];
            int count = 0;

            if (firstModel is OLVCardItem)
            {
                count = e.SourceModels.Count;
            }
            else if (firstModel is InventoryCardCluster)
            {
                foreach (InventoryCardCluster cluster in e.SourceModels)
                    count += cluster.Count;
            }
            else if (firstModel is InventoryCard)
            {                
                foreach (InventoryCard card in e.SourceModels)
                    count += (int)card.Count;
            }
            else
            {
                return;
            }

            var destination = DocumentName;
            if (Collection.Type == "deck")
                destination += e.ListView == cardListView ? " - mainboard" : " - sideboard";

            e.InfoMessage = $"Add {count} card{(count == 1 ? "" : "s")} to {destination}";
            e.Effect = DragDropEffects.Move;
        }
        private void fastObjectListView1_ModelDropped(object sender, ModelDropEventArgs e)
        {
            var sourceForm = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.cardListView == e.SourceListView || x.sideboardListView == e.SourceListView);
            var args = new CardsDroppedEventArgs
            {
                TargetCollection = Collection,
                SourceForm = sourceForm,
                TargetBoard = e.ListView == cardListView ? "mainboard" : "sideboard"
            };

            if (e.SourceModels[0] is InventoryCardCluster)
            {
                var items = new ArrayList();
                foreach (InventoryCardCluster cluster in e.SourceModels)
                    items.AddRange(cluster.Cards);
                args.Items = items;
            }
            else
                args.Items = e.SourceModels as ArrayList;

            if (sourceForm != null)
                args.SourceBoard = e.SourceListView == sourceForm.cardListView ? "mainboard" : "sideboard";
            OnCardsDropped(args);
        }
        private void fastObjectListView1_CellEditFinished(object sender, CellEditEventArgs e)
        {
            var listView = sender as FastObjectListView;
            listView.Focus();
            if (e.RowObject is InventoryCard editedCard)
            {
                var args = new CardsUpdatedEventArgs { CollectionViewForm = this, Board = listView == cardListView ? "mainboard" : "sideboard" };
                if (editedCard is InventoryCardCluster cluster)
                    args.Items = new ArrayList(cluster.Cards);
                else
                    args.Items = new ArrayList { editedCard };
                var rowItem = listView.ModelToItem(editedCard);
                if (e.Column.AspectName == "Count" && ((e.RowObject is InventoryCardCluster cardCluster && cardCluster.Cards[0].Count < 1) || editedCard.Count < 1))
                {
                    if (ConfirmCardDeletion() != DialogResult.Yes)
                    {
                        if (Int32.TryParse(e.Value.ToString(), out int oldCount))
                        {
                            if (e.RowObject is InventoryCardCluster inventoryCardCluster)
                                inventoryCardCluster.Cards[0].Count = oldCount;
                            else
                                editedCard.Count = oldCount;
                        }
                        else
                        {
                            if (e.RowObject is InventoryCardCluster inventoryCardCluster)
                                inventoryCardCluster.Cards[0].Count = 1;
                            else
                                editedCard.Count = 1;
                        }
                        return;
                    }
                }

                if (!(e.RowObject is InventoryCardCluster) &&  listView.SelectedObjects != null && 
                    !(!listView.SelectedObjects.Contains(editedCard) && e.Column.CheckBoxes))
                    foreach (InventoryCard card in listView.SelectedObjects)
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
                    if (listView.SelectedObjects[0] is InventoryCardCluster cluster)
                    {
                        if (listView.SelectedObjects.Count == 1 && cluster.Cards.Count == 1)
                        {
                            var cardsUpdated = new List<InventoryCard>();
                            foreach (var card in cluster.Cards)
                            {
                                cardsUpdated.Add(card);
                                card.Count++;
                            }
                            OnCardsUpdated(new CardsUpdatedEventArgs { Items =  new ArrayList(cardsUpdated), CollectionViewForm = this, Board = listView == cardListView ? "mainboard" : "sideboard" });
                        }
                    }
                    else
                    {
                        foreach (var item in listView.SelectedObjects)
                            if (item is InventoryCard card)
                                card.Count++;
                        OnCardsUpdated(new CardsUpdatedEventArgs { Items = listView.SelectedObjects as ArrayList, CollectionViewForm = this, Board = listView == cardListView ? "mainboard" : "sideboard" });
                    }
                }
                else if (e.KeyChar == '-' || e.KeyChar == '_')
                {
                    e.Handled = true;
                    bool pendingDeletion = false;
                    var cardsUpdated = new List<InventoryCard>();
                    if (listView.SelectedObjects[0] is InventoryCardCluster cluster)
                    {
                        if (listView.SelectedObjects.Count == 1 && cluster.Cards.Count == 1)
                        {
                            foreach (var card in cluster.Cards)
                            {
                                cardsUpdated.Add(card);
                                card.Count--;
                                if (card.Count < 1)
                                    pendingDeletion = true;
                            }
                        }
                        else
                            return;
                    }
                    else
                        foreach (var item in listView.SelectedObjects)
                        {
                            if (item is InventoryCard card)
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
                            if (listView.SelectedObjects[0] is InventoryCardCluster cardCluster)
                            {
                                if (listView.SelectedObjects.Count == 1 && cardCluster.Cards.Count == 1)
                                    foreach (var card in cardCluster.Cards)
                                        card.Count++;
                            }
                            else
                                foreach (var item in listView.SelectedObjects)
                                    if (item is InventoryCard card)
                                        card.Count++;
                            return;
                        }
                    }
                    if (cardsUpdated.Count > 0)
                        OnCardsUpdated(new CardsUpdatedEventArgs { Items = new ArrayList(cardsUpdated), CollectionViewForm = this, Board = listView == cardListView ? "mainboard" : "sideboard" });
                    else
                        OnCardsUpdated(new CardsUpdatedEventArgs { Items = listView.SelectedObjects as ArrayList, CollectionViewForm = this, Board = listView == cardListView ? "mainboard" : "sideboard" });
                }
                else if (e.KeyChar == '\r')
                {
                    e.Handled = true;
                    if (listView.SelectedObjects.Count > 0)
                    {
                        var cardsToPrice = new Dictionary<string, InventoryCard>();
                        if (listView.SelectedObjects[0] is InventoryCardCluster cluster)
                        {
                            foreach (var card in cluster.Cards)
                                if (card.ScryfallId != null)
                                    if (!cardsToPrice.ContainsKey(card.ScryfallId))
                                        cardsToPrice.Add(card.ScryfallId, card);
                        }
                        else
                            foreach (var row in listView.SelectedObjects)
                                if (row is InventoryCard card && card.ScryfallId != null)
                                    if (!cardsToPrice.ContainsKey(card.ScryfallId))
                                        cardsToPrice.Add(card.ScryfallId, row as InventoryCard);
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
            if (e.Column != null && e.Column.IsEditable && e.Model is InventoryCard card)
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
            {
                if (listView.SelectedObjects[0] is InventoryCardCluster cluster)
                {
                    OnCardSelected(new CardSelectedEventArgs { MagicCards = cluster.Cards, Cluster = true});
                }
                else
                {
                    OnCardSelected(new CardSelectedEventArgs { MagicCards = listView.SelectedObjects });
                }
            }
        }

        private void cardListView_SubItemChecking(object sender, SubItemCheckingEventArgs e)
        {
            if (e.RowObject is InventoryCard card)
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
                        if (cardListView.SelectedObjects[0] is InventoryCardCluster)
                        {
                            var cardList = new ArrayList();
                            foreach (InventoryCardCluster cluster in cardListView.SelectedObjects)
                            {
                                foreach (var card in cluster.Cards)
                                {
                                    cardList.Add(card);
                                    card.Count = 0;
                                }
                            }
                            OnCardsUpdated(new CardsUpdatedEventArgs { Items = cardList, CollectionViewForm = this, Board = sender == cardListView ? "mainboard" : "sideboard" });
                        }
                        else if (cardListView.SelectedObjects[0] is InventoryCard)
                        {
                            foreach (InventoryCard cardItem in cardListView.SelectedObjects)
                                cardItem.Count = 0;

                            OnCardsUpdated(new CardsUpdatedEventArgs { Items = cardListView.SelectedObjects as ArrayList, CollectionViewForm = this, Board = sender == cardListView ? "mainboard" : "sideboard" });
                        }
                    }
                }
            }
        }

        private void cardListView_CellEditStarting(object sender, CellEditEventArgs e)
        {
            var listView = sender as FastObjectListView;
            if (e.RowObject is InventoryCardCluster cluster)
            {
                int countIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Count").Index;
                if (e.SubItemIndex == countIndex && cluster.Cards.Count > 1)
                {
                    e.Cancel = true;
                    return;
                }
                if (listView.SelectedObjects?.Count > 1)
                {
                    e.Cancel = true;
                    return;
                }
                string scryfallId = null;
                foreach (var cardItem in cluster.Cards)
                {
                    if (scryfallId == null)
                        scryfallId = cardItem.ScryfallId;
                    else if (scryfallId != cardItem.ScryfallId)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            if (e.RowObject is InventoryCard card)
            {
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
                    if (card is InventoryCardCluster cardCluster)
                        cardCluster.finishes = cardCluster.Cards[0].finishes;
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
            if (e.RowObject is InventoryCard card)
            {
                var listView = sender as FastObjectListView;
                int costIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Cost").Index;
                int condIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Condition").Index;
                int finishIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Finish").Index;
                int countIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Count").Index;
                int tagsIndex = listView.AllColumns.FirstOrDefault(x => x.AspectName == "Tags").Index;
                if (e.SubItemIndex == costIndex)
                {
                    if (double.TryParse(e.NewValue?.ToString(), out double cellValue))
                    {
                        if (e.RowObject is InventoryCardCluster cluster)
                        {
                            foreach (var cardItem in cluster.Cards)
                                cardItem.Cost = cellValue;
                        }
                        else
                            card.Cost = cellValue;
                    }
                    listView.RefreshObject(card);
                    e.Cancel = true;
                }
                else if (e.SubItemIndex == condIndex)
                {
                    if (e.RowObject is InventoryCardCluster cluster)
                    {
                        foreach (var cardItem in cluster.Cards)
                        {
                            if (EditorComboBox.SelectedIndex == 0)
                                cardItem.Condition = null;
                            else
                                cardItem.Condition = EditorComboBox.SelectedItem?.ToString();
                        }
                    }
                    else
                    {
                        if (EditorComboBox.SelectedIndex == 0)
                            card.Condition = null;
                        else
                            card.Condition = EditorComboBox.SelectedItem?.ToString();
                    }
                    listView.RefreshObject(card);
                    e.Cancel = true;
                }
                else if (e.SubItemIndex == finishIndex)
                {                    
                    var DefaultPaperCurrency = SettingsManager.ApplicationSettings.DefaultPaperCurrency;
                    if (card is InventoryCardCluster cluster)
                    {
                        foreach (var cardItem in cluster.Cards)
                        {
                            cardItem.Finish = EditorComboBox.SelectedItem?.ToString();
                            cardItem.Price = cardItem.FindPrice(DefaultPaperCurrency);
                        }
                    }
                    else
                    {
                        card.Price = card.FindPrice(DefaultPaperCurrency);
                    }

                    listView.RefreshObject(card);
                    e.Cancel = true;
                }
                else if (e.SubItemIndex == countIndex)
                {
                    if (e.RowObject is InventoryCardCluster cluster)
                    {
                        if (int.TryParse(e.NewValue?.ToString(), out int cellValue))
                            foreach (var cardItem in cluster.Cards)
                                cardItem.Count = cellValue;                            
                        listView.RefreshObject(card);
                        e.Cancel = true;
                    }
                }
                else if (e.SubItemIndex == tagsIndex)
                {
                    if (e.RowObject is InventoryCardCluster cluster)
                    {
                        foreach (var cardItem in cluster.Cards)
                            cardItem.Tags = e.NewValue?.ToString();
                        listView.RefreshObject(card);
                        e.Cancel = true;
                    }
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
                    var board = cardListViewMenuStrip.SourceControl == cardListView ? "mainboard" : "sideboard";
                    if (cardListView.SelectedObjects[0] is InventoryCardCluster)
                    {
                        var cardList = new ArrayList();
                        foreach (InventoryCardCluster cluster in cardListView.SelectedObjects)
                        {
                            foreach (var card in cluster.Cards)
                            {
                                cardList.Add(card);
                                card.Count = 0;
                            }
                        }
                        OnCardsUpdated(new CardsUpdatedEventArgs { Items = cardList, CollectionViewForm = this, Board = board});
                    }
                    else if (cardListView.SelectedObjects[0] is InventoryCard)
                    {
                        foreach (InventoryCard cardItem in cardListView.SelectedObjects)
                            cardItem.Count = 0;

                        OnCardsUpdated(new CardsUpdatedEventArgs { Items = cardListView.SelectedObjects as ArrayList, CollectionViewForm = this, Board = board });
                    }  
                }
            }
        }

        private void cardListViewMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var listView = cardListViewMenuStrip.SourceControl as FastObjectListView;
            if (listView.SelectedObjects?.Count < 1 || listView.SelectedObject is InventoryTotalsItem)
                e.Cancel = true;
            else
            {
                collapsedViewToolStripMenuItem.Text = (Collection.CollapsedView.HasValue && Collection.CollapsedView.Value) ? "Expand View" : "Collapse View";
                splitToolStripMenuItem.Visible = false;
                combineToolStripMenuItem.Visible = false;
                makeCommanderToolStripMenuItem.Visible = false;
                if (listView.SelectedObject is InventoryCard card)
                {
                    if ((!Collection.CollapsedView.HasValue || !Collection.CollapsedView.Value) && card.Count > 1)
                    {
                        splitToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        if (Collection.Type == "deck" && card.Board == "mainboard" && card.type_line.Contains("Legendary") && card.type_line.Contains("Creature"))
                        {
                            makeCommanderToolStripMenuItem.Visible = true;
                            if (Collection.Commander.HasValue && card.InventoryId == Collection.Commander.Value)
                            {
                                makeCommanderToolStripMenuItem.Text = "Remove as Commander";
                            }
                            else
                            {
                                makeCommanderToolStripMenuItem.Text = "Make Commander";
                            }
                        }                        
                    }
                }

                if (listView.SelectedObjects != null && listView.SelectedObjects.Count > 1)
                {
                    var selectedCards = listView.SelectedObjects.Cast<object>().Where(x => x is InventoryCard).Cast<InventoryCard>();
                    var firstCard = selectedCards.First();
                    foreach (var selectedCard in selectedCards)
                        if (selectedCard.ScryfallId != firstCard.ScryfallId)
                            return;

                    combineToolStripMenuItem.Visible = true;
                }
            }
        }

        private void splitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cardListView.SelectedObject is InventoryCard card && card.Count > 1)
            {
                int total = card.Count.Value;
                var card1 = card.InventoryCardBase;
                card1.Count = total / 2;
                var card2 = card.InventoryCardBase;
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
                var selectedCards = cardListView.SelectedObjects.Cast<object>().Where(x => x is InventoryCard).Cast<InventoryCard>();
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
                    context.Update(firstCard.InventoryCardBase);
                    foreach (var selectedCard in selectedCards)
                        if (selectedCard != firstCard)
                        {
                            cardListView.RemoveObject(selectedCard);
                            context.Remove(selectedCard.InventoryCardBase);
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
                            result = (x as InventoryCard).SortableNumber.CompareTo((y as InventoryCard).SortableNumber);
                        else if (AspectName == "PaddedName")
                            result = (x as InventoryCard).Name.CompareTo((y as InventoryCard).Name);
                        else if (AspectName == "type")
                            result = (x as InventoryCard).type_line.CompareTo((y as InventoryCard).type_line);
                        else if (AspectName == "Edition")
                            result = (x as InventoryCard).set_name.CompareTo((y as InventoryCard).set_name);
                        else if (AspectName == "ManaCost")
                        {
                            string valueX = (x as InventoryCard).mana_cost ?? "";
                            string valueY = (y as InventoryCard).mana_cost ?? "";
                            result = valueX.CompareTo(valueY);
                        }
                        else if (AspectName == "TimeAdded")
                            result = (x as InventoryCard).SortableTimeAdded.CompareTo((y as InventoryCard).SortableTimeAdded);
                        else if (AspectName == "Foil")
                            result = (x as InventoryCard).Foil.CompareTo((y as InventoryCard).Foil);
                        else if (AspectName == "Cost")
                            result = CompareNullableDoubles((x as InventoryCard).Cost, (y as InventoryCard).Cost);
                        else if (AspectName == "Price")
                            result = CompareNullableDoubles((x as InventoryCard).Price, (y as InventoryCard).Price);
                        else if (AspectName == "Count")
                        {
                            var cx = (x as InventoryCard).Count;
                            var cy = (y as InventoryCard).Count;
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
                            result = CompareText((x as InventoryCard).Tags, (y as InventoryCard).Tags);
                        else if (AspectName == "text")
                            result = CompareText((x as InventoryCard).text, (y as InventoryCard).text);
                        else if (AspectName == "Delta")
                            result = CompareNullableDoubles((x as InventoryCard).Delta, (y as InventoryCard).Delta);
                        else if (AspectName == "X")
                            result = CompareNullableDoubles((x as InventoryCard).Percent, (y as InventoryCard).Percent);
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
        private void makeCommanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var listView = cardListViewMenuStrip.SourceControl as FastObjectListView;
            if (listView == cardListView && listView.SelectedObject is InventoryCard card)
            {
                if (Collection.Commander.HasValue && card.InventoryId == Collection.Commander.Value)
                {
                    Collection.Commander = null;
                }
                else
                {
                    Collection.Commander = card.InventoryId;
                }
                
                using (var context = new ScryfallCardsDbContext())
                {
                    context.Update(Collection);
                    context.SaveChanges();
                }

                listView.BuildGroups();
            }
        }
        private void collapsedViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Collection.CollapsedView.HasValue && Collection.CollapsedView.Value)
            {
                Collection.CollapsedView = false;
                var items = new List<object>();
                foreach (var item in cardListView.Objects)
                {
                    if (item is InventoryCardCluster cluster)
                        items.AddRange(cluster.Cards);
                    else
                        items.Add(item);
                }
                cardListView.ClearObjects();
                cardListView.AddObjects(items);

                items.Clear();
                foreach (var item in sideboardListView.Objects)
                {
                    if (item is InventoryCardCluster cluster)
                        items.AddRange(cluster.Cards);
                    else
                        items.Add(item);
                }
                sideboardListView.ClearObjects();
                sideboardListView.AddObjects(items);
            }
            else
            {
                Collection.CollapsedView = true;
                var clusterDictionary = new Dictionary<string, InventoryCardCluster>();
                InventoryTotalsItem totalsItem = null;
                foreach (var item in cardListView.Objects)
                {
                    if (item is InventoryCard card)
                    {
                        if (!clusterDictionary.ContainsKey(card.Name))
                            clusterDictionary.Add(card.Name, new InventoryCardCluster(card));
                        else
                            clusterDictionary[card.Name].Cards.Add(card);
                    }
                    else if (item is InventoryTotalsItem totals)
                    {
                        totalsItem = totals;
                    }
                }
                cardListView.ClearObjects();
                cardListView.AddObject(totalsItem);
                cardListView.AddObjects(clusterDictionary.Values.ToList());

                clusterDictionary.Clear();
                foreach (var item in sideboardListView.Objects)
                {
                    if (item is InventoryCard card)
                    {
                        if (!clusterDictionary.ContainsKey(card.Name))
                            clusterDictionary.Add(card.Name, new InventoryCardCluster(card));
                        else
                            clusterDictionary[card.Name].Cards.Add(card);
                    }
                    else if (item is InventoryTotalsItem totals)
                    {
                        totalsItem = totals;
                    }
                }
                sideboardListView.ClearObjects();
                sideboardListView.AddObject(totalsItem);
                sideboardListView.AddObjects(clusterDictionary.Values.ToList());
            }
            using (var context = new ScryfallCardsDbContext())
            {
                context.Update(Collection);
                context.SaveChanges();
            }
        }
    }
}