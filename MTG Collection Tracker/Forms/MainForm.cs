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
//Note: editable columns - count, cost, tags
//      date format: {0:yyyy-MMM-dd}
//TODO restsharp
//TODO add card preview
//TODO improve bulk card add speed
//TODO only show part A in collection view
//TODO unknown error: collection unmodified
namespace MTG_Collection_Tracker
{
    public partial class MainForm : Form
    {
        static CardInfoForm cardInfoForm;
        CardNavigatorForm navForm;
        DBViewForm dbViewForm;
        static TasksForm tasksForm;
        SplashForm splash = new SplashForm();
        private static ImageList _manaIcons;
        private static ImageList _symbolIcons16;
        internal static ImageList ManaIcons => _manaIcons;
        internal static ImageList SymbolIcons16 => _symbolIcons16;
        internal static ImageList SmallIconList => smallIconList;
        private static MainForm thisForm;
        private const int SmallIconWidth = 27;
        private const int SmallIconHeight = 21;

        public MainForm()
        {
            InitializeComponent();
            thisForm = this;
            splash.Show();
            //AddSets();
            //FillGatherer();
            //AddLibraryCards();
            //SetupCache();
            //AddPrices();
            //UpdateCatalogIDs();
            SetupUI();
        }

        private void SetupUI()
        {
            SetupImageLists();
            cardInfoForm = new CardInfoForm();
            navForm = new CardNavigatorForm();
            dbViewForm = new DBViewForm();
            dbViewForm.CardActivated += dbFormCardActivated;
            dbViewForm.CardSelected += CardSelected;
            dbViewForm.CardFocused += CardFocused;
            tasksForm = new TasksForm(tasksLabel, tasksProgressBar);
            tasksForm.tasksListView.GetColumn(0).Renderer = new IconRenderer();
            tasksForm.tasksListView.GetColumn(1).Renderer = new ProgressBarRenderer();
            tasksForm.TaskManager.SetDownloaded += SetDownloaded;
            splitContainer1.SplitterDistance = Height;
            InitUIWorker.RunWorkerAsync();
        }

        private void SetupImageLists()
        {
            smallIconList = new ImageList(components) { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(SmallIconWidth, SmallIconHeight) };
            _manaIcons = new ImageList(components) { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(18, 18) };
            _symbolIcons16 = new ImageList(components) { ColorDepth = ColorDepth.Depth24Bit, ImageSize = new Size(16, 16) };

            #region Add mana icons
            ManaIcons.Images.Add("{W}", Properties.Resources.W_20);
            ManaIcons.Images.Add("{U}", Properties.Resources.U_20);
            ManaIcons.Images.Add("{B}", Properties.Resources.B_20);
            ManaIcons.Images.Add("{R}", Properties.Resources.R_20);
            ManaIcons.Images.Add("{G}", Properties.Resources.G_20);
            ManaIcons.Images.Add("{C}", Properties.Resources.C_20);
            ManaIcons.Images.Add("{X}", Properties.Resources.X_20);
            SymbolIcons16.Images.Add("{B}", Properties.Resources.B_16);
            SymbolIcons16.Images.Add("{R}", Properties.Resources.R_16);
            SymbolIcons16.Images.Add("{G}", Properties.Resources.G_16);
            SymbolIcons16.Images.Add("{W}", Properties.Resources.W_16);
            SymbolIcons16.Images.Add("{U}", Properties.Resources.U_16);
            SymbolIcons16.Images.Add("{0}", Properties.Resources.C0_16);
            SymbolIcons16.Images.Add("{1}", Properties.Resources.C1_16);
            SymbolIcons16.Images.Add("{2}", Properties.Resources.C2_16);
            SymbolIcons16.Images.Add("{3}", Properties.Resources.C3_16);
            SymbolIcons16.Images.Add("{4}", Properties.Resources.C4_16);
            SymbolIcons16.Images.Add("{5}", Properties.Resources.C5_16);
            SymbolIcons16.Images.Add("{6}", Properties.Resources.C6_16);
            SymbolIcons16.Images.Add("{7}", Properties.Resources.C7_16);
            SymbolIcons16.Images.Add("{8}", Properties.Resources.C8_16);
            SymbolIcons16.Images.Add("{9}", Properties.Resources.C9_16);
            SymbolIcons16.Images.Add("{10}", Properties.Resources.C10_16);
            SymbolIcons16.Images.Add("{11}", Properties.Resources.C11_16);
            SymbolIcons16.Images.Add("{12}", Properties.Resources.C12_16);
            SymbolIcons16.Images.Add("{13}", Properties.Resources.C13_16);
            SymbolIcons16.Images.Add("{14}", Properties.Resources.C14_16);
            SymbolIcons16.Images.Add("{15}", Properties.Resources.C15_16);
            SymbolIcons16.Images.Add("{16}", Properties.Resources.C16_16);
            SymbolIcons16.Images.Add("{17}", Properties.Resources.C17_16);
            SymbolIcons16.Images.Add("{18}", Properties.Resources.C18_16);
            SymbolIcons16.Images.Add("{19}", Properties.Resources.C19_16);
            SymbolIcons16.Images.Add("{20}", Properties.Resources.C20_16);
            SymbolIcons16.Images.Add("{X}", Properties.Resources.CX_16);
            SymbolIcons16.Images.Add("{C}", Properties.Resources.C_16);
            SymbolIcons16.Images.Add("{B/G}", Properties.Resources.BG_16);
            SymbolIcons16.Images.Add("{B/R}", Properties.Resources.BR_16);
            SymbolIcons16.Images.Add("{G/U}", Properties.Resources.GU_16);
            SymbolIcons16.Images.Add("{G/W}", Properties.Resources.GW_16);
            SymbolIcons16.Images.Add("{R/G}", Properties.Resources.RG_16);
            SymbolIcons16.Images.Add("{R/W}", Properties.Resources.RW_16);
            SymbolIcons16.Images.Add("{U/B}", Properties.Resources.UB_16);
            SymbolIcons16.Images.Add("{U/R}", Properties.Resources.UR_16);
            SymbolIcons16.Images.Add("{W/B}", Properties.Resources.WB_16);
            SymbolIcons16.Images.Add("{W/U}", Properties.Resources.WU_16);
            SymbolIcons16.Images.Add("{2/W}", Properties.Resources.C2W_16);
            SymbolIcons16.Images.Add("{2/U}", Properties.Resources.C2U_16);
            SymbolIcons16.Images.Add("{2/B}", Properties.Resources.C2B_16);
            SymbolIcons16.Images.Add("{2/R}", Properties.Resources.C2R_16);
            SymbolIcons16.Images.Add("{2/G}", Properties.Resources.C2G_16);
            #endregion
        }

        internal static void CardFocused(object sender, CardFocusedEventArgs e)
        {
            cardInfoForm.CardFocusedUuid = e.uuid;
        }

        private delegate void SetDownloadedDelegate(object sender, SetDownloadedEventArgs e);
        private void SetDownloaded(object sender, SetDownloadedEventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new SetDownloadedDelegate(SetDownloaded), sender, e);
            else
            {
                AddSetIcon(e.SetCode);
                dbViewForm.LoadSet(e.SetCode);
                if (tasksForm.TaskManager.TaskCount == 0)
                    dbViewForm.SortCardListView();
            }
        }

        private void cvFormCardsDropped(object sender, CardsDroppedEventArgs e)
        {
            if (dockPanel1.ActiveDocument is CollectionViewForm activeDocument)
            {
                var collectionName = activeDocument.DocumentName;
                using (MyDbContext context = new MyDbContext())
                {
                    List<InventoryCard> cardsAdded = new List<InventoryCard>();
                    int insertionIndex = 0;
                    foreach (OLVCardItem item in e.Items)
                    {
                        InventoryCard card = new InventoryCard { uuid = item.MagicCard.uuid, multiverseId_Inv = item.MagicCard.multiverseId, CollectionId = activeDocument.Collection.Id, InsertionIndex = insertionIndex };
                        context.Library.Add(card);
                        cardsAdded.Add(card);
                        insertionIndex++;
                    }
                    context.SaveChanges();
                    foreach (InventoryCard card in cardsAdded)
                    {
                        var cardInstance = card.ToFullCard(context);
                        if (cardInstance != null)
                            activeDocument.AddCardInstance(cardInstance);
                    }
                }
            }
        }

        private void cvFormCardsUpdated(object sender, CardsUpdatedEventArgs e)
        {
            if (dockPanel1.ActiveDocument is CollectionViewForm activeDocument)
            {
                var collectionName = activeDocument.DocumentName;
                using (MyDbContext context = new MyDbContext())
                {
                    try
                    {
                        foreach (FullInventoryCard card in e.Items)
                            context.Library.Update(card.InventoryCard);
                        
                        context.SaveChanges();
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

        private void dbFormCardActivated(object sender, CardActivatedEventArgs args)
        {
            if (dockPanel1.ActiveDocument is CollectionViewForm activeDocument)
            {
                var collectionName = activeDocument.DocumentName;
                InventoryCard card = new InventoryCard { uuid = args.MagicCard.uuid, multiverseId_Inv = args.MagicCard.multiverseId, CollectionId = activeDocument.Collection.Id };
                using (MyDbContext context = new MyDbContext())
                {
                    context.Library.Add(card);
                    context.SaveChanges();
                    var cardInstance = card.ToFullCard(context);
                    if (cardInstance != null)
                        activeDocument.AddCardInstance(cardInstance);
                }
            }
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
                    if (set.CommonIcon != null)     SmallIconList.Images.Add($"{set.Name}: Common",     set.CommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.UncommonIcon != null)   SmallIconList.Images.Add($"{set.Name}: Uncommon",   set.UncommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.RareIcon != null)       SmallIconList.Images.Add($"{set.Name}: Rare",       set.RareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.MythicRareIcon != null) SmallIconList.Images.Add($"{set.Name}: Mythic",     set.MythicRareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
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
                    if (set.CommonIcon != null)     SmallIconList.Images.Add($"{set.Name}: Common",     set.CommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.UncommonIcon != null)   SmallIconList.Images.Add($"{set.Name}: Uncommon",   set.UncommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.RareIcon != null)       SmallIconList.Images.Add($"{set.Name}: Rare",       set.RareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.MythicRareIcon != null) SmallIconList.Images.Add($"{set.Name}: Mythic",     set.MythicRareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                }
            }
        }

        private void navFormCollectionActivated(object sender, CollectionActivatedEventArgs e)
        {
            if (e.NavigatorCollection?.Name is string docName)
            {
                if (dockPanel1.Documents.FirstOrDefault(x => (x as CollectionViewForm).DocumentName == docName) is CollectionViewForm document)
                {
                    document.Activate();
                }
                else
                {
                    document = new CollectionViewForm { Collection = e.NavigatorCollection.CardCollection, Text = e.NavigatorCollection.Text };
                    document.LoadCollection();
                    document.CardsDropped += cvFormCardsDropped;
                    document.CardsUpdated += cvFormCardsUpdated;
                    document.CardSelected += CardSelected;
                    document.cardListView.SmallImageList = SmallIconList;
                    document.Show(dockPanel1, DockState.Document);
                    dockPanel1.ActiveDocumentPane.SetDoubleBuffered();
                    dockPanel1.SetDoubleBuffered();
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        internal static void CardSelected(object sender, CardSelectedEventArgs e)
        {
            CardFocused(sender, new CardFocusedEventArgs { uuid = e.uuid });
            using (CardImagesDbContext context = new CardImagesDbContext(e.Edition))
            {
                var imageBytes = (from i in context.CardImages
                                  where i.uuid == e.uuid
                                  select i).FirstOrDefault()?.CardImageBytes;

                if (imageBytes != null)
                {
                    Image img = ImageExtensions.FromByteArray(imageBytes);
                    OnCardImageRetrieved(new CardImageRetrievedEventArgs { uuid = e.uuid, CardImage = img });
                }
                else
                {
                    tasksForm.TaskManager.AddTask(new DownloadResourceTask { ForDisplay = true, Caption = $"Card Image: {e.Name}", URL = $"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={e.MultiverseId}&type=card", TaskObject = new BasicCardArgs { uuid = e.uuid, MultiverseId = e.MultiverseId, Edition = e.Edition }, OnTaskCompleted = ImageDownloadCompleted });
                }
            }
        }

        private static void ImageDownloadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = e.Result as CardResourceArgs;
            using (CardImagesDbContext context = new CardImagesDbContext(args.Edition))
            {
                context.Add(new DbCardImage { uuid = args.uuid, CardImageBytes = args.Data } );
                context.SaveChanges();
            }
            Image img = ImageExtensions.FromByteArray(args.Data);
            OnCardImageRetrieved(new CardImageRetrievedEventArgs { uuid = args.uuid, MultiverseId = args.MultiverseId, CardImage = img });
        }

        static internal event EventHandler<CardImageRetrievedEventArgs> CardImageRetrieved;
        private static void OnCardImageRetrieved(CardImageRetrievedEventArgs args)
        {
            CardImageRetrieved?.Invoke(thisForm, args);
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
