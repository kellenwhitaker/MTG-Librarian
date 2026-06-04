using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KW.WinFormsUI.Docking;
using BrightIdeasSoftware;
using System.Collections;

namespace MTG_Librarian
{
    public partial class DBViewForm : DockForm
    {
        #region Fields

        private Dictionary<string, OLVSetItem> sets;
        public List<OLVSetItem> SetItems = new List<OLVSetItem>();
        public bool SearchHasMoreResults = false;
        object SetSelected = null;
        public bool addingToCLV = false;

        #endregion Fields

        #region Constructors

        public DBViewForm()
        {
            InitializeComponent();
            setListView.SmallImageList = Globals.ImageLists.SmallIconList;
            setListView.TreeColumnRenderer = new SetRenderer();
            setListView.UseFiltering = true;
            cardListView.SmallImageList = Globals.ImageLists.SmallIconList;
            cardListView.VirtualListDataSource = new MyCustomSortingDataSource(cardListView);
            whiteManaButton.ImageList = blueManaButton.ImageList = blackManaButton.ImageList = redManaButton.ImageList = greenManaButton.ImageList
                                      = colorlessManaButton.ImageList = genericManaButton.ImageList = Globals.ImageLists.ManaIcons;
            (whiteManaButton.ImageKey, blueManaButton.ImageKey) = ("{W}", "{U}");
            (blackManaButton.ImageKey, redManaButton.ImageKey, greenManaButton.ImageKey) = ("{B}", "{R}", "{G}");
            (colorlessManaButton.ImageKey, genericManaButton.ImageKey) = ("{C}", "{X}");
            cardListView.UseFiltering = true;
            cardListView.SetDoubleBuffered();
            cardListView.AllColumns.FirstOrDefault(x => x.AspectName == "ManaCost").Renderer = new ManaCostRenderer();
            formatFilterComboBox.SelectedIndex = 0;
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockBottom;
        }

        #endregion Constructors

        #region Methods

        private string BuildScryfallQuery()
        {
            string query = "";
            string manaSymbols = "";
            string amp = "+";
            if (cardListView.LastSortColumn != null)
            {
                switch (cardListView.LastSortColumn.AspectName)
                {
                    case "DisplayName": query += "order=name"; break;
                    case "ManaCost": query += "order=cmc"; break;
                    case "Set": query += "order=set"; break;
                    case "Price": query += $"order={SettingsManager.ApplicationSettings.DefaultCurrency.ToLower()}"; break;
                    default: break;
                }
            }
            if (query != "")
            {
                if (cardListView.LastSortOrder == SortOrder.Ascending)
                    query += "&dir=asc&";
                else if (cardListView.LastSortOrder == SortOrder.Descending)
                    query += "&dir=desc&";
            }
            query += "include_variations=true&unique=prints&q=";
            if (whiteManaButton.Checked) manaSymbols += "W";
            if (blueManaButton.Checked) manaSymbols += "U";
            if (blackManaButton.Checked) manaSymbols += "B";
            if (redManaButton.Checked) manaSymbols += "R";
            if (greenManaButton.Checked) manaSymbols += "G";
            if (colorlessManaButton.Checked) manaSymbols += "C";
            if (manaSymbols != "")
                query += $"(m:{manaSymbols})";
            if (cardNameFilterBox.UserText != "")
                query += $"{(query.Length > 0 ? amp : "")}name%3A{cardNameFilterBox.UserText}";
            if (cardTextFilterTextBox.UserText != "")
                query += $"{(query.Length > 0 ? amp : "")}oracle%3A{cardTextFilterTextBox.UserText}";
            if (typeFilterTextBox.UserText != "")
                query += $"{(query.Length > 0 ? amp : "")}type:{typeFilterTextBox.UserText}";
            if (setListView.SelectedObject != null)
            {
                string setCode = (setListView.SelectedObject as OLVSetItem).CardSet.code;
                query += $"{(query.Length > 0 ? amp : "")}set%3A{setCode}";
            }
            if (formatFilterComboBox.SelectedIndex > 0)
                query += $"{(query.Length > 0 ? amp : "")}(legal:{formatFilterComboBox.SelectedItem.ToString()}+or+restricted:{formatFilterComboBox.SelectedItem.ToString()})";

            return query;
        }
        private Predicate<object> GetSetNameTreeFilter()
        {
            string boxText = setFilterBox.UserText.ToUpper();
            return x => boxText == ""
                ? true
                : (x is OLVRarityItem rarityItem && (rarityItem.Parent as OLVSetItem).Name.ToUpper().Contains(boxText))
                  || (x is OLVSetItem && (x as OLVSetItem).Name.ToUpper().Contains(boxText));
        }
        public void SortCardListView()
        {
            if (cardListView.PrimarySortColumn == null) // sort by set, number if not already sorted
            {
                var sorted = cardListView.Objects.Cast<OLVCardItem>().OrderBy(x => x.MagicCard.set_name).ThenBy(x => x.MagicCard.SortableNumber);
                cardListView.Objects = sorted;
            }
            else
                cardListView.Sort();
        }

        public void LoadSet(string SetCode)
        {
            var existingSet = setListView.Objects.Cast<OLVSetItem>().FirstOrDefault(x => x.CardSet.code == SetCode);
            var selectedSet = setListView.SelectedObject as OLVSetItem;
            if (existingSet != null)
            {
                var existingCards = cardListView.Objects.Cast<OLVCardItem>().Where(x => x.MagicCard.set == SetCode).ToArray();
                foreach (var card in existingCards)
                    Globals.Collections.MagicCardCache.Remove(card.MagicCard.ScryfallId);
                setListView.RemoveObject(existingSet);
                cardListView.RemoveObjects(existingCards);
            }
            try
            {
                using (var context = new ScryfallCardsDbContext())
                {
                    var dbSet = (from s in context.Sets
                                 where s.code == SetCode
                                 select s).FirstOrDefault();
                    if (dbSet != null)
                    {
                        var set = new OLVSetItem(dbSet);
                        var cards = from c in context.Catalog
                                    where c.set == SetCode
                                    orderby new AlphaNumericString(c.collector_number), c.Name
                                    select c;

                        foreach (var card in cards)
                        {
                            var invItems = from i in context.LibraryView
                                           where i.set == card.set && i.collector_number == card.collector_number
                                           select i;

                            if (invItems.FirstOrDefault() != null)
                            {
                                int count = 0;
                                foreach (var item in invItems)
                                    if (!item.Virtual && item.Count.HasValue)
                                        count += item.Count.Value;
                                card.CopiesOwned = count;
                                set.AddCard(card);
                            }
                        }

                        setListView.AddObject(set);
                        if (setListView.Objects.Count() == 1) // first set added, must sort the tree
                            setListView.Sort(setListView.AllColumns[1], SortOrder.Descending);
                        SetItems.Add(set);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
            }
            if (selectedSet != null)
                setListView.SelectedObject = setListView.Objects.Cast<OLVSetItem>().FirstOrDefault(x => x.CardSet.code == selectedSet.CardSet.code);
        }
        // TODO: refactor
        public delegate void LoadSetsDelegate();
        public void LoadSets()
        {
            if (InvokeRequired)
                BeginInvoke(new LoadSetsDelegate(LoadSets), null);
            else
            {
                setListView.CanExpandGetter = x => x is OLVSetItem;
                setListView.ChildrenGetter = x => (x as OLVSetItem).Rarities;
                var renderer = setListView.TreeColumnRenderer;
                renderer.IsShowLines = false;
                renderer.UseTriangles = true;
                sets = new Dictionary<string, OLVSetItem>();
                try
                {
                    using (var context = new ScryfallCardsDbContext())
                    {
                        var dbSets = from s in context.Sets
                                     orderby s.name
                                     select s;

                        OLVSetItem set;
                        foreach (var dbSet in dbSets)
                        {
                            set = new OLVSetItem(dbSet);
                            var cards = from c in context.Catalog
                                        where c.set == dbSet.code
                                        orderby new AlphaNumericString(c.collector_number), c.Name
                                        select c;

                            foreach (var card in cards)
                            {
                                var invItems = from i in context.LibraryView
                                               where i.set == card.set && i.collector_number == card.collector_number
                                               select i;

                                if (invItems.FirstOrDefault() != null)
                                {
                                    int count = 0;
                                    foreach (var item in invItems)
                                        if (!item.Virtual && item.Count.HasValue)
                                            count += item.Count.Value;
                                    card.CopiesOwned = count;
                                    set.AddCard(card);
                                }
                            }

                            setListView.AddObject(set);
                            //if (setListView.Objects.Count() == 1) // first set added, must sort the tree
                            //setListView.Sort(setListView.AllColumns[1], SortOrder.Descending);
                            SetItems.Add(set);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugOutput.WriteLine(ex.ToString());
                }

                SetItems.AddRange(sets.Values);
            }
        }
        public void LoadTree()
        {
            foreach (var set in sets.Values)
                setListView.AddObject(set);

            setListView.Sort(setListView.AllColumns[1], SortOrder.Descending);
        }
        private void UpdateSetModelFilter()
        {
            setListView.ModelFilter = new ModelFilter(GetSetNameTreeFilter());
        }

        #endregion Methods

        #region Events

        #region DBViewForm Events

        public event EventHandler<CardsActivatedEventArgs> CardsActivated;

        private void OnCardsActivated(CardsActivatedEventArgs args)
        {
            CardsActivated?.Invoke(this, args);
        }

        public event EventHandler<CardSelectedEventArgs> CardSelected;

        private void OnCardSelected(CardSelectedEventArgs args)
        {
            CardSelected?.Invoke(this, args);
        }

        #endregion DBViewForm Events

        #region setListView Events

        private void treeListView1_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.Model is OLVCardItem)
            {
                var padding = new Rectangle { X = -10 };
                e.Item.CellPadding = padding;
            }
        }

        private void treeListView1_ItemActivate(object sender, EventArgs e)
        {
            if (setListView.SelectedObject is OLVSetItem item)
            {
                if (!setListView.IsExpanded(item))
                    setListView.Expand(item);
                else
                    setListView.Collapse(item);
            }
        }

        private void setListView_SelectionChanged(object sender, EventArgs e)
        {
            if (setListView.SelectedObject != SetSelected)
            {
                SetSelected = setListView.SelectedObject;
                cardListView.SelectedObject = null;
                DoScryfallQuery();
            }
        }

        #endregion setListView Events

        #region cardListView Events

        private void fastObjectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cards = cardListView.SelectedObjects;
            if (cards.Count > 0)
                OnCardSelected(new CardSelectedEventArgs { MagicCards = cards });
        }

        private void fastObjectListView1_ItemActivate(object sender, EventArgs e)
        {
            OnCardsActivated(new CardsActivatedEventArgs { CardItems = new ArrayList { cardListView.SelectedObject as OLVCardItem } });
        }

        private void fastObjectListView1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Cursor.Current = new Cursor(Properties.Resources.hand.GetHicon());
        }

        private void cardListView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cardListView.SelectedItems != null)
                if (e.KeyChar == '\r')
                    e.Handled = true;
        }
        private void cardListView_Scroll(object sender, ScrollEventArgs e)
        {
            if (cardListView.Items[cardListView.Items.Count - 1].Bounds.Top < 500)
            {
                FetchMoreResults();
            }
        }
        #endregion cardListView Events

        #region Menu Events

        private void updateThisSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (setListView.SelectedObject is OLVSetItem setItem)
            {
                var newTask = new DownloadSetTask(setItem.CardSet);
                Globals.Forms.TasksForm.TaskManager.AddTask(newTask);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!(setListView.SelectedObject is OLVSetItem))
                e.Cancel = true;
        }

        #endregion Menu Events

        #region Misc Events

        private void setFilterBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSetModelFilter();
        }

        private void whiteManaButton_Click(object sender, EventArgs e)
        {
            DoScryfallQuery();
        }
        private void cardNameFilterBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                DoScryfallQuery();
            }
        }
        private void formatFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoScryfallQuery();
        }
        public void InventoryChanged(object sender, InventoryChangedEventArgs e)
        {
            using (var context = new ScryfallCardsDbContext())
            {
                var setsNeedingRecount = new List<string>();
                foreach (var card in e.Cards)
                {
                    var scryfallCard = card.ToScryfallMagicCard();
                    var invCards = from i in context.LibraryView
                                   where i.ScryfallId == scryfallCard.ScryfallId
                                   select i;
                    int count = 0;
                    foreach (var inv in invCards)
                    {
                        if (inv.Count.HasValue)
                            count += inv.Count.Value;
                    }
                    foreach (OLVCardItem lvCard in cardListView.Objects)
                        if (lvCard.MagicCard.ScryfallId == card.ScryfallId)
                        {
                            lvCard.MagicCard.CopiesOwned = count;
                            cardListView.RefreshObject(lvCard);
                        }
                    if (!setsNeedingRecount.Contains(scryfallCard.set))
                        setsNeedingRecount.Add(scryfallCard.set);
                }
                foreach (var set in setsNeedingRecount)
                {
                    OLVSetItem olvSet = null;
                    foreach (OLVSetItem item in setListView.Objects)
                        if (item.CardSet.code == set)
                        {
                            olvSet = item;
                            break;
                        }
                    if (olvSet != null)
                    {
                        olvSet.Cards.Clear();
                        var cards = from c in context.Catalog
                                    where c.set == set
                                    orderby new AlphaNumericString(c.collector_number), c.Name
                                    select c;

                        foreach (var card in cards)
                        {
                            var invItems = from i in context.LibraryView
                                           where i.set == card.set && i.collector_number == card.collector_number
                                           select i;

                            if (invItems.FirstOrDefault() != null)
                            {
                                int count = 0;
                                foreach (var item in invItems)
                                    if (!item.Virtual && item.Count.HasValue)
                                        count += item.Count.Value;
                                card.CopiesOwned = count;
                                olvSet.AddCard(card);
                            }
                        }
                        setListView.RefreshObject(olvSet);
                    }
                }
            }
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
                if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "CollectorNumber"))
                    FilteredObjectList.Sort(new CollectorNumberComparer { SortOrder = order });
                else if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "DisplayName"))
                    FilteredObjectList.Sort(new NameComparer { SortOrder = order });
                else if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "Type"))
                    FilteredObjectList.Sort(new TypeComparer { SortOrder = order });
                else if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "Set"))
                    FilteredObjectList.Sort(new SetComparer { SortOrder = order });
                else if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "ManaCost"))
                    FilteredObjectList.Sort(new ManaCostComparer { SortOrder = order });
                else if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "CopiesOwned"))
                    FilteredObjectList.Sort(new CopiesOwnedComparer { SortOrder = order });
                RebuildIndexMap();
            }

            private class CopiesOwnedComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result = (x as OLVCardItem).CopiesOwned.CompareTo((y as OLVCardItem).CopiesOwned);
                    return SortOrder == SortOrder.Ascending ? result : -1 * result;
                }
            }

            private class CollectorNumberComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result = (x as OLVCardItem).SortableNumber.CompareTo((y as OLVCardItem).SortableNumber);
                    return SortOrder == SortOrder.Ascending ? result : -1 * result;
                }
            }

            private class NameComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result = (x as OLVCardItem).Name.CompareTo((y as OLVCardItem).Name);
                    return SortOrder == SortOrder.Ascending ? result : result * -1;
                }
            }

            private class TypeComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result = (x as OLVCardItem).Type.CompareTo((y as OLVCardItem).Type);
                    return SortOrder == SortOrder.Ascending ? result : result * -1;
                }
            }

            private class SetComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result = (x as OLVCardItem).Set.CompareTo((y as OLVCardItem).Set);
                    return SortOrder == SortOrder.Ascending ? result : result * -1;
                }
            }

            private class ManaCostComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result;
                    string valueX = (x as OLVCardItem).ManaCost ?? "";
                    string valueY = (y as OLVCardItem).ManaCost ?? "";
                    result = valueX.CompareTo(valueY);
                    return SortOrder == SortOrder.Ascending ? result : result * -1;
                }
            }
        }

        #endregion Classes
        
        private void FetchMoreResults()
        {
            Globals.Forms.TasksForm.TaskManager.ContinueWaitingTask();
        }
        private void DoScryfallQuery()
        {
            string query = BuildScryfallQuery();
            if (query != "" && Globals.Forms.TasksForm != null)
            {
                cardListView.ClearObjects();
                cardListView.EmptyListMsg = "Performing query...";
                var newTask = new ScryfallSearchTask(query);
                Globals.Forms.TasksForm.TaskManager.AddTask(newTask);
            }
        }

        private void cardListView_BeforeSorting(object sender, BeforeSortingEventArgs e)
        {
            e.Handled = true;
            if (!addingToCLV)
            {
                if (e.ColumnToSort.AspectName == "DisplayName" || e.ColumnToSort.AspectName == "ManaCost" || e.ColumnToSort.AspectName == "Set" || e.ColumnToSort.AspectName == "Price")
                {
                    cardListView.LastSortColumn = e.ColumnToSort;
                    cardListView.LastSortOrder = e.SortOrder;
                    DoScryfallQuery();
                }
                else
                    e.Canceled = true;
            }
        }
    }
}