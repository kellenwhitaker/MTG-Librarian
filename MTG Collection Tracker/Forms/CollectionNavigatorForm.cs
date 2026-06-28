using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using KW.WinFormsUI.Docking;
using MTG_Librarian.Forms;

namespace MTG_Librarian
{
    public partial class CollectionNavigatorForm : DockForm
    {
        #region Fields

        private List<NavigatorGroup> groupList;

        #endregion Fields

        #region Constructors

        public CollectionNavigatorForm()
        {
            InitializeComponent();
            navigatorListView.CanExpandGetter = x => (x as NavigatorItem).CanExpand;
            navigatorListView.ChildrenGetter = x => (x as NavigatorGroup).Collections;
            navigatorListView.TreeColumnRenderer = new CollectionNameRenderer();
            var dropSink = navigatorListView.DropSink as SimpleDropSink;
            dropSink.CanDropOnItem = false;
            dropSink.Billboard.BackColor = Color.DodgerBlue;
            dropSink.Billboard.TextColor = Color.White;
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockBottom;
        }

        #endregion Constructors

        #region Methods

        public void LoadGroups()
        {
            groupList = new List<NavigatorGroup>();
            using (var context = new ScryfallCardsDbContext())
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

        #endregion Methods

        #region Events

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
            if (e.RowObject is NavigatorGroup navGroup)
            {
                using (var context = new ScryfallCardsDbContext())
                {
                    if (e.Control.Text == "Collections" || e.Control.Text == "Wish Lists" || e.Control.Text == "Decks")
                    {
                        if (navGroup.CollectionGroup != null)
                            context.Entry(navGroup.CollectionGroup).Reload();
                        MessageBox.Show("Group name conflicts with an existing permanent group");
                        return;
                    }
                    try
                    {
                        if (navGroup.CollectionGroup == null) // not yet added
                        {
                            navGroup.CollectionGroup = new CollectionGroup { GroupName = e.Control.Text, Permanent = false };
                            context.CollectionGroups.Add(navGroup.CollectionGroup);
                        }
                        else
                        {
                            navGroup.CollectionGroup.GroupName = e.Control.Text;
                            context.CollectionGroups.Update(navGroup.CollectionGroup);
                        }
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        DebugOutput.WriteLine(ex.ToString());
                        if (navGroup.CollectionGroup != null)
                            context.Entry(navGroup.CollectionGroup).Reload();
                        MessageBox.Show("Could not add/edit group");
                    }
                }
            }
            else if (e.RowObject is NavigatorCollection navCollection)
            {
                using (var context = new ScryfallCardsDbContext())
                {
                    if (e.Control.Text == "Main")
                    {
                        if (navCollection.CardCollection != null)
                            context.Entry(navCollection.CardCollection).Reload();
                        MessageBox.Show("Collection name conflicts with an existing permanent collection");
                        return;
                    }
                    try
                    {
                        if (navCollection.CardCollection == null) // not yet added
                        {
                            navCollection.CardCollection = new CardCollection
                            {
                                GroupId = navCollection.GroupId,
                                CollectionName = e.Control.Text,
                                Permanent = false,
                                Type = "collection",
                                Virtual = navCollection.Parent.Virtual
                            };
                            context.Collections.Add(navCollection.CardCollection);
                        }
                        else
                        {
                            navCollection.CardCollection.CollectionName = e.Control.Text;
                            context.Collections.Update(navCollection.CardCollection);
                            var doc = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => (x is CollectionViewForm form) && form.Collection.Id == navCollection.CardCollection.Id);
                            if (doc is CollectionViewForm collectionForm)
                            {
                                collectionForm.Text = navCollection.Name;
                                collectionForm.Hide();
                                collectionForm.Show();
                            }
                        }
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        DebugOutput.WriteLine(ex.ToString());
                        if (navCollection.CardCollection != null)
                            context.Entry(navCollection.CardCollection).Reload();
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
                        using (var context = new ScryfallCardsDbContext())
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
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to delete {(isGroup ? "group." : "collection.")}");
                        DebugOutput.WriteLine(ex.ToString());
                        return;
                    }
                }
                var collectionsToRemove = !isGroup
                    ? Globals.Forms.OpenCollectionForms.Where(x => x.Collection.Id == (navigatorItem as NavigatorCollection).Id).ToArray()
                    : Globals.Forms.OpenCollectionForms.Where(x => x.Collection.GroupId == (navigatorItem as NavigatorGroup).Id).ToArray();
                foreach (var collection in collectionsToRemove)
                {
                    Globals.Forms.OpenCollectionForms.Remove(collection);
                    collection.Close();
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
                var form = new NewCollectionForm();
                form.collectionNameTextBox.Text = "New Collection";
                var defaultPlatforms = SettingsManager.ApplicationSettings.DefaultPlatforms;
                if (defaultPlatforms[0] == '1')
                    form.platformComboBox.Text = "Paper";
                else if (defaultPlatforms[1] == '1')
                    form.platformComboBox.Text = "Arena";
                else if (defaultPlatforms[2] == '1')
                    form.platformComboBox.Text = "Magic Online";
                else
                    form.platformComboBox.Text = "Paper";
                if (form.ShowDialog() == DialogResult.OK)
                {
                    using (var context = new ScryfallCardsDbContext())
                    {
                        if (form.collectionNameTextBox.Text == "Main")
                        {
                            MessageBox.Show("Collection name conflicts with an existing permanent collection");
                            return;
                        }
                        try
                        {
                            var collection = new CardCollection
                            {
                                GroupId = group.CollectionGroup.Id,
                                CollectionName = form.collectionNameTextBox.Text,
                                Permanent = false,
                                Type = "collection",
                                Virtual = group.Virtual,
                                Platform = form.platformComboBox.Text
                            };

                            if (group.Name == "Decks")
                                collection.Type = "deck";
                            if (collection.Platform == "Magic Online")
                                collection.Platform = "MTGO";
                            context.Collections.Add(collection);
                            context.SaveChanges();
                            var newCollection = new NavigatorCollection { CardCollection = collection, Name = collection.CollectionName, Parent = group };
                            group.AddCollection(newCollection);
                            navigatorListView.RebuildAll(true);
                            navigatorListView.Expand(group);
                            navigatorListView.SelectedObject = newCollection;
                        }
                        catch (Exception ex)
                        {
                            DebugOutput.WriteLine(ex.ToString());
                            MessageBox.Show("Could not add collection");
                        }
                    }
                }                
            }
        }

        private void navigatorListView_ModelCanDrop(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            object rowObjectUnderMouse = navigatorListView.OlvHitTest(e.MouseLocation.X, e.MouseLocation.Y).RowObject;
            if (e.SourceModels[0] is NavigatorCollection collection)
            {
                if (!collection.Permanent)
                {
                    if (rowObjectUnderMouse is NavigatorGroup group)
                    {
                        navigatorListView.SelectedObject = group;
                        e.Effect = DragDropEffects.Copy;
                        e.InfoMessage = $"Move {collection.Name} to {group.Name} group";
                    }
                    else if (rowObjectUnderMouse is NavigatorCollection col && col != collection)
                    {
                        navigatorListView.SelectedObject = col;
                        e.Effect = DragDropEffects.Move;
                        e.InfoMessage = $"Move {collection.Name} to {col.Name} collection";
                    }
                }
            }
            else if (!(e.SourceModels[0] is InventoryTotalsItem) && rowObjectUnderMouse is NavigatorCollection navigatorCollection)
            {
                string DocumentName = navigatorCollection.CardCollection.CollectionName;
                e.Effect = DragDropEffects.Move;
                if (e.SourceModels[0] is OLVCardItem)
                    e.InfoMessage = $"Add {e.SourceModels.Count} card{(e.SourceModels.Count == 1 ? "" : "s")} to {DocumentName}";
                else if (e.SourceModels[0] is OLVSetItem setItem)
                    e.InfoMessage = $"Add set [{setItem.Name}] to {DocumentName}";
                else if (e.SourceModels[0] is OLVRarityItem rarityItem)
                    e.InfoMessage = $"Add {rarityItem.Rarity}s from [{(rarityItem.Parent as OLVSetItem).Name}] to {DocumentName}";
                else if (e.SourceModels[0] is InventoryCardCluster)
                {
                    int count = 0;
                    foreach (InventoryCardCluster cluster in e.SourceModels)
                        count += (int)cluster.Count;
                    var parentForm = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.cardListView == e.SourceListView || x.sideboardListView == e.SourceListView);
                    if (parentForm != null && parentForm.Collection.Id != navigatorCollection.Id)
                        e.InfoMessage = $"Move {count} card{(count == 1 ? "" : "s")} to {DocumentName}";
                }
                else if (e.SourceModels[0] is InventoryCard)
                {
                    int count = 0;
                    foreach (InventoryCard card in e.SourceModels)
                        count += (int)card.Count;
                    var parentForm = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.cardListView == e.SourceListView || x.sideboardListView == e.SourceListView);
                    if (parentForm != null && parentForm.Collection.Id != navigatorCollection.Id)
                        e.InfoMessage = $"Move {count} card{(count == 1 ? "" : "s")} to {DocumentName}";
                }
            }
        }

        private void navigatorListView_ModelDropped(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            object rowObjectUnderMouse = navigatorListView.OlvHitTest(e.MouseLocation.X, e.MouseLocation.Y).RowObject;
            if (e.SourceModels[0] is NavigatorCollection collection)
            {
                if (!collection.Permanent && rowObjectUnderMouse is NavigatorGroup group)
                {
                    using (var context = new ScryfallCardsDbContext())
                    {
                        try
                        {
                            collection.CardCollection.GroupId = group.Id;
                            if (collection.CardCollection.GroupName == "Wish Lists" || group.Name == "Wish Lists")
                                collection.CardCollection.Virtual = group.Virtual;
                            context.Update(collection.CardCollection);
                            context.SaveChanges();
                            collection.RemoveFromParent();
                            group.AddCollection(collection);
                            navigatorListView.RebuildAll(true);
                        }
                        catch (Exception ex)
                        {
                            DebugOutput.WriteLine(ex.ToString());
                            context.Entry(collection.CardCollection).Reload();
                        }
                    }
                }
                else if (rowObjectUnderMouse is NavigatorCollection targetCollection && targetCollection != collection)
                {
                    var items = e.SourceModels as ArrayList;
                    var sourceForm = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.Collection.Id == collection.Id);
                    var cards = new List<InventoryCard>();
                    using (var context = new ScryfallCardsDbContext())
                    {
                        try
                        {
                            cards = (from s in context.LibraryView
                                     where s.CollectionId == collection.Id
                                     select s).ToList();
                        }
                        catch (Exception ex)
                        {
                            DebugOutput.WriteLine(ex.ToString());
                        }
                    }

                    if (cards != null)
                    {
                        var args = new CardsDroppedEventArgs
                        {
                            Items = new ArrayList(cards),
                            SourceForm = sourceForm,
                            TargetCollection = targetCollection.CardCollection
                        };

                        OnCardsDropped(args);
                    }
                }
            }
            else if (rowObjectUnderMouse is NavigatorCollection navigatorCollection)
            {
                var items = new ArrayList();
                CollectionViewForm sourceForm = null;
                if (e.SourceModels[0] is InventoryCardCluster)
                {
                    foreach (InventoryCardCluster cluster in e.SourceModels)
                    {
                        items.AddRange(cluster.Cards);
                    }
                    sourceForm = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.cardListView == e.SourceListView || x.sideboardListView == e.SourceListView);
                }
                else if (e.SourceModels[0] is InventoryCard)
                {
                    items.AddRange(e.SourceModels);
                    sourceForm = Globals.Forms.OpenCollectionForms.FirstOrDefault(x => x.cardListView == e.SourceListView || x.sideboardListView == e.SourceListView);
                }

                var args = new CardsDroppedEventArgs
                {
                    Items = items,
                    SourceForm = sourceForm,
                    TargetCollection = navigatorCollection.CardCollection
                };

                if (sourceForm != null)
                {
                    if (e.SourceListView == sourceForm.cardListView)
                        args.SourceBoard = "mainboard";
                    else if (e.SourceListView == sourceForm.sideboardListView)
                        args.SourceBoard = "sideboard";
                }

                OnCardsDropped(args);
            }
        }

        public event EventHandler<CardsDroppedEventArgs> CardsDropped;

        private void OnCardsDropped(CardsDroppedEventArgs args)
        {
            CardsDropped?.Invoke(this, args);
        }

        #endregion Events

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
        public int GroupId => Parent?.Id ?? -1;
        public override bool Virtual => CardCollection?.Virtual ?? false;
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
        public override bool Virtual => CollectionGroup?.Virtual ?? false;
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
        public virtual bool Virtual { get; }
    }
}