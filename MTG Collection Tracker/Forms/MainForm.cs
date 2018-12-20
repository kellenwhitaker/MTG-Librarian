using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using KW.WinFormsUI.Docking;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Net;
using System.Drawing;
using BrightIdeasSoftware;
using System.Collections;
using System.ComponentModel;
//Note: editable columns - count, cost, tags, foil
//TODO restsharp
//TODO2 add card preview
//TODO improve bulk card add speed
//TODO4 allow cards to be moved from one collection to another
//TODO4 save window z order on exit
//TODO4 add scrapedname field to sets, using that to check against instead of the set name when checking for new sets
//TODO4 allow updating of sets
//TODO4 add debugging output for debug/release builds
//TODO3 keep status bar constant height
//TODO unknown error: collection unmodified
namespace MTG_Librarian
{
    public partial class MainForm : Form
    {
        private static SplashForm splash = new SplashForm();
        private const int SmallIconWidth = 27;
        private const int SmallIconHeight = 21;
        private static ApplicationSettings ApplicationSettings;

        public MainForm()
        {
            InitializeComponent();
            Globals.Forms.DockPanel = dockPanel1;
            Globals.Forms.DockPanel.SetDoubleBuffered();
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
            var activeDocument = Globals.States.ActiveCollectionForm;
            if (activeDocument != null)
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
                    var fullCardsAdded = new List<FullInventoryCard>();
                    foreach (InventoryCard card in cardsAdded)
                    {
                        var fullCard = card.ToFullCard(context);
                        if (fullCard != null)
                            fullCardsAdded.Add(fullCard);
                        if (Globals.Collections.AllMagicCards.TryGetValue(card.uuid, out MagicCard magicCard))
                            magicCard.CopiesOwned++;
                    }
                    activeDocument.AddFullInventoryCards(fullCardsAdded);
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
            var activeDocument = Globals.States.ActiveCollectionForm;
            if (activeDocument != null)
            {
                var collectionName = activeDocument.DocumentName;
                var setItems = new Dictionary<string, OLVSetItem>();
                using (MyDbContext context = new MyDbContext())
                {
                    try
                    {
                        foreach (FullInventoryCard card in e.Items)
                        {
                            if (card.Count > 0)
                            {
                                context.Library.Update(card.InventoryCard);
                                if (!setItems.TryGetValue(card.Edition, out OLVSetItem setItem))
                                    if ((setItem = Globals.Forms.DBViewForm.SetItems.Where(x => x.Name == card.Edition).FirstOrDefault()) != null)
                                        setItems.Add(card.Edition, setItem);
                            }
                            else
                                context.Library.Remove(card.InventoryCard);
                        }
                        context.SaveChanges();
                        var cardsStillSelected = new List<FullInventoryCard>();
                        foreach (FullInventoryCard card in e.Items)
                        {
                            var allCopiesSum = (from c in context.LibraryView
                                            where c.uuid == card.uuid
                                            select c.Count).Sum();
                            if (allCopiesSum.HasValue && Globals.Collections.AllMagicCards.TryGetValue(card.uuid, out MagicCard magicCard))
                                magicCard.CopiesOwned = allCopiesSum.Value;
                            if (card.Count < 1)
                                activeDocument.cardListView.RemoveObject(card);
                            else
                                cardsStillSelected.Add(card);
                        }
                        activeDocument.cardListView.SelectedObjects = cardsStillSelected; // workaround: list view will not actually update selected items when removing from SelectedObjects
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

        public CollectionViewForm LoadCollection(int id, DockState dockState = DockState.Document)
        {
            CollectionViewForm collectionViewForm = null;
            CardCollection collection;
            using (var context = new MyDbContext())
            {
                collection = (from c in context.Collections
                              where c.Id == id
                              select c).FirstOrDefault();
            }
            if (collection != null)
                collectionViewForm = LoadCollection(collection, dockState);

            return collectionViewForm;
        }

        public CollectionViewForm LoadCollection(CardCollection collection, DockState dockState = DockState.Document)
        {
            var document = new CollectionViewForm { Collection = collection, Text = collection.CollectionName };
            document.LoadCollection();
            document.CardsDropped += cvFormCardsDropped;
            document.CardsUpdated += cvFormCardsUpdated;
            document.CardSelected += CardSelected;
            document.cardListView.SmallImageList = Globals.ImageLists.SmallIconList;
            document.Show(Globals.Forms.DockPanel, dockState);
            Globals.Forms.DockPanel.ActiveDocumentPane.SetDoubleBuffered();
            return document;
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
            ApplicationSettings = new ApplicationSettings();
            ApplicationSettings.MainFormWindowState = WindowState;
            ApplicationSettings.MainFormLocation = Location;
            ApplicationSettings.MainFormSize = Size;
            SaveDockState(DockState.DockLeft);
            SaveDockState(DockState.Document);
            SaveDockState(DockState.DockBottom);
            SaveDockState(DockState.DockRight);
            ApplicationSettings.Save();
        }

        private void SaveDockState(DockState dockState)
        {
            ApplicationSettings.GetDockPaneSettings(dockState).ContentPanes = new List<ApplicationSettings.DockContentSettings>();
            var dockWindow = Globals.Forms.DockPanel.DockWindows[dockState];
            if (dockWindow != null && dockWindow.NestedPanes.Count > 0)
            {
                var contentArray = dockWindow.NestedPanes[0].DockContentArray;
                var activeContent = dockWindow.NestedPanes[0].ActiveContent;
                foreach (var dockContent in contentArray)
                {
                    if (dockContent is CardInfoForm cardInfoForm)
                        ApplicationSettings.GetDockPaneSettings(dockState).ContentPanes.Add(new ApplicationSettings.DockContentSettings { ContentType = ApplicationSettings.DockContentEnum.CardInfoForm, IsActivated = cardInfoForm == activeContent });
                    else if (dockContent is DBViewForm dBViewForm)
                        ApplicationSettings.GetDockPaneSettings(dockState).ContentPanes.Add(new ApplicationSettings.DockContentSettings { ContentType = ApplicationSettings.DockContentEnum.DBViewForm, IsActivated = dBViewForm == activeContent });
                    else if (dockContent is CardNavigatorForm cardNavigatorForm)
                        ApplicationSettings.GetDockPaneSettings(dockState).ContentPanes.Add(new ApplicationSettings.DockContentSettings { ContentType = ApplicationSettings.DockContentEnum.NavigatorForm, IsActivated = cardNavigatorForm == activeContent });
                    else if (dockContent is TasksForm tasksForm)
                        ApplicationSettings.GetDockPaneSettings(dockState).ContentPanes.Add(new ApplicationSettings.DockContentSettings { ContentType = ApplicationSettings.DockContentEnum.TasksForm, IsActivated = tasksForm == activeContent });
                    else if (dockContent is CollectionViewForm collectionViewForm)
                        ApplicationSettings.GetDockPaneSettings(dockState).ContentPanes.Add(new ApplicationSettings.DockContentSettings { ContentType = ApplicationSettings.DockContentEnum.CollectionViewForm, IsActivated = collectionViewForm == activeContent, DocumentId = collectionViewForm.Collection.Id });
                }
            }
        }
 
        private void cardInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Globals.Forms.CardInfoForm.IsHidden)
                Globals.Forms.CardInfoForm.Show(Globals.Forms.DockPanel, DockState.DockLeft);
            else
                Globals.Forms.CardInfoForm.Activate();
        }

        private void dBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Globals.Forms.DBViewForm.IsHidden)
                Globals.Forms.DBViewForm.Show(Globals.Forms.DockPanel, DockState.DockBottom);
            else
                Globals.Forms.DBViewForm.Activate();
        }

        private void navigatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Globals.Forms.NavigationForm.IsHidden)
                Globals.Forms.NavigationForm.Show(Globals.Forms.DockPanel, DockState.DockRight);
            else
                Globals.Forms.NavigationForm.Activate();
        }

        private void tasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Globals.Forms.TasksForm.IsHidden)
                Globals.Forms.TasksForm.Show(Globals.Forms.DockPanel, DockState.DockRight);
            else
                Globals.Forms.TasksForm.Activate();
        }
    }
}
