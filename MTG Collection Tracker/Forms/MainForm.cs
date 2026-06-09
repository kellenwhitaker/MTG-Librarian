using KW.WinFormsUI.Docking;
using MTG_Librarian.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

//Note: editable columns - count, cost, tags, foil

namespace MTG_Librarian
{
    public partial class MainForm : Form
    {
        #region Fields

        private readonly SplashForm splash;
        private const int SmallIconWidth = 27;
        private const int SmallIconHeight = 21;
        private StatusBarActionButtonClickDelegate statusBarActionButtonClickDelegate;

        #endregion Fields

        #region Constructors

        public MainForm()
        {
            InitializeComponent();
            Globals.Forms.DockPanel = dockPanel1;
            Globals.Forms.DockPanel.SetDoubleBuffered();
            Globals.Forms.MainForm = this;
            splash = new SplashForm();
            splash.Show();
            SetupUI();
        }

        #endregion Constructors

        #region Methods

        private void SetupUI()
        {
            SetupImageLists();
            EventManager.DefaultCurrencyChanged += DefaultCurrencyChanged;
            Globals.Forms.CardInfoForm = new CardInfoForm();
            EventManager.CardImageRetrieved += Globals.Forms.CardInfoForm.cardImageRetrieved;
            Globals.Forms.NavigationForm = new CollectionNavigatorForm();
            Globals.Forms.NavigationForm.CardsDropped += EventManager.NavigationFormCardsDropped;
            Globals.Forms.DBViewForm = new DBViewForm();
            EventManager.InventoryChanged += Globals.Forms.DBViewForm.InventoryChanged;
            Globals.Forms.DBViewForm.setListView.AddObject(new OLVSetItem(""));
            Globals.Forms.DBViewForm.setListView.ClearObjects();
            Globals.Forms.DBViewForm.CardsActivated += EventManager.DBViewFormCardActivated;
            Globals.Forms.DBViewForm.CardSelected += EventManager.CardSelected;
            Globals.Forms.TasksForm = new TasksForm();
            Globals.Forms.TasksForm.InitializeTaskManager();
            Globals.Forms.TasksForm.tasksListView.GetColumn(0).Renderer = new IconRenderer();
            Globals.Forms.TasksForm.tasksListView.GetColumn(1).Renderer = new ProgressBarRenderer();
            Globals.Forms.TasksForm.TaskManager.SetDownloaded += EventManager.SetDownloaded;
            Globals.Forms.TasksForm.TaskManager.ScryfallSearchEnded += EventManager.ScryfallSearchEnded;
            Globals.Forms.TasksForm.TaskManager.CardsUpdatedFromScryfall += EventManager.CardsUpdatedFromScryfall;
            splitContainer1.SplitterDistance = Height;
            InitUIWorker.RunWorkerAsync();
        }

        public void UpdateStatusBarTotals(IList cards)
        {
            int cardCount = 0;
            double costTotal = 0;
            double priceTotal = 0;
            foreach (var card in cards)
            {
                if (card is FullInventoryCard inventoryCard)
                {
                    if (inventoryCard.Count.HasValue)
                    {
                        int count = inventoryCard.Count.Value;
                        cardCount += count;
                        if (inventoryCard.Cost.HasValue)
                            costTotal += count * inventoryCard.Cost.Value;
                        if (inventoryCard.Price.HasValue)
                            priceTotal += count * inventoryCard.Price.Value;
                    }
                }
            }

            if (cardCount > 0)
            {
                mainStatusLabel.Text = $"Selected: {cardCount} cards [{costTotal}] [{priceTotal}]";
                mainStatusLabel.Show();
            }
            else
            {
                mainStatusLabel.Text = "";
                mainStatusLabel.Hide();
            }
        }
        private static void SetupImageLists()
        {
            Globals.ImageLists.SmallIconList = new ImageList() { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(SmallIconWidth, SmallIconHeight) };
            Globals.ImageLists.ManaIcons = new ImageList() { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(18, 18) };
            Globals.ImageLists.SymbolIcons16 = new ImageList() { ColorDepth = ColorDepth.Depth24Bit, ImageSize = new Size(16, 16) };

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
            Globals.ImageLists.SymbolIcons16.Images.Add("{W/P}", Properties.Resources.WP_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{U/P}", Properties.Resources.UP_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{B/P}", Properties.Resources.BP_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{R/P}", Properties.Resources.RP_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{G/P}", Properties.Resources.GP_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{S}", Properties.Resources.S_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{T}", Properties.Resources.T_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{Q}", Properties.Resources.Q_16);
            Globals.ImageLists.SymbolIcons16.Images.Add("{E}", Properties.Resources.E_16);

            #endregion Add mana icons
        }

        public void AddSetIcon(string SetCode)
        {
            using (var context = new ScryfallCardsDbContext())
            {
                var set = (from s in context.Sets
                           where s.code == SetCode
                           select s).FirstOrDefault();
                if (set != null)
                {
                    if (set.CommonIcon != null) AddOrUpdateImageListImage(Globals.ImageLists.SmallIconList, $"{set.SymbolCode}: Common", set.CommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.UncommonIcon != null) AddOrUpdateImageListImage(Globals.ImageLists.SmallIconList, $"{set.SymbolCode}: Uncommon", set.UncommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.RareIcon != null) AddOrUpdateImageListImage(Globals.ImageLists.SmallIconList, $"{set.SymbolCode}: Rare", set.RareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.MythicRareIcon != null) AddOrUpdateImageListImage(Globals.ImageLists.SmallIconList, $"{set.SymbolCode}: Mythic", set.MythicRareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                }
            }
        }

        private void AddOrUpdateImageListImage(ImageList imageList, string key, Image image)
        {
            int index = imageList.Images.IndexOfKey(key);
            if (index > -1)
                imageList.Images[index] = image;
            else
                imageList.Images.Add(key, image);
        }

        private void AddSetIcons()
        {
            using (var context = new ScryfallCardsDbContext())
            {
                var sets = from s in context.Sets
                           select s;

                foreach (var set in sets)
                {
                    if (set.CommonIcon != null) Globals.ImageLists.SmallIconList.Images.Add($"{set.SymbolCode}: Common", set.CommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.UncommonIcon != null) Globals.ImageLists.SmallIconList.Images.Add($"{set.SymbolCode}: Uncommon", set.UncommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.RareIcon != null) Globals.ImageLists.SmallIconList.Images.Add($"{set.SymbolCode}: Rare", set.RareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.MythicRareIcon != null) Globals.ImageLists.SmallIconList.Images.Add($"{set.SymbolCode}: Mythic", set.MythicRareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                }
            }
        }

        #endregion Methods

        #region Events

        #region MainForm Events

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingsManager.FillSettings();
            SettingsManager.SaveSettings();
        }

        #endregion MainForm Events

        #region Menu Events

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
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm())
            {
                settingsForm.defaultCurrencyComboBox.Text = SettingsManager.ApplicationSettings.DefaultCurrency;
                settingsForm.defaultSearchLanguageComboBox.Text = SettingsManager.ApplicationSettings.DefaultSearchLanguage;
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    string defaultCurrency = SettingsManager.ApplicationSettings.DefaultCurrency;
                    SettingsManager.ApplicationSettings.DefaultCurrency = settingsForm.defaultCurrencyComboBox.Text;
                    SettingsManager.ApplicationSettings.DefaultSearchLanguage = settingsForm.defaultSearchLanguageComboBox.Text;
                    SettingsManager.ApplicationSettings.Save();
                    if (defaultCurrency != SettingsManager.ApplicationSettings.DefaultCurrency)
                        EventManager.OnDefaultCurrencyChanged();
                }
            }
        }
        #endregion Menu Events

        #region Misc Events

        private void DefaultCurrencyChanged(object sender, EventArgs e)
        {
            string DefaultCurrency = SettingsManager.ApplicationSettings.DefaultCurrency;
            foreach (var form in Globals.Forms.OpenCollectionForms)
            {
                var cardsToRefresh = new List<FullInventoryCard>();
                foreach (var item in form.cardListView.Objects)
                {
                    if (item is FullInventoryCard card)
                    {
                        cardsToRefresh.Add(card);
                        string priceString;
                        if (card.prices.TryGetValue($"{DefaultCurrency.ToLower()}{(card.Foil ? "_foil" : "")}", out priceString))
                            card.Price = Convert.ToDouble(priceString);
                        else
                            card.Price = null;
                    }
                }
                form.UpdateTotals();
                form.cardListView.RefreshObjects(cardsToRefresh);
            }
        }

        private delegate void StatusBarActionButtonClickDelegate();

        private void statusBarActionButton_Click(object sender, EventArgs e)
        {
            statusBarActionButtonClickDelegate?.Invoke();
        }

        #endregion Misc Events

        #endregion Events

        private void exportDeckcollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Globals.Forms.DockPanel.ActiveDocument != null)
            {
                var targetForm = Globals.Forms.DockPanel.ActiveDocument as CollectionViewForm;
                var targetCollection = targetForm.Collection;
                var targetLV = targetForm.cardListView;
                Directory.CreateDirectory("export");
                using (StreamWriter file = new StreamWriter($"export\\{targetCollection.CollectionName}.csv"))
                {
                    file.WriteLine("Quantity,Name,Code,PurchasePrice,Foil,Condition,Language,PurchaseDate");
                    foreach (var row in targetLV.Objects)
                    {
                        if (row is FullInventoryCard card)
                            file.WriteLine($"{card.Count},\"{card.DisplayName}\",{card.set_id},{(card.Cost.HasValue ? card.Cost : 0)},{(card.Foil ? 1 : 0)},0,0,{card.TimeAdded}");                       
                    }
                    
                }
            }
        }
        private void updateMissingSetIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateMissingSetIconsWorker.RunWorkerAsync();
        }
    }
}