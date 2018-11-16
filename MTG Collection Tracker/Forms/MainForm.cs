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
//TODO spreadsheet link: https://docs.google.com/spreadsheets/d/1sm3lCzmUg_XjctafEOZL--FPjaw60ToF_58u7Cy-Bmg/export?format=ods
//TODO add support for magiccards.info and make default
//TODO add card preview
//TODO improve bulk card add speed
//TODO improve card set filter tree
//      deselect on change
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
            UpdateCatalogIDs();
            SetupUI();
        }

        private List<string> GetGathererSets()
        {
            string URL = "http://gatherer.wizards.com/Pages/Default.aspx";
            var web = new HtmlWeb();
            var doc = web.Load(URL);
            var sets = new List<string>();
            var options = doc.DocumentNode.Descendants("select")
                .Where(x => x.Attributes["id"].Value == "ctl00_ctl00_MainContent_Content_SearchControls_setAddText")
                .First()
                .Descendants("option");
            foreach (var option in options)
                if (option.Attributes["value"].Value != "")
                    sets.Add(WebUtility.HtmlDecode(option.Attributes["value"].Value));

            return sets;
        }

        private List<CardSet> GetWikipediaSets()
        {
            int GetOrdinal(HtmlNode table, string headerText)
            {
                var prevSiblings = table.Descendants("th")
                                        .Where(x => {
                                            string cleanString = x.InnerText.Replace("\n", "").Replace(" ", "").ToUpper();
                                            return cleanString.StartsWith(headerText.Replace(" ", "").ToUpper());
                                        })
                                        .FirstOrDefault()?.SelectNodes("preceding-sibling::th");
                return prevSiblings != null ? prevSiblings.Count() + 1 : 1;
            }

            HtmlNode RemoveLinks(HtmlNode node)
            {
                var links = node?.SelectNodes($"descendant::a[contains(@href, '#')]");
                if (links != null)
                    foreach (var link in links)
                        link.Remove();

                return node;
            }
            //TODO change to user master set list or MCI
            string URL = "https://en.wikipedia.org/wiki/List_of_Magic:_The_Gathering_sets";
            var doc = new HtmlWeb().Load(URL);
            var sets = new List<CardSet>();
            var tables = doc.DocumentNode.SelectNodes(@"//table[@class='wikitable']");
            using (var context = new MyDbContext())
            {
                foreach (var table in tables)
                {
                    var type = table.SelectSingleNode(@"(preceding-sibling::*[starts-with(name(), 'h')]) [last()]");
                    int setOrdinal = GetOrdinal(table, "set");
                    int releaseOrdinal = GetOrdinal(table, "release date");
                    int codeOrdinal = GetOrdinal(table, "expansion code");
                    var dataRows = table.SelectNodes(@"descendant::tr[descendant::td]");

                    foreach (var dataRow in dataRows)
                    {
                        if (dataRow.SelectNodes($"descendant::td").Count == 1)
                            continue;
                        var set = RemoveLinks(dataRow.SelectSingleNode($"descendant::td [{setOrdinal}]"));
                        if (set != null && set.InnerText != "")
                        {
                            var block = RemoveLinks(dataRow.NodesBeforeSelf()
                                        .Where(x => x.Descendants("td").Count() == 1 && x.Descendants("td").Any(y => y.InnerText.ToUpper().Contains("BLOCK")))
                                        .FirstOrDefault());
                            var releaseDate = RemoveLinks(dataRow.SelectSingleNode($"descendant::td [{releaseOrdinal}]"));
                            var code = RemoveLinks(dataRow.SelectSingleNode($"descendant::td [{codeOrdinal}]"));
                            var cardSet = new CardSet { Block = block?.InnerText, Name = set.InnerText, ReleaseDate = releaseDate?.InnerText, Type = type?.InnerText, Code = code?.InnerText.Replace("none", "").Replace("N/A", "") };
                            Console.WriteLine("{0}: {1} [{2}] {3} {4}", cardSet.Block, cardSet.Name, cardSet.Code, cardSet.ReleaseDate, cardSet.Type);
                            context.Sets.Update(cardSet);
                            //context.SaveChanges();
                        }
                    }
                }
                //context.SaveChanges();
            }
            return sets;
        }

        private void SetupUI()
        {
            smallIconList  =    new ImageList(components) { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(21, 21) };
            _manaIcons     =    new ImageList(components) { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(18, 18) };
            _symbolIcons16 =    new ImageList(components) { ColorDepth = ColorDepth.Depth24Bit, ImageSize = new Size(16, 16) };

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
            cardInfoForm = new CardInfoForm();
            navForm = new CardNavigatorForm();
            dbViewForm = new DBViewForm();
            dbViewForm.CardActivated += dbFormCardActivated;
            tasksForm = new TasksForm(tasksLabel, tasksProgressBar);
            tasksForm.tasksListView.GetColumn(0).Renderer = new IconRenderer();
            tasksForm.tasksListView.GetColumn(1).Renderer = new ProgressBarRenderer();
            splitContainer1.SplitterDistance = Height;
            InitUIWorker.RunWorkerAsync();
        }

        private void cvFormCardsDropped(object sender, CardDroppedEventArgs e)
        {
            if (dockPanel1.ActiveDocument is CollectionViewForm activeDocument)
            {
                var collectionName = activeDocument.DocumentName;
                using (MyDbContext context = new MyDbContext())
                {
                    List<DBCardInstance> cardsAdded = new List<DBCardInstance>();
                    int insertionIndex = 0;
                    foreach (OLVCardItem item in e.Items)
                    {
                        DBCardInstance card = new DBCardInstance { MVid = item.MCard.MVid, CollectionName = collectionName, InsertionIndex = insertionIndex };
                        context.Library.Add(card);
                        cardsAdded.Add(card);
                        insertionIndex++;
                    }
                    context.SaveChanges();
                    foreach (DBCardInstance card in cardsAdded)
                    {
                        var cardInstance = card.ToCardInstance(context);
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
                        foreach (CardInstance card in e.Items)
                            context.Library.Update(card.DBCardInstance);
                        
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        foreach (CardInstance card in e.Items)
                            context.Entry(card).Reload();

                        MessageBox.Show(ex.ToString());
                    }
                    finally { activeDocument.fastObjectListView1.Refresh(); }
                }
            }
        }

        private void dbFormCardActivated(object sender, CardActivatedEventArgs args)
        {
            if (dockPanel1.ActiveDocument is CollectionViewForm activeDocument)
            {
                var collectionName = activeDocument.DocumentName;
                DBCardInstance card = new DBCardInstance { MVid = args.MCard.MVid, CollectionName = collectionName };
                using (MyDbContext context = new MyDbContext())
                {
                    context.Library.Add(card);
                    context.SaveChanges();
                    var cardInstance = card.ToCardInstance(context);
                    if (cardInstance != null)
                        activeDocument.AddCardInstance(cardInstance);
                }
            }
        }

        private List<string> GetWizardsArchiveSets()
        {
            var sets = new List<string>();
            string URL = "https://magic.wizards.com/en/products/card-set-archive";
            var web = new HtmlWeb();
            var doc = web.Load(URL);
            var tables = doc.DocumentNode
                         .Descendants("div")
                         .Where(x => x.HasClass("card-set-archive-table"));

            foreach (var table in tables)
            {
                var title = table
                            .Descendants("li")
                            .Where(x => x.HasClass("title"))
                            .FirstOrDefault();
                if (title != null && title.InnerText.Contains("Featured"))
                    continue;

                string titleText = title != null ? WebUtility.HtmlDecode(title.InnerText.Trim()) : "";
                var tableSets = table
                                .Descendants("li")
                                .Where(x => !x.HasClass("title"));
                foreach (var set in tableSets)
                {
                    string setName = WebUtility.HtmlDecode(set
                                     .Descendants("span")
                                     .Where(x => x.HasClass("nameSet"))
                                     .FirstOrDefault()
                                     .InnerText.Trim()).ToUpper();

                    string releaseDate = WebUtility.HtmlDecode(set
                                         .Descendants("span")
                                         .Where(x => x.HasClass("date-display-single") || x.HasClass("releaseDate"))
                                         .FirstOrDefault()
                                         .InnerText.Trim());
                    setName = setName.Replace(" & ", " AND ").Replace(" / RENAISSANCE", "");
                    setName = setName.StripPunctuation().Trim();
                    if (setName.EndsWith(" EDITION"))
                        setName = setName.Replace(" EDITION", "");
                    setName = setName.Replace("CORE SET", "").Replace("MAGIC THE GATHERING", "").Replace("DUEL DECKS", "").Trim();
                    sets.Add(setName);
                }
            }
            return sets;
        }

        private void AddSetIcons()
        {
            using (var context = new MyDbContext())
            {
                var sets = from s in context.Sets
                           select s;

                foreach (var set in sets)
                {
                    if (set.CommonIcon != null)     SmallIconList.Images.Add($"{set.Name}: Common", set.CommonIcon.EnlargeCanvas(21, 21));
                    if (set.UncommonIcon != null)   SmallIconList.Images.Add($"{set.Name}: Uncommon", set.UncommonIcon.EnlargeCanvas(21, 21));
                    if (set.RareIcon != null)       SmallIconList.Images.Add($"{set.Name}: Rare", set.RareIcon.EnlargeCanvas(21, 21));
                    if (set.MythicRareIcon != null) SmallIconList.Images.Add($"{set.Name}: Mythic Rare", set.MythicRareIcon.EnlargeCanvas(21, 21));
                }
            }
        }
        //TODO double click opens group on nav form
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
                    document = new CollectionViewForm { DocumentName = docName, Text = e.NavigatorCollection.Text };
                    document.LoadCollection();
                    document.CardsDropped += cvFormCardsDropped;
                    document.CardsUpdated += cvFormCardsUpdated;
                    document.fastObjectListView1.SmallImageList = SmallIconList;
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
            using (CardImagesDbContext context = new CardImagesDbContext(e.Edition))
            {
                var imageBytes = (from i in context.CardImages
                                 where i.MVid == e.MVid
                                 select i).FirstOrDefault()?.CardImageBytes;

                if (imageBytes != null)
                {
                    Image img = ImageExtensions.FromByteArray(imageBytes);
                    OnCardImageRetrieved(new CardImageRetrievedEventArgs { MVid = e.MVid, CardImage = img });
                }
                else
                {
                    tasksForm.taskManager.AddTask(new DownloadResourceTask { Caption = "Image download", URL = $"http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={e.MVid}&type=card", TaskObject = new BasicCardArgs { MVid = e.MVid, Edition = e.Edition }, OnTaskCompleted = ImageDownloadCompleted });   
                }
            }
        }

        private static void ImageDownloadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = e.Result as CardResourceArgs;
            using (CardImagesDbContext context = new CardImagesDbContext(args.Edition))
            {
                context.Add(new DbCardImage { MVid = args.MVid, CardImageBytes = args.Data } );
                context.SaveChanges();
            }
            Image img = ImageExtensions.FromByteArray(args.Data);
            OnCardImageRetrieved(new CardImageRetrievedEventArgs { MVid = args.MVid, CardImage = img });
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
