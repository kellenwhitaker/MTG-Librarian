﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using BrightIdeasSoftware;

namespace MTG_Collection_Tracker
{
    public partial class DBViewForm : DockContent
    {
        private Dictionary<string, OLVSetItem> sets;
        private ModelFilter setNameFilter;

        public DBViewForm()
        {
            InitializeComponent();
            setNameFilter = new ModelFilter(x => !(x is OLVSetItem) || (x is OLVSetItem && (x as OLVSetItem).Name.ToUpper().Contains(setFilterBox.Text.ToUpper())));
            setListView.SmallImageList = MainForm.SmallIconList;
            setListView.TreeColumnRenderer = new SetRenderer();
            fastObjectListView1.SmallImageList = MainForm.SmallIconList;
            whiteManaButton.ImageList = blueManaButton.ImageList = blackManaButton.ImageList = redManaButton.ImageList = greenManaButton.ImageList 
                                      = colorlessManaButton.ImageList = genericManaButton.ImageList = MainForm.ManaIcons;
            (whiteManaButton.ImageKey, blueManaButton.ImageKey, blackManaButton.ImageKey, redManaButton.ImageKey, greenManaButton.ImageKey)
            = ("{W}",                  "{U}",                   "{B}",                    "{R}",                  "{G}");
            (colorlessManaButton.ImageKey, genericManaButton.ImageKey) = ("{C}", "{X}");
            fastObjectListView1.UseFiltering = true;
            fastObjectListView1.SetDoubleBuffered();
            fastObjectListView1.GetColumn(2).Renderer = new ManaCostRenderer();
        }

        internal event EventHandler<CardActivatedEventArgs> CardActivated;
        private void OnCardActivated(CardActivatedEventArgs args)
        {
            CardActivated?.Invoke(this, args);
        }

        private Predicate<object> GetCardFilter()
        {
            return GetManaCostFilter().And(GetTreeViewFilter());
        }

        private Predicate<object> GetManaCostFilter()
        {
            Predicate<object> combinedFilter = x => true;
            if (whiteManaButton.Checked)    combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("W"));
            if (blueManaButton.Checked)     combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("U"));
            if (blackManaButton.Checked)    combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("B"));
            if (redManaButton.Checked)      combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("R"));
            if (greenManaButton.Checked)    combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("G"));
            if (colorlessManaButton.Checked)combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("C"));
            if (genericManaButton.Checked)  combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("X") || (x as OLVCardItem).Cost.Any(c => char.IsDigit(c)));
            return combinedFilter;
        }

        private Predicate<object> GetTreeViewFilter()
        {
            Predicate<object> combinedFilter = x => true;
            if (setListView.SelectedIndex != -1)
            {
                var lvItem = setListView.SelectedItem.RowObject as OLVItem;
                do
                {
                    combinedFilter = combinedFilter.And(lvItem.Filter);
                } while ((lvItem = lvItem.Parent) != null);
            }
            return combinedFilter;
        }

        internal void SortCardListView()
        {
            if (fastObjectListView1.PrimarySortColumn == null) // sort by set if not already sorted
            {
                fastObjectListView1.PrimarySortColumn = fastObjectListView1.AllColumns[3];
                fastObjectListView1.SecondarySortColumn = fastObjectListView1.AllColumns[4];
                fastObjectListView1.Sort();
            }
        }

        internal void LoadSet(string SetCode)
        {
            using (var context = new MyDbContext())
            {
                var dbSet = (from s in context.Sets
                            where s.Code == SetCode
                            select s).FirstOrDefault();
                if (dbSet != null)
                {
                    var set = new OLVSetItem(dbSet);
                    var cards = from c in context.Catalog
                                where c.SetCode == SetCode
                                orderby new AlphaNumericString(c.number), c.name
                                select c;
                    foreach (var card in cards)
                        set.AddCard(card);

                    setListView.AddObject(set);
                    fastObjectListView1.AddObjects(set.Cards);
                    if (setListView.Objects.Count() == 1) // first set added, must sort the tree
                        setListView.Sort(setListView.AllColumns[1], SortOrder.Descending);
                }
            }
        }

        internal void LoadSets()
        {
            setListView.CanExpandGetter = x => x is OLVSetItem;
            setListView.ChildrenGetter = x => (x as OLVSetItem).Rarities;
            var renderer = setListView.TreeColumnRenderer;
            renderer.IsShowLines = false;
            renderer.UseTriangles = true;
            sets = new Dictionary<string, OLVSetItem>();
            using (var context = new MyDbContext())
            {
                var dbSets = from s in context.Sets
                             orderby s.Name
                             select s;
                var cards = from c in context.Catalog
                            orderby c.Edition, new AlphaNumericString(c.number), c.name
                            select c;
                
                foreach (var card in cards)
                {
                    if (!sets.TryGetValue(card.Edition, out OLVSetItem set))
                    {
                        set = new OLVSetItem(dbSets.Where(x => x.Name == card.Edition).FirstOrDefault());
                        sets.Add(card.Edition, set);
                    }
                    set.AddCard(card);                    
                }
            }            
        }

        internal void LoadTree()
        {
            foreach (var set in sets.Values)
            {
                setListView.AddObject(set);
                fastObjectListView1.AddObjects(set.Cards);
            }
            setListView.Sort(setListView.AllColumns[1], SortOrder.Descending);
        }

        private void treeListView1_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.Model is OLVCardItem)
            {
                Rectangle padding = new Rectangle { X = -10 };
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

        private void setFilterBox_TextChanged(object sender, EventArgs e)
        {
            if (setFilterBox.Text != "")
            {
                setListView.ModelFilter = setNameFilter;
                setListView.UseFiltering = true;
            }
            else
                setListView.UseFiltering = false;
        }

        private void treeListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fastObjectListView1.SelectedObject = null;
            fastObjectListView1.ModelFilter = new ModelFilter(GetCardFilter());
        }

        private void whiteManaButton_Click(object sender, EventArgs e)
        {
            fastObjectListView1.ModelFilter = new ModelFilter(GetCardFilter());
        }

        private void fastObjectListView1_ItemActivate(object sender, EventArgs e)
        {
            OnCardActivated(new CardActivatedEventArgs { MagicCard = (fastObjectListView1.SelectedObject as OLVCardItem).MagicCard });
        }

        private void fastObjectListView1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Cursor.Current = new Cursor(Properties.Resources.hand.GetHicon());
        }

        internal event EventHandler<CardSelectedEventArgs> CardSelected;
        private void OnCardSelected(CardSelectedEventArgs args)
        {
            CardSelected?.Invoke(this, args);
        }

        internal event EventHandler<CardFocusedEventArgs> CardFocused;
        private void OnCardFocused(CardFocusedEventArgs e)
        {
            CardFocused?.Invoke(this, e);
        }

        private void fastObjectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MagicCard card = (fastObjectListView1.SelectedObject as OLVCardItem)?.MagicCard;
            if (card != null)
                OnCardSelected(new CardSelectedEventArgs { Edition = card.Edition, uuid = card.uuid, MultiverseId = card.multiverseId, Name = card.name });
        }
    }
}
