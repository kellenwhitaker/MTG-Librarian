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
//TODO poor performance on fastobjectlistview
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

                    cardListView.AddObjects(items.ToList());
                }
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
            var args = new CardsUpdatedEventArgs { Items = new ArrayList { e.RowObject } };
            
            var rowItem = cardListView.ModelToItem(e.RowObject);
            if (MultiEditing)
            {
                foreach (var row in cardListView.SelectedObjects)
                {
                    if (row != e.RowObject)
                    {
                        Console.WriteLine(e.Column.AspectName);
                        if (e.Column.AspectName == "Tags")
                            (row as FullInventoryCard).Tags = (e.RowObject as FullInventoryCard).Tags;
                        else if (e.Column.AspectName == "Count")
                            (row as FullInventoryCard).Count = (e.RowObject as FullInventoryCard).Count;
                        else if (e.Column.AspectName == "Cost")
                            (row as FullInventoryCard).Cost = (e.RowObject as FullInventoryCard).Cost;
                        args.Items.Add(row);
                    }
                }
                MultiEditing = false;
            }
            OnCardsUpdated(args);
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
                        item.Count--;
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
                var card = cardListView.SelectedObject as FullInventoryCard;
                OnCardSelected(new CardSelectedEventArgs { uuid = card.uuid, Edition = card.Edition, MultiverseId = card.multiverseId, Name = card.name });
            }
        }
    }
}
