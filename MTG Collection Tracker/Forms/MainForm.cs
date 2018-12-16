using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Net;
using System.Drawing;
using BrightIdeasSoftware;
using System.Collections;
using System.ComponentModel;
//Note: editable columns - count, cost, tags, foil
//TODO restsharp
//TODO add card preview
//TODO improve bulk card add speed
//TODO unknown error: collection unmodified
namespace MTG_Librarian
{
    public partial class MainForm : Form
    {
        private static SplashForm splash = new SplashForm();
        private const int SmallIconWidth = 27;
        private const int SmallIconHeight = 21;

        public MainForm()
        {
            InitializeComponent();
            Globals.Forms.DockPanel = dockPanel1;
            Globals.Forms.MainForm = this;
            splash.Show();
            SetupUI();
        }

        private void SetupUI()
        {
            SetupImageLists();
            Globals.Forms.CardInfoForm = new CardInfoForm();
            Globals.Forms.NavigationForm = new CardNavigatorForm();
            Globals.Forms.DBViewForm = new DBViewForm();
            Globals.Forms.DBViewForm.CardsActivated += dbFormCardActivated;
            Globals.Forms.DBViewForm.CardSelected += CardSelected;
            Globals.Forms.DBViewForm.CardFocused += CardFocused;
            Globals.Forms.TasksForm = new TasksForm();
            Globals.Forms.TasksForm.InitializeTaskManager();
            Globals.Forms.TasksForm.tasksListView.GetColumn(0).Renderer = new IconRenderer();
            Globals.Forms.TasksForm.tasksListView.GetColumn(1).Renderer = new ProgressBarRenderer();
            Globals.Forms.TasksForm.TaskManager.SetDownloaded += SetDownloaded;
            splitContainer1.SplitterDistance = Height;
            InitUIWorker.RunWorkerAsync();
        }

        private void SetupImageLists()
        {
            Globals.ImageLists.SmallIconList = new ImageList(components) { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(SmallIconWidth, SmallIconHeight) };
            Globals.ImageLists.ManaIcons = new ImageList(components) { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(18, 18) };
            Globals.ImageLists.SymbolIcons16 = new ImageList(components) { ColorDepth = ColorDepth.Depth24Bit, ImageSize = new Size(16, 16) };

            #region Add mana icons
            Globals.ImageLists.ManaIcons.Images.Add("{W}", Properties.Resources.W_20);
            Globals.ImageLists.ManaIcons.Images.Add("{U}", Properties.Resources.U_20);
            Globals.ImageLists.ManaIcons.Images.Add("{B}", Properties.Resources.B_20);
            Globals.ImageLists.ManaIcons.Images.Add("{R}", Properties.Resources.R_20);
            Globals.ImageLists.ManaIcons.Images.Add("{G}", Properties.Resources.G_20);
            Globals.ImageLists.ManaIcons.Images.Add("{C}", Properties.Resources.C_20);
            Globals.ImageLists.ManaIcons.Images.Add("{X}", Properties.Resources.X_20);
            Globals.ImageLists.SymbolIcons16.Images.Add("{B}", Properties.Resources.B_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{R}", Properties.Resources.R_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{G}", Properties.Resources.G_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{W}", Properties.Resources.W_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{U}", Properties.Resources.U_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{0}", Properties.Resources.C0_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{1}", Properties.Resources.C1_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{2}", Properties.Resources.C2_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{3}", Properties.Resources.C3_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{4}", Properties.Resources.C4_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{5}", Properties.Resources.C5_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{6}", Properties.Resources.C6_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{7}", Properties.Resources.C7_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{8}", Properties.Resources.C8_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{9}", Properties.Resources.C9_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{10}", Properties.Resources.C10_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{11}", Properties.Resources.C11_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{12}", Properties.Resources.C12_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{13}", Properties.Resources.C13_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{14}", Properties.Resources.C14_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{15}", Properties.Resources.C15_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{16}", Properties.Resources.C16_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{17}", Properties.Resources.C17_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{18}", Properties.Resources.C18_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{19}", Properties.Resources.C19_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{20}", Properties.Resources.C20_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{X}", Properties.Resources.CX_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{C}", Properties.Resources.C_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{B/G}", Properties.Resources.BG_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{B/R}", Properties.Resources.BR_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{G/U}", Properties.Resources.GU_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{G/W}", Properties.Resources.GW_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{R/G}", Properties.Resources.RG_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{R/W}", Properties.Resources.RW_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{U/B}", Properties.Resources.UB_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{U/R}", Properties.Resources.UR_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{W/B}", Properties.Resources.WB_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{W/U}", Properties.Resources.WU_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{2/W}", Properties.Resources.C2W_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{2/U}", Properties.Resources.C2U_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{2/B}", Properties.Resources.C2B_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{2/R}", Properties.Resources.C2R_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{2/G}", Properties.Resources.C2G_16);
            #endregion
        }

        private void CountInventory()
        {
            using (var context = new MyDbContext())
            {
                var inventoryCards = from c in context.Library
                            select c;

                MagicCard magicCard;
                foreach (var inventoryCard in inventoryCards)
                {
                    if (inventoryCard.Count.HasValue && Globals.Collections.AllMagicCards.TryGetValue(inventoryCard.uuid, out magicCard))
                        magicCard.CopiesOwned += inventoryCard.Count.Value;
                }
            }
        }

        public static void CardFocused(object sender, CardFocusedEventArgs e)
        {
            Globals.States.CardFocusedUuid = e.uuid;
        }

        private delegate void SetDownloadedDelegate(object sender, SetDownloadedEventArgs e);
        private void SetDownloaded(object sender, SetDownloadedEventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new SetDownloadedDelegate(SetDownloaded), sender, e);
            else
            {
                AddSetIcon(e.SetCode);
                Globals.Forms.DBViewForm.LoadSet(e.SetCode);
                if (Globals.Forms.TasksForm.TaskManager.TaskCount == 0)
                    Globals.Forms.DBViewForm.SortCardListView();
            }
        }

        private InventoryCard AddMagicCardToCollection(MyDbContext context, MagicCard magicCard, int CollectionId, int insertionIndex = 0)
        {
            InventoryCard inventoryCard = new InventoryCard { DisplayName = magicCard.name, uuid = magicCard.uuid, multiverseId_Inv = magicCard.multiverseId, CollectionId = CollectionId, InsertionIndex = insertionIndex };
            if (magicCard.isFoilOnly)
                inventoryCard.Foil = true;
            else
                inventoryCard.Foil = false;
            if (magicCard.PartB != null)
                inventoryCard.PartB_uuid = magicCard.PartB.uuid;
            context.Library.Add(inventoryCard);
            return inventoryCard;
        }

        private void AddMagicCardsToActiveDocument(List<OLVCardItem> cards)
        {
            if (dockPanel1.ActiveDocument is CollectionViewForm activeDocument)
            {
                var collectionName = activeDocument.DocumentName;
                var setItems = new Dictionary<string, OLVSetItem>();
                using (MyDbContext context = new MyDbContext())
                {
                    List<InventoryCard> cardsAdded = new List<InventoryCard>();
                    int insertionIndex = 0;
                    foreach (OLVCardItem cardItem in cards)
                    {
                        MagicCard card = cardItem.MagicCard;
                        var inventoryCard = AddMagicCardToCollection(context, card, activeDocument.Collection.Id, insertionIndex);
                        cardsAdded.Add(inventoryCard);
                        insertionIndex++;
                        if (!setItems.TryGetValue(card.Edition, out OLVSetItem setItem))
                            if ((setItem = Globals.Forms.DBViewForm.SetItems.Where(x => x.Name == card.Edition).FirstOrDefault()) != null)
                                setItems.Add(card.Edition, setItem);
                    }
                    context.SaveChanges();
                    activeDocument.cardListView.Freeze();
                    foreach (InventoryCard card in cardsAdded)
                    {
                        var cardInstance = card.ToFullCard(context);
                        if (cardInstance != null)
                            activeDocument.AddCardInstance(cardInstance);
                        if (Globals.Collections.AllMagicCards.TryGetValue(card.uuid, out MagicCard magicCard))
                            magicCard.CopiesOwned++;
                    }
                    activeDocument.cardListView.Unfreeze();
                    Globals.Forms.DBViewForm.setListView.RefreshObjects(setItems.Values.ToArray());
                }
            }
        }

        private void cvFormCardsDropped(object sender, CardsDroppedEventArgs e)
        {
            if (e.Items[0] is OLVCardItem)
            {
                var cardItems = new List<OLVCardItem>();
                foreach (OLVCardItem cardItem in e.Items)
                    cardItems.Add(cardItem);
                AddMagicCardsToActiveDocument(cardItems);
            }
            else if (e.Items[0] is OLVSetItem setItem)
                AddMagicCardsToActiveDocument(setItem.Cards);
            else if (e.Items[0] is OLVRarityItem rarityItem)
                AddMagicCardsToActiveDocument(rarityItem.Cards);
        }

        private void cvFormCardsUpdated(object sender, CardsUpdatedEventArgs e)
        {
            if (dockPanel1.ActiveDocument is CollectionViewForm activeDocument)
            {
                var collectionName = activeDocument.DocumentName;
                var setItems = new Dictionary<string, OLVSetItem>();
                using (MyDbContext context = new MyDbContext())
                {
                    try
                    {
                        foreach (FullInventoryCard card in e.Items)
                        {
                            context.Library.Update(card.InventoryCard);
                            if (!setItems.TryGetValue(card.Edition, out OLVSetItem setItem))
                                if ((setItem = Globals.Forms.DBViewForm.SetItems.Where(x => x.Name == card.Edition).FirstOrDefault()) != null)
                                    setItems.Add(card.Edition, setItem);

                        }
                        context.SaveChanges();
                        foreach (FullInventoryCard card in e.Items)
                        {
                            var allCopiesSum = (from c in context.LibraryView
                                            where c.uuid == card.uuid
                                            select c.Count).Sum();
                            if (allCopiesSum.HasValue && Globals.Collections.AllMagicCards.TryGetValue(card.uuid, out MagicCard magicCard))
                                magicCard.CopiesOwned = allCopiesSum.Value;
                        }
                        Globals.Forms.DBViewForm.setListView.RefreshObjects(setItems.Values.ToArray());
                    }
                    catch (Exception ex)
                    {
                        foreach (FullInventoryCard card in e.Items)
                            context.Entry(card).Reload();

                        MessageBox.Show(ex.ToString());
                    }
                    finally { activeDocument.cardListView.Refresh(); }
                }
            }
        }

        private void dbFormCardActivated(object sender, CardsActivatedEventArgs args)
        {
            cvFormCardsDropped(sender, new CardsDroppedEventArgs { Items = args.CardItems });
        }

        private void AddSetIcon(string SetCode)
        {
            using (var context = new MyDbContext())
            {
                var set = (from s in context.Sets
                           where s.Code == SetCode
                           select s).FirstOrDefault();
                if (set != null)
                {
                    if (set.CommonIcon != null)     Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Common",     set.CommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.UncommonIcon != null)   Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Uncommon",   set.UncommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.RareIcon != null)       Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Rare",       set.RareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.MythicRareIcon != null) Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Mythic",     set.MythicRareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                }
            }
        }

        private void AddSetIcons()
        {
            using (var context = new MyDbContext())
            {
                var sets = from s in context.Sets
                           select s;

                foreach (var set in sets)
                {
                    if (set.CommonIcon != null)     Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Common",     set.CommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.UncommonIcon != null)   Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Uncommon",   set.UncommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.RareIcon != null)       Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Rare",       set.RareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.MythicRareIcon != null) Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Mythic",     set.MythicRareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                }
            }
        }

        public void LoadCollection(int id)
        {
            CardCollection collection;
            using (var context = new MyDbContext())
            {
                collection = (from c in context.Collections
                              where c.Id == id
                              select c).FirstOrDefault();
            }
            if (collection != null)
                LoadCollection(collection);
        }

        public void LoadCollection(CardCollection collection)
        {
            var document = new CollectionViewForm { Collection = collection, Text = collection.CollectionName };
            document.LoadCollection();
            document.CardsDropped += cvFormCardsDropped;
            document.CardsUpdated += cvFormCardsUpdated;
            document.CardSelected += CardSelected;
            document.cardListView.SmallImageList = Globals.ImageLists.SmallIconList;
            document.Show(dockPanel1, DockState.Document);
            dockPanel1.ActiveDocumentPane.SetDoubleBuffered();
            dockPanel1.SetDoubleBuffered();
        }

        private void navFormCollectionActivated(object sender, CollectionActivatedEventArgs e)
        {
            if (e.NavigatorCollection?.Name is string docName)
            {
                if (dockPanel1.Documents.FirstOrDefault(x => (x as CollectionViewForm).DocumentName == docName) is CollectionViewForm document)
                    document.Activate();
                else
                    LoadCollection(e.NavigatorCollection.CardCollection);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        public static void CardSelected(object sender, CardSelectedEventArgs e)
        {
            MagicCardBase card = e.MagicCard;
            Globals.Forms.CardInfoForm.CardSelected(card);
            CardFocused(sender, new CardFocusedEventArgs { uuid = card.uuid });
            
            using (CardImagesDbContext context = new CardImagesDbContext(card.Edition))
            {
                var imageBytes = (from i in context.CardImages
                                  where i.uuid == card.uuid
                                  select i).FirstOrDefault()?.CardImageBytes;

                if (imageBytes != null)
                {
                    Image img = ImageExtensions.FromByteArray(imageBytes);
                    OnCardImageRetrieved(new CardImageRetrievedEventArgs { uuid = card.uuid, CardImage = img });
                }
                else
                {
                    Globals.Forms.TasksForm.TaskManager.AddTask(new DownloadResourceTask { ForDisplay = true, Caption = $"Card Image: {card.name}", URL = $"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={card.multiverseId}&type=card", TaskObject = new BasicCardArgs { uuid = card.uuid, MultiverseId = card.multiverseId, Edition = card.Edition }, OnTaskCompleted = ImageDownloadCompleted });
                }
            }
        }

        private static void ImageDownloadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = e.Result as CardResourceArgs;
            try
            {
                using (CardImagesDbContext context = new CardImagesDbContext(args.Edition))
                {
                    context.Add(new DbCardImage { uuid = args.uuid, CardImageBytes = args.Data });
                    context.SaveChanges();
                }
                Image img = ImageExtensions.FromByteArray(args.Data);
                OnCardImageRetrieved(new CardImageRetrievedEventArgs { uuid = args.uuid, MultiverseId = args.MultiverseId, CardImage = img });
            }
            catch (Exception ex) { }
        }

        static public event EventHandler<CardImageRetrievedEventArgs> CardImageRetrieved;
        private static void OnCardImageRetrieved(CardImageRetrievedEventArgs args)
        {
            CardImageRetrieved?.Invoke(Globals.Forms.MainForm, args);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.OpenCollections = new System.Collections.Specialized.StringCollection();
            int index = 0;
            foreach (var doc in dockPanel1.Documents)
            {
                if (doc is CollectionViewForm collectionDoc)
                {
                    Properties.Settings.Default.OpenCollections.Add(collectionDoc.Collection.Id.ToString());
                    if (collectionDoc.IsActivated)
                        Properties.Settings.Default.ActiveCollectionIndex = index;
                }
                index++;
            }
            Properties.Settings.Default.Save();
        }
    }

    class MyCustomSortingDataSource : FastObjectListDataSource
    {
        public MyCustomSortingDataSource(FastObjectListView lv) : base(lv) { }
        override public void Sort(OLVColumn column, SortOrder order)
        {
            if (order != SortOrder.None)
            {
                ArrayList objects = (ArrayList)listView.Objects;
                objects.Sort(new ModelObjectComparer(listView.AllColumns.Where(x => x.AspectName == "TimeAdded").FirstOrDefault(), order));
            }
            RebuildIndexMap();
        }
    }
}
