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

namespace MTG_Collection_Tracker
{
    public partial class CardNavigatorForm : DockContent
    {
        private List<NavigatorGroup> groupList;

        public CardNavigatorForm()
        {
            InitializeComponent();
            navigatorListView.CanExpandGetter = x => (x as NavigatorItem).CanExpand;
            navigatorListView.ChildrenGetter = x => ((NavigatorGroup)x).Collections;
            var renderer = navigatorListView.TreeColumnRenderer;
            renderer.IsShowLines = false;
            renderer.UseTriangles = true;
        }

        public void LoadGroups()
        {
            groupList = new List<NavigatorGroup>();
            using (var context = new MyDbContext())
            {
                var groups = from g in context.CollectionGroups
                             select g;
                foreach (CollectionGroup group in groups)
                    groupList.Add(new NavigatorGroup { Name = group.GroupName });
                var collections = from c in context.Collections
                                  select c;
                NavigatorGroup navGroup;
                foreach (var collection in collections)
                {
                    navGroup = groupList.Find(x => x.Name == collection.GroupName);
                    if (navGroup != null)
                        navGroup.Collections.Add(new NavigatorCollection { CardCollection = collection });
                }
            }
        }

        public void LoadTree()
        {
            navigatorListView.AddObjects(groupList);
        }

        public event EventHandler<CollectionActivatedEventArgs> CollectionActivated;

        private void OnCollectionActivated(CollectionActivatedEventArgs args)
        {
            CollectionActivated?.Invoke(this, args);
        }

        private void navigatorListView_ItemActivate(object sender, EventArgs e)
        {
            if (navigatorListView.SelectedObject == null)
                Console.WriteLine("null");
            else
                OnCollectionActivated(new CollectionActivatedEventArgs { NavigatorCollection = navigatorListView.SelectedObject as NavigatorCollection });
        }
    }

    public class CollectionActivatedEventArgs
    {
        public NavigatorCollection NavigatorCollection { get; set; }
    }

    public class NavigatorCollection : NavigatorItem
    {
        public CardCollection CardCollection { get; set; }
        public override string Name { get => CardCollection?.CollectionName; }
        public string Text => Name;
    }

    public class NavigatorGroup : NavigatorItem
    {
        public override bool CanExpand => Collections != null ? Collections.Count > 0 : false;
        public List<NavigatorCollection> Collections { get; }
        public NavigatorGroup()
        {
            Collections = new List<NavigatorCollection>();
        }

        public override string ToString()
        {
            int collectionCount = Collections != null ? Collections.Count : 0;
            return $"{Name} [{collectionCount}]";
        }

        public string Text => ToString();
    }

    public class NavigatorItem
    {
        public virtual bool CanExpand => false;
        public virtual string Name { get; set; }
    }
}
