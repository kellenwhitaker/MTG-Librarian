using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using KW.WinFormsUI.Docking;
using System.Drawing;

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
            Globals.Forms.CardInfoForm = new CardInfoForm();
            EventManager.CardImageRetrieved += Globals.Forms.CardInfoForm.cardImageRetrieved;
            Globals.Forms.NavigationForm = new CollectionNavigatorForm();
            Globals.Forms.NavigationForm.CardsDropped += EventManager.NavigationFormCardsDropped;
            Globals.Forms.DBViewForm = new DBViewForm();
            Globals.Forms.DBViewForm.CardsActivated += EventManager.DBViewFormCardActivated;
            Globals.Forms.DBViewForm.CardSelected += EventManager.CardSelected;
            Globals.Forms.TasksForm = new TasksForm();
            Globals.Forms.TasksForm.InitializeTaskManager();
            Globals.Forms.TasksForm.tasksListView.GetColumn(0).Renderer = new IconRenderer();
            Globals.Forms.TasksForm.tasksListView.GetColumn(1).Renderer = new ProgressBarRenderer();
            Globals.Forms.TasksForm.TaskManager.SetDownloaded += EventManager.SetDownloaded;
            Globals.Forms.TasksForm.TaskManager.PricesFetched += EventManager.PricesFetched;
            splitContainer1.SplitterDistance = Height;
            InitUIWorker.RunWorkerAsync();
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

            #endregion Add mana icons
        }

        public void AddSetIcon(string SetCode)
        {
            using (var context = new MyDbContext())
            {
                var set = (from s in context.Sets
                           where s.Code == SetCode
                           select s).FirstOrDefault();
                if (set != null)
                {
                    if (set.CommonIcon != null) AddOrUpdateImageListImage(Globals.ImageLists.SmallIconList, $"{set.Name}: Common", set.CommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.UncommonIcon != null) AddOrUpdateImageListImage(Globals.ImageLists.SmallIconList, $"{set.Name}: Uncommon", set.UncommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.RareIcon != null) AddOrUpdateImageListImage(Globals.ImageLists.SmallIconList, $"{set.Name}: Rare", set.RareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.MythicRareIcon != null) AddOrUpdateImageListImage(Globals.ImageLists.SmallIconList, $"{set.Name}: Mythic", set.MythicRareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
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
            using (var context = new MyDbContext())
            {
                var sets = from s in context.Sets
                           select s;

                foreach (var set in sets)
                {
                    if (set.CommonIcon != null) Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Common", set.CommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.UncommonIcon != null) Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Uncommon", set.UncommonIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.RareIcon != null) Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Rare", set.RareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
                    if (set.MythicRareIcon != null) Globals.ImageLists.SmallIconList.Images.Add($"{set.Name}: Mythic", set.MythicRareIcon.SetCanvasSize(SmallIconWidth, SmallIconHeight));
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

        #endregion Menu Events

        #region Misc Events

        private delegate void StatusBarActionButtonClickDelegate();

        private void statusBarActionButton_Click(object sender, EventArgs e)
        {
            statusBarActionButtonClickDelegate?.Invoke();
        }

        #endregion Misc Events

        #endregion Events
    }
}