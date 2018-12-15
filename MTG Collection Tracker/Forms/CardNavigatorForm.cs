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

namespace MTG_Librarian
{
    public partial class CardNavigatorForm : DockContent
    {
        private List<NavigatorGroup> groupList;

        public CardNavigatorForm()
        {
            InitializeComponent();
            navigatorListView.CanExpandGetter = x => (x as NavigatorItem).CanExpand;
            navigatorListView.ChildrenGetter = x => (x as NavigatorGroup).Collections;
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
                    groupList.Add(new NavigatorGroup { CollectionGroup = group });
                var collections = from c in context.Collections
                                  select c;
                NavigatorGroup navGroup;
                foreach (var collection in collections)
                {
                    navGroup = groupList.Find(x => x.Id == collection.GroupId);
                    if (navGroup != null)
                        navGroup.AddCollection(new NavigatorCollection { CardCollection = collection });
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
            if (navigatorListView.SelectedObject is NavigatorGroup group)
            {
                if (!navigatorListView.IsExpanded(group))
                    navigatorListView.Expand(group);
                else
                    navigatorListView.Collapse(group);
            }
            else if (navigatorListView.SelectedObject is NavigatorCollection collection)
                OnCollectionActivated(new CollectionActivatedEventArgs { NavigatorCollection = collection });
        }

        private void newGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newGroup = new NavigatorGroup { Name = "New Group" };
            navigatorListView.AddObject(newGroup);
            navigatorListView.SelectedObject = newGroup;
            navigatorListView.StartCellEdit(navigatorListView.SelectedItem, 0);
        }

        private void navigatorListView_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            e.Control.Text = (e.RowObject as NavigatorItem).Name;
        }

        private void navigatorListView_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.RowObject is NavigatorGroup group)
            {
                using (var context = new MyDbContext())
                {
                    try
                    {
                        if (group.CollectionGroup == null) // not yet added
                        {
                            group.CollectionGroup = new CollectionGroup { GroupName = e.Control.Text, Permanent = false };
                            context.CollectionGroups.Add(group.CollectionGroup);
                        }
                        else
                        {
                            group.CollectionGroup.GroupName = e.Control.Text;
                            context.CollectionGroups.Update(group.CollectionGroup);
                        }
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        if (group.CollectionGroup != null)
                            context.Entry(group.CollectionGroup).Reload();
                        MessageBox.Show("Could not add/edit group");
                    }
                }
            }
            else if (e.RowObject is NavigatorCollection collection)
            {
                using (var context = new MyDbContext())
                {
                    try
                    {
                        if (collection.CardCollection == null) // not yet added
                        {
                            collection.CardCollection = new CardCollection { GroupId = collection.GroupId, CollectionName = e.Control.Text, Permanent = false, Type = "collection" };
                            context.Collections.Add(collection.CardCollection);
                        }
                        else
                        {
                            collection.CardCollection.CollectionName = e.Control.Text;
                            context.Collections.Update(collection.CardCollection);
                        }
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        if (collection.CardCollection != null)
                            context.Entry(collection.CardCollection).Reload();
                        MessageBox.Show("Could not add/edit collection");
                    }
                }
            }
        }

        private void navigatorListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (navigatorListView.SelectedObject is NavigatorItem navigatorItem)
            {
                if (!navigatorItem.Permanent)
                    editNameToolStripMenuItem.Visible = deleteToolStripMenuItem.Visible = true;
                else
                    editNameToolStripMenuItem.Visible = deleteToolStripMenuItem.Visible = false;
                newCollectionToolStripMenuItem.Visible = true;
                newGroupToolStripMenuItem.Visible = false;
            }
            else
            {
                newCollectionToolStripMenuItem.Visible = false;
                newGroupToolStripMenuItem.Visible = true;
                deleteToolStripMenuItem.Visible = false;
                editNameToolStripMenuItem.Visible = false;
            }
        }

        private void editNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            navigatorListView.StartCellEdit(navigatorListView.SelectedItem, 0);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (navigatorListView.SelectedObject is NavigatorItem navigatorItem)
            {
                bool isGroup = navigatorItem is NavigatorGroup;
                if (MessageBox.Show($"Are you sure you want do delete the {(isGroup ? "group " : "collection ")}{navigatorItem.Name}?{(isGroup ? "\nDoing so will also delete any underlying collections." : "")}", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (var context = new MyDbContext())
                        {
                            context.Remove(navigatorItem.Entity);
                            context.SaveChanges();
                            if (isGroup)
                                navigatorListView.RemoveObject(navigatorItem);
                            else if (navigatorItem is NavigatorCollection collection)
                            {
                                collection.Parent?.RemoveCollection(collection);
                                navigatorListView.RemoveObject(collection);
                            }
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
            }
        }

        private void newCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigatorGroup group = null;
            if (navigatorListView.SelectedObject is NavigatorGroup)
                group = navigatorListView.SelectedObject as NavigatorGroup;
            else if (navigatorListView.SelectedObject is NavigatorCollection)
                group = (navigatorListView.SelectedObject as NavigatorCollection).Parent;
            if (group != null)
            {
                var newCollection = new NavigatorCollection { Name = "New Collection", GroupId = group.Id };
                group.AddCollection(newCollection);
                navigatorListView.RebuildAll(true);
                navigatorListView.Expand(group);
                navigatorListView.SelectedObject = newCollection;
                navigatorListView.StartCellEdit(navigatorListView.SelectedItem, 0);
            }
        }

        private void navigatorListView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data is BrightIdeasSoftware.OLVDataObject data && data.ModelObjects.Count == 1 && data.ModelObjects[0] is NavigatorCollection collection && !collection.Permanent)
            {
                Point client = navigatorListView.PointToClient(new Point(e.X, e.Y));
                if (navigatorListView.OlvHitTest(client.X, client.Y).RowObject is NavigatorGroup group)
                {
                    navigatorListView.SelectedObject = group;
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    navigatorListView.SelectedObject = null;
                    e.Effect = DragDropEffects.Move;
                }
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void navigatorListView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data is BrightIdeasSoftware.OLVDataObject data && data.ModelObjects.Count == 1 && data.ModelObjects[0] is NavigatorCollection collection && !collection.Permanent)
            {
                Point client = navigatorListView.PointToClient(new Point(e.X, e.Y));
                if (navigatorListView.OlvHitTest(client.X, client.Y).RowObject is NavigatorGroup group)
                {
                    try
                    {
                        using (var context = new MyDbContext())
                        {
                            collection.CardCollection.GroupId = group.Id;
                            context.Update(collection.CardCollection);
                            context.SaveChanges();
                            collection.RemoveFromParent();
                            group.AddCollection(collection);
                            navigatorListView.RebuildAll(true);
                        }
                    }
                    catch (Exception ex) { }
                }
            }
        }
    }

    public class CollectionActivatedEventArgs
    {
        public NavigatorCollection NavigatorCollection { get; set; }
    }

    public class NavigatorCollection : NavigatorItem
    {
        public override object Entity => CardCollection;
        public override bool Permanent => CardCollection?.Permanent ?? false;
        public override int Id => CardCollection?.Id ?? -1;
        public int GroupId { get; set; }
        public CardCollection CardCollection { get; set; }
        private string _name;
        public override string Name
        {
            get => CardCollection?.CollectionName ?? _name;
            set => _name = value;
        }
        public string Text => Name;
        public NavigatorGroup Parent { get; set; }
        public void RemoveFromParent()
        {
            Parent?.RemoveCollection(this);
        }
    }

    public class NavigatorGroup : NavigatorItem
    {
        public override object Entity => CollectionGroup;
        public override bool Permanent => CollectionGroup?.Permanent ?? false;
        public override int Id => CollectionGroup?.Id ?? -1; 
        public CollectionGroup CollectionGroup { get; set; }
        public override bool CanExpand => Collections != null ? Collections.Count > 0 : false;
        public List<NavigatorCollection> Collections { get; }
        private string _name;
        public override string Name
        {
            get => CollectionGroup?.GroupName ?? _name;
            set => _name = value;
        }

        public void RemoveCollection(NavigatorCollection collection)
        {
            collection.Parent = null;
            Collections.Remove(collection);
        }

        public void AddCollection(NavigatorCollection collection)
        {
            collection.Parent = this;
            Collections.Add(collection);
        }

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
        public virtual object Entity { get; }
        public virtual int Id { get; }
        public virtual bool Permanent { get; }
        public virtual bool CanExpand => false;
        public virtual string Name { get; set; }
    }
}
