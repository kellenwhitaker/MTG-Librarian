using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using BrightIdeasSoftware;
//todo: change highlighting in treeview
namespace MTG_Collection_Tracker
{
    public partial class DBViewForm : DockContent
    {
        private Dictionary<string, OLVSetItem> sets;
        private ModelFilter setNameFilter;

        public DBViewForm()
        {
            InitializeComponent();
            setNameFilter = new ModelFilter(x => (x as OLVSetItem).Name.ToUpper().Contains(setFilterBox.Text.ToUpper()));
            treeListView1.SmallImageList = Form1.SmallIconList;
            fastObjectListView1.SmallImageList = Form1.SmallIconList;
            whiteManaButton.ImageList = blueManaButton.ImageList = blackManaButton.ImageList = redManaButton.ImageList = greenManaButton.ImageList 
                                      = colorlessManaButton.ImageList = genericManaButton.ImageList = Form1.ManaIcons;
            whiteManaButton.ImageKey = "W";
            blueManaButton.ImageKey = "U";
            blackManaButton.ImageKey = "B";
            redManaButton.ImageKey = "R";           
            greenManaButton.ImageKey = "G";
            colorlessManaButton.ImageKey = "C";
            genericManaButton.ImageKey = "X";
            fastObjectListView1.UseFiltering = true;
        }

        public class CardActivatedEventArgs : EventArgs
        {
            public MCard MCard { get; set; }
        }

        public event EventHandler<CardActivatedEventArgs> CardActivated;

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
            if (whiteManaButton.Checked)
                combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("W"));
            if (blueManaButton.Checked)
                combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("U"));
            if (blackManaButton.Checked)
                combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("B"));
            if (redManaButton.Checked)
                combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("R"));
            if (greenManaButton.Checked)
                combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("G"));
            if (colorlessManaButton.Checked)
                combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("C"));
            if (genericManaButton.Checked)
                combinedFilter = combinedFilter.And(x => (x as OLVCardItem).Cost.Contains("X") || (x as OLVCardItem).Cost.Any(c => char.IsDigit(c)));
            return combinedFilter;
        }

        private Predicate<object> GetTreeViewFilter()
        {
            Predicate<object> combinedFilter = x => true;
            if (treeListView1.SelectedIndex != -1)
            {
                var lvItem = treeListView1.SelectedItem.RowObject as OLVItem;
                do
                {
                    combinedFilter = combinedFilter.And(lvItem.Filter);
                } while ((lvItem = lvItem.Parent) != null);
            }
            return combinedFilter;
        }
        
        public class AlphaNumericString : IComparable<AlphaNumericString>
        {            
            private Int32? _intvalue;
            public bool HasValue => _intvalue.HasValue;
            public int Value => _intvalue.Value;
            public string String { get; private set; }

            public AlphaNumericString(string str)
            {
                String = str;
                if (Int32.TryParse(str, out int value))
                    _intvalue = value;
            }

            public int CompareTo(AlphaNumericString other)
            {
                if (HasValue && other.HasValue)
                    return Value.CompareTo(other.Value);
                else
                    return String.CompareTo(other.String);
            }
        }

        public void LoadSets()
        {
            treeListView1.CanExpandGetter = x => x is OLVSetItem;
            treeListView1.ChildrenGetter = x => ((OLVSetItem)x).Rarities;
            var renderer = treeListView1.TreeColumnRenderer;
            renderer.IsShowLines = false;
            renderer.UseTriangles = true;
            sets = new Dictionary<string, OLVSetItem>();
            using (var context = new MyDbContext())
            {
                var cards = from c in context.catalog
                            orderby c.Edition, new AlphaNumericString(c.ColNumber)
                            select c;
                
                foreach (MCard card in cards)
                {
                    if (!sets.TryGetValue(card.Edition, out OLVSetItem set))
                    {
                        set = new OLVSetItem(card.Edition);
                        sets.Add(card.Edition, set);
                    }
                    set.AddCard(card);                    
                }
            }
        }

        public void LoadTree()
        {
            foreach (var set in sets.Values)
            {
                treeListView1.AddObject(set);
                fastObjectListView1.AddObjects(set.Cards);
            }
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
            if (treeListView1.SelectedObject is OLVSetItem)
            {
                var item = treeListView1.SelectedObject as OLVSetItem;
                if (!item.Expanded)
                {
                    item.Expanded = true;
                    treeListView1.Expand(item);
                }
                else
                {
                    item.Expanded = false;
                    treeListView1.Collapse(item);
                }
            }
        }

        private void setFilterBox_TextChanged(object sender, EventArgs e)
        {
            if (setFilterBox.Text != "")
            {
                treeListView1.ModelFilter = setNameFilter;
                treeListView1.UseFiltering = true;
            }
            else
                treeListView1.UseFiltering = false;
        }

        private void treeListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fastObjectListView1.ModelFilter = new ModelFilter(GetCardFilter());
        }

        private void whiteManaButton_Click(object sender, EventArgs e)
        {
            fastObjectListView1.ModelFilter = new ModelFilter(GetCardFilter());
        }

        private void fastObjectListView1_ItemActivate(object sender, EventArgs e)
        {
            OnCardActivated(new CardActivatedEventArgs { MCard = fastObjectListView1.SelectedObject as MCard });
        }
    }   
}
