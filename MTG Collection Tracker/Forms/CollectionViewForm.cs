using BrightIdeasSoftware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
//TODO: improve appearance of checkboxes
//TODO: enable deletion of cards from collection
//TODO: enable dropping of whole sets (from the set tree view) onto a collection
//TODO: moving a collection view out of the panel breaks any updates to it
namespace MTG_Collection_Tracker
{
    public partial class CollectionViewForm : DockContent
    {
        public string DocumentName => Collection?.CollectionName;
        public CardCollection Collection { get; set; }
        private bool MultiEditing = false;

        public CollectionViewForm()
        {
            InitializeComponent();
            //FoilColumn.CellPadding = new Rectangle(8, 0, 0, 0);
            cardListView.SetDoubleBuffered();
            cardListView.GetColumn("Card").Renderer = new CardInstanceNameRenderer();
            cardListView.GetColumn("Mana Cost").Renderer = new ManaCostRenderer();
            cardListView.AddDecoration(new EditingCellBorderDecoration { UseLightbox = false, BorderPen = new Pen(Brushes.DodgerBlue, 3), BoundsPadding = new Size(1, 0) });
            var billboard = (cardListView.DropSink as SimpleDropSink).Billboard;
            billboard.BackColor = Color.DodgerBlue;
            billboard.TextColor = Color.White;
        }

        public void LoadCollection()
        {
            if (Collection != null)
            {
                using (MyDbContext context = new MyDbContext())
                {
                    var items = from c in context.LibraryView
                                where c.CollectionId == Collection.Id
                                select c;

                    var cardList = items.ToList();
                    cardListView.AddObjects(items.ToList());
                }
                if (cardListView.PrimarySortColumn == null) // not yet sorted
                    cardListView.Sort(cardListView.AllColumns[11], SortOrder.Ascending);
            }
        }

        internal void AddCardInstance(FullInventoryCard cardInstance)
        {
            cardListView.AddObject(cardInstance);
        }

        private void fastObjectListView1_ModelCanDrop(object sender, ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            e.InfoMessage = $"Add to {DocumentName}";
        }

        private void fastObjectListView1_ModelDropped(object sender, ModelDropEventArgs e)
        {
            
            OnCardsDropped(new CardsDroppedEventArgs { Items = e.SourceModels as ArrayList });
        }

        internal event EventHandler<CardsDroppedEventArgs> CardsDropped;
        private void OnCardsDropped(CardsDroppedEventArgs args)
        {
            CardsDropped?.Invoke(this, args);
        }

        private void fastObjectListView1_CellEditFinished(object sender, CellEditEventArgs e)
        {
            if (e.RowObject is FullInventoryCard rowObject)
            {
                var args = new CardsUpdatedEventArgs { Items = new ArrayList { rowObject } };
                var rowItem = cardListView.ModelToItem(rowObject);
                if (e.Column.AspectName == "Count" && rowObject.Count < 1)
                    rowObject.Count = 1;

                if (MultiEditing)
                {
                    foreach (var row in cardListView.SelectedObjects)
                    {
                        if (row != rowObject)
                        {
                            if (e.Column.AspectName == "Tags")
                                (row as FullInventoryCard).Tags = rowObject.Tags;
                            else if (e.Column.AspectName == "Count")
                                (row as FullInventoryCard).Count = rowObject.Count;
                            else if (e.Column.AspectName == "Cost")
                                (row as FullInventoryCard).Cost = rowObject.Cost;
                            args.Items.Add(row);
                        }
                    }
                    MultiEditing = false;
                }
                OnCardsUpdated(args);
            }
        }

        internal event EventHandler<CardsUpdatedEventArgs> CardsUpdated;
        private void OnCardsUpdated(CardsUpdatedEventArgs args)
        {
            CardsUpdated?.Invoke(this, args);
        }

        internal event EventHandler<CardSelectedEventArgs> CardSelected;
        private void OnCardSelected(CardSelectedEventArgs args)
        {
            CardSelected?.Invoke(this, args);
        }

        private void fastObjectListView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cardListView.SelectedItems != null)
            {
                if (e.KeyChar == '=' || e.KeyChar == '+')
                {
                    e.Handled = true;
                    foreach (FullInventoryCard item in cardListView.SelectedObjects)
                        item.Count++;
                    OnCardsUpdated(new CardsUpdatedEventArgs { Items = cardListView.SelectedObjects as ArrayList });
                }
                else if (e.KeyChar == '-' || e.KeyChar == '_')
                {
                    e.Handled = true;
                    foreach (FullInventoryCard item in cardListView.SelectedObjects)
                    {
                        if (item.Count > 1)
                            item.Count--;
                    }
                    OnCardsUpdated(new CardsUpdatedEventArgs { Items = cardListView.SelectedObjects as ArrayList });
                }
            }
        }

        private void fastObjectListView1_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ClickCount == 2 && e.ModifierKeys == Keys.Shift && cardListView.SelectedObjects.Count > 1)
            {
                MultiEditing = true;
                e.ListView.StartCellEdit(e.Item, e.Item.SubItems.IndexOf(e.SubItem));
                e.Handled = true;
            }
        }

        private void fastObjectListView1_SelectionChanged(object sender, EventArgs e)
        {
            if (cardListView.SelectedObject != null)
            {
                var card = cardListView.SelectedObject as MagicCardBase;
                OnCardSelected(new CardSelectedEventArgs { MagicCard = card });
            }
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
    }
}
