using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using HtmlAgilityPack;
using System.Net;
using System.Threading;
using System.Diagnostics;
using BrightIdeasSoftware;
using System.Drawing;

namespace MTG_Collection_Tracker
{
    public partial class Form1 : Form
    {
        CollectionViewForm collectionViewForm = new CollectionViewForm();
        CardInfoForm cardInfoForm;
        CardNavigatorForm navForm;
        DBViewForm dbViewForm;
        TasksForm tasksForm;
        SplashForm splash = new SplashForm();
        protected internal static ImageList SmallIconList => smallIconList;
        private static ImageList _manaIcons;
        internal static ImageList ManaIcons => _manaIcons;

        public Form1()
        {
            InitializeComponent();
            splash.Show();
            //AddSets();
            //FillGatherer();
            //AddLibraryCards();
            //SetupCache();
            //AddPrices();
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
            {
                if (option.Attributes["value"].Value != "")
                    sets.Add(WebUtility.HtmlDecode(option.Attributes["value"].Value));
            }
            return sets;
        }

        private void SetupUI()
        {
            smallIconList = new ImageList(components)
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(18, 18)
            };
            _manaIcons = new ImageList()
            {
                ColorDepth = ColorDepth.Depth24Bit,
                ImageSize = new Size(18, 18),
                TransparentColor = Color.FromArgb(255, 189, 57, 149)
            };
            #region add mana icons
            ManaIcons.Images.Add("W", Properties.Resources.W20);
            ManaIcons.Images.Add("WC", Properties.Resources.W20c);
            ManaIcons.Images.Add("U", Properties.Resources.U20);
            ManaIcons.Images.Add("UC", Properties.Resources.U20c);
            ManaIcons.Images.Add("B", Properties.Resources.B20);
            ManaIcons.Images.Add("BC", Properties.Resources.B20c);
            ManaIcons.Images.Add("R", Properties.Resources.R20);
            ManaIcons.Images.Add("RC", Properties.Resources.R20c);
            ManaIcons.Images.Add("G", Properties.Resources.G20);
            ManaIcons.Images.Add("GC", Properties.Resources.G20c);
            ManaIcons.Images.Add("C", Properties.Resources.C20);
            ManaIcons.Images.Add("CC", Properties.Resources.C20c);
            ManaIcons.Images.Add("X", Properties.Resources.X20);
            ManaIcons.Images.Add("XC", Properties.Resources.X20c);
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

        private void dbFormCardActivated(object sender, DBViewForm.CardActivatedEventArgs args)
        {

        }

        private void AddPrices()
        {
            XElement root = XElement.Load("TCG_Player__Medium_.xml");
            var results = root.Element("list").Elements("mc");
            using (var context = new MyDbContext())
            {
                foreach (var result in results)
                {
                    int id = Convert.ToInt32(result.Element("id").Value);
                    double price = Convert.ToDouble(result.Element("dbprice").Value);
                    var card = from c in context.catalog
                               where c.MVid == id
                               select c;
                    if (card != null && card.Count() > 0)
                        card.First().OnlinePrice = price;
                }
                context.SaveChanges();
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
                    setName = StripPunctuation(setName).Trim();
                    if (setName.EndsWith(" EDITION"))
                        setName = setName.Replace(" EDITION", "");
                    setName = setName.Replace("CORE SET", "").Replace("MAGIC THE GATHERING", "").Replace("DUEL DECKS", "").Trim();
                    sets.Add(setName);
                }
            }
            return sets;
        }

        private string StripPunctuation(string input)
        {
            return new string(input.Where(c => !char.IsPunctuation(c)).ToArray());
        }

        private void checkForNewSetsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var sets = GetGathererSets();
            var archiveSets = GetWizardsArchiveSets();
            foreach (var set in sets)
            {
                string setName = StripPunctuation(set.Replace(" & ", " AND ")).Trim();
                setName = setName.ToUpper();
                if (setName.EndsWith(" EDITION"))
                    setName = setName.Replace(" EDITION", "");
                setName = setName.ToUpper().Replace("CORE SET", "").Replace("MAGIC THE GATHERING", "").Replace("DUEL DECKS", "").Replace("BOX SET", "").Trim();
            }
            using (var context = new MyDbContext())
            {
                var DBSets = from s in context.sets
                             select s;

                foreach (var set in DBSets)
                {
                    if (sets.Contains(set.Name))
                        sets.Remove(set.Name);
                }
            }
            e.Result = sets;
        }

        private void CheckForNewSetsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var sets = e.Result as List<string>;
            if (sets.Count > 0)
            {
                string newSets = "";
                foreach (var set in sets)
                    newSets += "[" + set + "] ";

                if (MessageBox.Show("The following new sets are available for download: \n\n" + newSets + "\n\nWould you like to download them now?", "New Sets Are Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (var set in sets)
                    {
                        DownloadSetTask task = new DownloadSetTask(set);
                        tasksForm.taskManager.AddTask(task);
                    }
                }
            }
        }

        private void InitUIWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Loading collections...", 1));
            navForm.LoadGroups();
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Loading catalog...", 2));
            dbViewForm.LoadSets();
            AddSetIcons();
            InitUIWorker.ReportProgress(0, new SplashProgressObject("Starting application...", 3));
            Thread.Sleep(100);
        }

        private void AddSetIcons()
        {
            using (var context = new MyDbContext())
            {
                var sets = from s in context.sets
                           select s;

                foreach (var set in sets)
                {
                    if (set.CommonIcon != null)     SmallIconList.Images.Add($"{set.Name}: Common", set.CommonIcon);
                    if (set.UncommonIcon != null)   SmallIconList.Images.Add($"{set.Name}: Uncommon", set.UncommonIcon);
                    if (set.RareIcon != null)       SmallIconList.Images.Add($"{set.Name}: Rare", set.RareIcon);
                    if (set.MythicRareIcon != null) SmallIconList.Images.Add($"{set.Name}: Mythic Rare", set.MythicRareIcon);
                }
            }
            Console.WriteLine(SmallIconList.Images.Count);
        }

        private void InitUIWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {      
            cardInfoForm.Show(dockPanel1, DockState.DockLeft);
            navForm.LoadTree();
            navForm.CollectionActivated += navFormCollectionActivated;
            navForm.Show(dockPanel1, DockState.DockRight);
            //collectionViewForm.Show(dockPanel1, DockState.Document);
            tasksForm.Show(dockPanel1, DockState.DockRight);
            navForm.Activate();
            dbViewForm.LoadTree();
            dbViewForm.Show(dockPanel1, DockState.DockBottom);
            dockPanel1.UpdateDockWindowZOrder(DockStyle.Left, true);
            dockPanel1.UpdateDockWindowZOrder(DockStyle.Right, true);
            //CheckForNewSetsWorker.RunWorkerAsync();
            Show();
        }

        private void navFormCollectionActivated(object sender, CollectionActivatedEventArgs e)
        {
            string docName = e.NavigatorCollection.Name;
            if (dockPanel1.Documents.FirstOrDefault(x => (x as CollectionViewForm).DocumentName == docName) is CollectionViewForm document)
            {
                document.Activate();
            }
            else
            {
                document = new CollectionViewForm { DocumentName = docName, Text = e.NavigatorCollection.Text };
                document.Show(dockPanel1, DockState.Document);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        
        private void InitUIWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            splash.ProgressChanged(e.UserState as SplashProgressObject);
        }

        public class ProgressBarRenderer : BaseRenderer
        {
            public override void Render(Graphics g, Rectangle r)
            {
                var task = ListItem.RowObject as BackgroundTask;
                Bitmap bmp = new Bitmap(task.ProgressBar.Width, task.ProgressBar.Height);
                task.ProgressBar.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                Rectangle backgroundRect = new Rectangle(r.Left, r.Top - 1, r.Width, r.Height + 1);
                if (IsItemSelected)
                    g.FillRectangle(new SolidBrush(Color.AliceBlue), backgroundRect);
                else
                    g.FillRectangle(Brushes.White, backgroundRect);
                if (task.Running)
                {
                    g.DrawImageUnscaled(bmp, r.Left + 5, r.Top + 20);
                    Font font = new Font(FontFamily.Families.Where(x => x.Name == "Segoe UI").First(), 9, FontStyle.Regular);
                    g.DrawString(GetText(), font, Brushes.Black, r.Left + 3, r.Top + 2);
                    g.DrawString(task.Runtime.ToString() + "s", new Font(font, FontStyle.Bold), Brushes.Green, r.Left + 5 + (int)g.MeasureString(task.Caption, font).Width, r.Top + 2);
                }
                else
                {
                    Font font = new Font(FontFamily.Families.Where(x => x.Name == "Segoe UI").First(), 10, FontStyle.Bold);
                    g.DrawString(task.Caption, font, Brushes.Black, r.Left + 3, r.Top + 7);
                    g.DrawString(task.Runtime.ToString() + "s", new Font(font, FontStyle.Regular), Brushes.Black, r.Left + 5 + (int)g.MeasureString(task.Caption, font).Width, r.Top + 7);
                }
            }
        }

        public class IconRenderer : BaseRenderer
        {
            public override void Render(Graphics g, Rectangle r)
            {
                Rectangle backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
                if (IsItemSelected)
                    g.FillRectangle(new SolidBrush(Color.AliceBlue), backgroundRect);
                else
                    g.FillRectangle(Brushes.White, backgroundRect);

                var task = ListItem.RowObject as BackgroundTask;
                Image icon = task.Icon;
                if (icon != null)
                {
                    int xinc = (r.Width - icon.Width) / 2;
                    int yinc = (r.Height - icon.Height) / 2;
                    g.DrawImage(icon, new Rectangle(r.X + xinc, r.Y + yinc, icon.Width, icon.Height));
                }
            }
        }
    }
}
