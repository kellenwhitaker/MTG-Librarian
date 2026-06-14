using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KW.WinFormsUI.Docking;
using BrightIdeasSoftware;
using System.Collections;
// TODO: add operators to mana cost field in search panel
namespace MTG_Librarian
{
    public partial class DBViewForm : DockForm
    {
        #region Fields

        private Dictionary<string, OLVSetItem> sets;
        public List<OLVSetItem> SetItems = new List<OLVSetItem>();
        public bool SearchHasMoreResults = false;
        object SetSelected = null;
        public bool addingToCLV = false;
        private Point mouseLocation;

        #endregion Fields

        #region Constructors

        public DBViewForm()
        {
            InitializeComponent();
            setListView.SmallImageList = Globals.ImageLists.SmallIconList;
            setListView.TreeColumnRenderer = new SetRenderer();
            setListView.UseFiltering = true;
            cardListView.SmallImageList = Globals.ImageLists.SmallIconList;
            cardListView.VirtualListDataSource = new MyCustomSortingDataSource(cardListView);
            var setsPage = new TabPage("Sets");
            setsPage.Controls.Add(setsPanel);
            setsPanel.Dock = DockStyle.Fill;
            compatibleTabControl1.TabPages.Add(setsPage);
            var paramsPage = new TabPage("Additional search parameters");
            paramsPage.Controls.Add(searchParametersPanel);
            searchParametersPanel.Dock = DockStyle.Fill;
            compatibleTabControl1.TabPages.Add(paramsPage);
            compatibleTabControl1.Dock = DockStyle.Fill;
            whiteManaButton.ImageList = blueManaButton.ImageList = blackManaButton.ImageList = redManaButton.ImageList = greenManaButton.ImageList
                                      = colorlessManaButton.ImageList = genericManaButton.ImageList = Globals.ImageLists.ManaIcons;
            (whiteManaButton.ImageKey, blueManaButton.ImageKey) = ("{W}", "{U}");
            (blackManaButton.ImageKey, redManaButton.ImageKey, greenManaButton.ImageKey) = ("{B}", "{R}", "{G}");
            (colorlessManaButton.ImageKey, genericManaButton.ImageKey) = ("{C}", "{X}");
            colorsWhiteButton.ImageList = colorsBlueButton.ImageList = colorsBlackButton.ImageList = colorsRedButton.ImageList = colorsGreenButton.ImageList
                                        = colorsColorlessButton.ImageList = Globals.ImageLists.ManaIcons;
            (colorsWhiteButton.ImageKey, colorsBlueButton.ImageKey, colorsBlackButton.ImageKey) = ("{W}", "{U}", "{B}");
            (colorsRedButton.ImageKey, colorsGreenButton.ImageKey, colorsColorlessButton.ImageKey) = ("{R}", "{G}", "{C}");
            commanderWhiteButton.ImageList = commanderBlueButton.ImageList = commanderBlackButton.ImageList = commanderRedButton.ImageList = commanderGreenButton.ImageList
                                           = commanderColorlessButton.ImageList = Globals.ImageLists.ManaIcons;
            (commanderWhiteButton.ImageKey, commanderBlueButton.ImageKey, commanderBlackButton.ImageKey) = ("{W}", "{U}", "{B}");
            (commanderRedButton.ImageKey, commanderGreenButton.ImageKey, commanderColorlessButton.ImageKey) = ("{R}", "{G}", "{C}");
            cardListView.UseFiltering = true;
            cardListView.SetDoubleBuffered();
            cardListView.AllColumns.FirstOrDefault(x => x.AspectName == "ManaCost").Renderer = new ManaCostRenderer();
            formatFilterComboBox.SelectedIndex = 0;
            uniqueComboBox.SelectedIndex = 2;
            languageComboBox.Text = SettingsManager.ApplicationSettings.DefaultSearchLanguage;
            pricesCurrencyComboBox.Text = SettingsManager.ApplicationSettings.DefaultCurrency;
            var defaultPlatforms = SettingsManager.ApplicationSettings.DefaultPlatforms;
            paperCheckBox.Checked = defaultPlatforms[0] == '1';
            arenaCheckBox.Checked = defaultPlatforms[1] == '1';
            magicOnlineCheckBox.Checked = defaultPlatforms[2] == '1';
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockBottom;
        }

        #endregion Constructors

        #region Methods

        private string BuildScryfallQuery()
        {
            string query = "";
            string manaSymbols = "";
            string plus = "+";
            query += $"include_variations={(includeVariationsCheckBox.Checked ? "true" : "false")}";
            switch (uniqueComboBox.SelectedIndex)
            {
                case 0: query += "&unique=cards"; break;
                case 1: query += "&unique=art"; break;
                default: query += "&unique=prints"; break;
            }
            if (cardListView.LastSortColumn != null)
            {
                switch (cardListView.LastSortColumn.AspectName)
                {
                    case "DisplayName": query += "order=name"; break;
                    case "ManaCost": query += "order=cmc"; break;
                    case "Set": query += "order=set"; break;
                    case "Price": query += $"order={SettingsManager.ApplicationSettings.DefaultCurrency.ToLower()}"; break;
                    default: break;
                }
                if (cardListView.LastSortOrder == SortOrder.Ascending)
                    query += "&dir=asc";
                else if (cardListView.LastSortOrder == SortOrder.Descending)
                    query += "&dir=desc";
            }
            query += "&q=";
            var queryClauses = new List<string>();
            if (whiteManaButton.Checked) manaSymbols += "W";
            if (blueManaButton.Checked) manaSymbols += "U";
            if (blackManaButton.Checked) manaSymbols += "B";
            if (redManaButton.Checked) manaSymbols += "R";
            if (greenManaButton.Checked) manaSymbols += "G";
            if (colorlessManaButton.Checked) manaSymbols += "C";
            if (!string.IsNullOrWhiteSpace(manaSymbols))
                queryClauses.Add($"(m%3A{manaSymbols})");
            if (!string.IsNullOrWhiteSpace(cardNameFilterBox.UserText))
                queryClauses.Add($"name%3A{cardNameFilterBox.UserText}");
            if (!string.IsNullOrWhiteSpace(cardTextFilterTextBox.UserText))
                queryClauses.Add($"oracle%3A{cardTextFilterTextBox.UserText}");
            if (!string.IsNullOrWhiteSpace(typeFilterTextBox.UserText))
                queryClauses.Add($"type%3A{typeFilterTextBox.UserText}");
            if (setListView.SelectedObject != null)
            {
                string setCode = (setListView.SelectedObject as OLVSetItem).CardSet.code;
                queryClauses.Add($"set%3A{setCode}");
            }
            if (formatFilterComboBox.SelectedIndex > 0)
                queryClauses.Add($"(legal%3A{formatFilterComboBox.SelectedItem.ToString()}+or+restricted%3A{formatFilterComboBox.SelectedItem.ToString()})");

            // --- Append inputs from searchParametersPanel ---
            // Helper to percent-encode comparison operators used in the query
            string EncodeOperator(string op)
            {
                if (string.IsNullOrEmpty(op)) return "";
                // handle multi-char ops first
                op = op.Replace(">=", "%3E%3D").Replace("<=", "%3C%3D");
                // then single-char
                op = op.Replace(">", "%3E").Replace("<", "%3C").Replace("=", "%3D");
                return op;
            }


            // cmc
            if (cmcOperatorComboBox.SelectedIndex > -1)
            {
                var op = EncodeOperator(cmcOperatorComboBox.Text.Trim());
                queryClauses.Add($"cmc{op}{cmcNumericUpDown.Value}");
            }

            // power
            if (powerComboBox.SelectedIndex > -1)
            {
                var op = EncodeOperator(powerComboBox.Text.Trim());
                queryClauses.Add($"power{op}{powerNumericUpDown.Value}");
            }

            // toughness
            if (toughnessComboBox.SelectedIndex > -1)
            {
                var op = EncodeOperator(toughnessComboBox.Text.Trim());
                queryClauses.Add($"toughness{op}{toughnessNumericUpDown.Value}");
            }

            // loyalty
            if (loyaltyComboBox.SelectedIndex > -1)
            {
                var op = EncodeOperator(loyaltyComboBox.Text.Trim());
                queryClauses.Add($"loyalty{op}{loyaltyNumericUpDown.Value}");
            }

            // explicit mana cost text (from manaCostTextBox)
            if (!string.IsNullOrWhiteSpace(manaCostTextBox.Text))
            {
                // use the same encoding style as other fields
                queryClauses.Add($"mana%3A{manaCostTextBox.Text}");
            }

            // price filters
            if (pricesCurrencyComboBox.SelectedIndex > -1 && pricesPriceNumericUpDown.Value > 0 && pricesOperatorComboBox.SelectedIndex > -1)
            {
                var currency = pricesCurrencyComboBox.SelectedItem.ToString().ToLower();
                var op = EncodeOperator(pricesOperatorComboBox.Text.Trim());
                queryClauses.Add($"{currency}{op}{pricesPriceNumericUpDown.Value}");
            }

            // artist filter
            if (!string.IsNullOrWhiteSpace(artistTextBox.Text))
                queryClauses.Add($"artist%3A{artistTextBox.Text}");

            // flavor text filter
            if (!string.IsNullOrWhiteSpace(flavorTextTextBox.Text))
                queryClauses.Add($"flavor%3A{flavorTextTextBox.Text}");

            // language filter
            if (languageComboBox.SelectedIndex > -1 && !string.IsNullOrWhiteSpace(languageComboBox.Text))
                queryClauses.Add($"lang%3A{Globals.Methods.AbbreviateLanguage(languageComboBox.Text)}");

            // rarity filter (converted from checked list to four checkboxes)
            var rarities = new List<string>();
            if (rarityCommonCheckBox.Checked) rarities.Add("common");
            if (rarityUncommonCheckBox.Checked) rarities.Add("uncommon");
            if (rarityRareCheckBox.Checked) rarities.Add("rare");
            if (rarityMythicCheckBox.Checked) rarities.Add("mythic");

            if (rarities.Count == 1)
            {
                queryClauses.Add($"rarity%3A{rarities[0]}");
            }
            else if (rarities.Count > 1)
            {
                var parts = rarities.Select(r => $"rarity%3A{r}");
                queryClauses.Add($"({string.Join("+or+", parts)})");
            }

            // game/platform filter (paper / arena / mtgo)
            if ((paperCheckBox.Checked) || (arenaCheckBox.Checked) || (magicOnlineCheckBox.Checked))
            {
                var games = new List<string>();
                if (paperCheckBox.Checked) games.Add("paper");
                if (arenaCheckBox.Checked) games.Add("arena");
                if (magicOnlineCheckBox.Checked) games.Add("mtgo"); // Magic Online
                if (games.Count == 1)
                {
                    queryClauses.Add($"game%3A{games[0]}");
                }
                else if (games.Count > 1)
                {
                    var parts = games.Select(g => $"game%3A{g}");
                    queryClauses.Add($"({string.Join("+or+", parts)})");
                }
            }

            // commander color identity filter (commander buttons)
            var commanderSymbols = "";
            if (commanderWhiteButton.Checked) commanderSymbols += "W";
            if (commanderBlueButton.Checked) commanderSymbols += "U";
            if (commanderBlackButton.Checked) commanderSymbols += "B";
            if (commanderRedButton.Checked) commanderSymbols += "R";
            if (commanderGreenButton.Checked) commanderSymbols += "G";
            if (commanderColorlessButton.Checked) commanderSymbols += "C";
            if (!string.IsNullOrEmpty(commanderSymbols))
            {
                // keep existing commander token
                queryClauses.Add($"commander%3A{commanderSymbols}");
            }

            // color / color-identity parameter controlled by colorsComboBox.
            var colorSymbols = "";
            if (colorsWhiteButton.Checked) colorSymbols += "W";
            if (colorsBlueButton.Checked) colorSymbols += "U";
            if (colorsBlackButton.Checked) colorSymbols += "B";
            if (colorsRedButton.Checked) colorSymbols += "R";
            if (colorsGreenButton.Checked) colorSymbols += "G";
            if (colorsColorlessButton.Checked) colorSymbols += "C";

            if (!string.IsNullOrEmpty(colorSymbols) && colorsComboBox != null && colorsComboBox.SelectedIndex > -1)
            {
                // determine token
                var useColorsToken = colorsComboBox.SelectedIndex == 0;
                var token = useColorsToken ? "colors" : "identity";

                // include optional operator from colorsOperatorComboBox (percent-encoded)
                var colorsOp = "";
                if (colorsOperatorComboBox != null && colorsOperatorComboBox.SelectedIndex > -1)
                {
                    colorsOp = EncodeOperator(colorsOperatorComboBox.Text.Trim());
                }

                queryClauses.Add($"{token}{colorsOp}{colorSymbols}");
            }

            // attributes (from attributesObjectListView). Each item stores the raw attribute text.
            if (attributesObjectListView.Objects != null)
            {
                foreach (var obj in attributesObjectListView.Objects)
                {
                    if (obj is OLVAttributeItem attrItem && !string.IsNullOrWhiteSpace(attrItem.Attribute))
                    {
                        var clause = $"is%3A{attrItem.Attribute.Replace(" ", "")}";
                        if (attrItem.Not) clause = "-" + clause;
                        queryClauses.Add(clause);
                    }
                }
            }

            // If any clauses were built, append them joined by the same 'plus' separator used elsewhere.
            if (queryClauses.Count > 0)
            {
                // if query currently ends with "&q=" and nothing yet, don't add extra plus
                var prefix = query.Length > 0 ? plus : "";
                query += $"{(query.EndsWith("&q=") ? "" : prefix)}{string.Join(plus, queryClauses)}";
            }

            return query;
        }
        private Predicate<object> GetSetNameTreeFilter()
        {
            string boxText = setFilterBox.UserText.ToUpper();
            return x => boxText == ""
                ? true
                : (x is OLVRarityItem rarityItem && (rarityItem.Parent as OLVSetItem).Name.ToUpper().Contains(boxText))
                  || (x is OLVSetItem && (x as OLVSetItem).Name.ToUpper().Contains(boxText));
        }
        public void SortCardListView()
        {
            if (cardListView.PrimarySortColumn == null) // sort by set, number if not already sorted
            {
                var sorted = cardListView.Objects.Cast<OLVCardItem>().OrderBy(x => x.MagicCard.set_name).ThenBy(x => x.MagicCard.SortableNumber);
                cardListView.Objects = sorted;
            }
            else
                cardListView.Sort();
        }
        public void LoadSet(string SetCode)
        {
            var existingSet = setListView.Objects.Cast<OLVSetItem>().FirstOrDefault(x => x.CardSet.code == SetCode);
            var selectedSet = setListView.SelectedObject as OLVSetItem;

            try
            {
                using (var context = new ScryfallCardsDbContext())
                {
                    var dbSet = (from s in context.Sets
                                 where s.code == SetCode
                                 select s).FirstOrDefault();
                    if (dbSet != null)
                    {
                        if (existingSet == null)
                        {
                            var set = new OLVSetItem(dbSet);
                            var cards = from c in context.Catalog
                                        where c.set == SetCode
                                        orderby new AlphaNumericString(c.collector_number), c.Name
                                        select c;

                            foreach (var card in cards)
                            {
                                var invItems = from i in context.LibraryView
                                               where i.set == card.set && i.collector_number == card.collector_number
                                               select i;

                                if (invItems.FirstOrDefault() != null)
                                {
                                    int count = 0;
                                    foreach (var item in invItems)
                                        if (!item.Virtual && item.Count.HasValue)
                                            count += item.Count.Value;
                                    card.CopiesOwned = count;
                                    set.AddCard(card);
                                }
                            }

                            setListView.AddObject(set);
                            if (setListView.Objects.Count() == 1) // first set added, must sort the tree
                                setListView.Sort(setListView.AllColumns[1], SortOrder.Descending);
                            SetItems.Add(set);
                        }
                        else
                        {
                            existingSet.CardSet = dbSet;
                            setListView.RefreshObject(existingSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine(ex.ToString());
            }
        }
        // TODO: refactor
        public delegate void LoadSetsDelegate();
        public void LoadSets()
        {
            if (InvokeRequired)
                BeginInvoke(new LoadSetsDelegate(LoadSets), null);
            else
            {
                var renderer = setListView.TreeColumnRenderer;
                renderer.IsShowLines = false;
                sets = new Dictionary<string, OLVSetItem>();
                try
                {
                    using (var context = new ScryfallCardsDbContext())
                    {
                        var dbSets = from s in context.Sets
                                     orderby s.name
                                     select s;

                        OLVSetItem set;
                        foreach (var dbSet in dbSets)
                        {
                            set = new OLVSetItem(dbSet);
                            var cards = from c in context.Catalog
                                        where c.set == dbSet.code
                                        orderby new AlphaNumericString(c.collector_number), c.Name
                                        select c;

                            foreach (var card in cards)
                            {
                                var invItems = from i in context.LibraryView
                                               where i.set == card.set && i.collector_number == card.collector_number
                                               select i;

                                if (invItems.FirstOrDefault() != null)
                                {
                                    int count = 0;
                                    foreach (var item in invItems)
                                        if (!item.Virtual && item.Count.HasValue)
                                            count += item.Count.Value;
                                    card.CopiesOwned = count;
                                    set.AddCard(card);
                                }
                            }

                            setListView.AddObject(set);
                            //if (setListView.Objects.Count() == 1) // first set added, must sort the tree
                                //setListView.Sort(setListView.AllColumns[1], SortOrder.Descending);
                            SetItems.Add(set);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugOutput.WriteLine(ex.ToString());
                }

                SetItems.AddRange(sets.Values);
            }
        }
        public void LoadTree()
        {
            foreach (var set in sets.Values)
                setListView.AddObject(set);

            setListView.Sort(setListView.AllColumns[1], SortOrder.Descending);
        }
        private void UpdateSetModelFilter()
        {
            setListView.ModelFilter = new ModelFilter(GetSetNameTreeFilter());
        }

        #endregion Methods

        #region Events

        #region DBViewForm Events

        public event EventHandler<CardsActivatedEventArgs> CardsActivated;

        private void OnCardsActivated(CardsActivatedEventArgs args)
        {
            CardsActivated?.Invoke(this, args);
        }

        public event EventHandler<CardSelectedEventArgs> CardSelected;

        private void OnCardSelected(CardSelectedEventArgs args)
        {
            CardSelected?.Invoke(this, args);
        }

        #endregion DBViewForm Events

        #region setListView Events

        private void treeListView1_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.Model is OLVCardItem)
            {
                var padding = new Rectangle { X = -10 };
                e.Item.CellPadding = padding;
            }
        }

        private void treeListView1_ItemActivate(object sender, EventArgs e)
        {
            if (setListView.SelectedObject is OLVSetItem item)
            {
                if (!setListView.IsExpanded(item))
                    setListView.Expand(item);
                else
                    setListView.Collapse(item);
            }
        }

        private void setListView_SelectionChanged(object sender, EventArgs e)
        {
            if (setListView.SelectedObject != SetSelected)
            {
                SetSelected = setListView.SelectedObject;
                cardListView.SelectedObject = null;
                DoScryfallQuery();
            }
        }

        #endregion setListView Events

        #region cardListView Events

        private void fastObjectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cards = cardListView.SelectedObjects;
            if (cards.Count > 0)
                OnCardSelected(new CardSelectedEventArgs { MagicCards = cards });
        }

        private void fastObjectListView1_ItemActivate(object sender, EventArgs e)
        {
            OnCardsActivated(new CardsActivatedEventArgs { CardItems = new ArrayList { cardListView.SelectedObject as OLVCardItem } });
        }

        private void fastObjectListView1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Cursor.Current = new Cursor(Properties.Resources.hand.GetHicon());
        }

        private void cardListView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cardListView.SelectedItems != null)
                if (e.KeyChar == '\r')
                    e.Handled = true;
        }
        private void cardListView_Scroll(object sender, ScrollEventArgs e)
        {
            if (cardListView.Items[cardListView.Items.Count - 1].Bounds.Top < 500)
            {
                FetchMoreResults();
            }
        }
        #endregion cardListView Events

        #region Menu Events

        private void updateThisSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (setListView.SelectedObject is OLVSetItem setItem)
            {
                var newTask = new DownloadSetTask(setItem.CardSet);
                Globals.Forms.TasksForm.TaskManager.AddTask(newTask);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!(setListView.SelectedObject is OLVSetItem))
                e.Cancel = true;
        }

        #endregion Menu Events

        #region Misc Events

        private void setFilterBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSetModelFilter();
        }

        private void whiteManaButton_Click(object sender, EventArgs e)
        {
            DoScryfallQuery();
        }
        private void cardNameFilterBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                DoScryfallQuery();
            }
        }
        private void formatFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoScryfallQuery();
        }
        public void InventoryChanged(object sender, InventoryChangedEventArgs e)
        {
            using (var context = new ScryfallCardsDbContext())
            {
                var setsNeedingRecount = new List<string>();
                foreach (var card in e.Cards)
                {
                    var scryfallCard = card.ToScryfallMagicCard();
                    var invCards = from i in context.LibraryView
                                   where i.ScryfallId == scryfallCard.ScryfallId
                                   select i;
                    int count = 0;
                    foreach (var inv in invCards)
                    {
                        if (inv.Count.HasValue)
                            count += inv.Count.Value;
                    }
                    foreach (OLVCardItem lvCard in cardListView.Objects)
                        if (lvCard.MagicCard.ScryfallId == card.ScryfallId)
                        {
                            lvCard.MagicCard.CopiesOwned = count;
                            cardListView.RefreshObject(lvCard);
                        }
                    if (!setsNeedingRecount.Contains(scryfallCard.set))
                        setsNeedingRecount.Add(scryfallCard.set);
                }
                foreach (var set in setsNeedingRecount)
                {
                    OLVSetItem olvSet = null;
                    foreach (OLVSetItem item in setListView.Objects)
                        if (item.CardSet.code == set)
                        {
                            olvSet = item;
                            break;
                        }
                    if (olvSet != null)
                    {
                        olvSet.Cards.Clear();
                        var cards = from c in context.Catalog
                                    where c.set == set
                                    orderby new AlphaNumericString(c.collector_number), c.Name
                                    select c;

                        foreach (var card in cards)
                        {
                            var invItems = from i in context.LibraryView
                                           where i.set == card.set && i.collector_number == card.collector_number
                                           select i;

                            if (invItems.FirstOrDefault() != null)
                            {
                                int count = 0;
                                foreach (var item in invItems)
                                    if (!item.Virtual && item.Count.HasValue)
                                        count += item.Count.Value;
                                card.CopiesOwned = count;
                                olvSet.AddCard(card);
                            }
                        }
                        setListView.RefreshObject(olvSet);
                    }
                }
            }
        }

        #endregion Misc Events

        #endregion Events

        #region Classes

        public class MyCustomSortingDataSource : FastObjectListDataSource
        {
            public MyCustomSortingDataSource(FastObjectListView listView) : base(listView)
            {
            }

            override public void Sort(OLVColumn column, SortOrder order)
            {
                if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "CollectorNumber"))
                    FilteredObjectList.Sort(new CollectorNumberComparer { SortOrder = order });
                else if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "DisplayName"))
                    FilteredObjectList.Sort(new NameComparer { SortOrder = order });
                else if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "Type"))
                    FilteredObjectList.Sort(new TypeComparer { SortOrder = order });
                else if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "Set"))
                    FilteredObjectList.Sort(new SetComparer { SortOrder = order });
                else if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "ManaCost"))
                    FilteredObjectList.Sort(new ManaCostComparer { SortOrder = order });
                else if (column == listView.AllColumns.FirstOrDefault(x => x.AspectName == "CopiesOwned"))
                    FilteredObjectList.Sort(new CopiesOwnedComparer { SortOrder = order });
                RebuildIndexMap();
            }

            private class CopiesOwnedComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result = (x as OLVCardItem).CopiesOwned.CompareTo((y as OLVCardItem).CopiesOwned);
                    return SortOrder == SortOrder.Ascending ? result : -1 * result;
                }
            }

            private class CollectorNumberComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result = (x as OLVCardItem).SortableNumber.CompareTo((y as OLVCardItem).SortableNumber);
                    return SortOrder == SortOrder.Ascending ? result : -1 * result;
                }
            }

            private class NameComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result = (x as OLVCardItem).Name.CompareTo((y as OLVCardItem).Name);
                    return SortOrder == SortOrder.Ascending ? result : result * -1;
                }
            }

            private class TypeComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result = (x as OLVCardItem).Type.CompareTo((y as OLVCardItem).Type);
                    return SortOrder == SortOrder.Ascending ? result : result * -1;
                }
            }

            private class SetComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result = (x as OLVCardItem).Set.CompareTo((y as OLVCardItem).Set);
                    return SortOrder == SortOrder.Ascending ? result : result * -1;
                }
            }

            private class ManaCostComparer : IComparer
            {
                public SortOrder SortOrder;

                public int Compare(object x, object y)
                {
                    int result;
                    string valueX = (x as OLVCardItem).ManaCost ?? "";
                    string valueY = (y as OLVCardItem).ManaCost ?? "";
                    result = valueX.CompareTo(valueY);
                    return SortOrder == SortOrder.Ascending ? result : result * -1;
                }
            }
        }

        #endregion Classes

        private void FetchMoreResults()
        {
            Globals.Forms.TasksForm.TaskManager.ContinueSearch();
        }
        private void DoScryfallQuery()
        {
            string query = BuildScryfallQuery();
            if (query != "" && Globals.Forms.TasksForm != null)
            {
                Text = $"Catalog | Query: {query.Replace("&", "&&").Replace("%3A", ":")}";
                cardListView.ClearObjects();
                cardListView.EmptyListMsg = "Performing query...";
                var newTask = new ScryfallSearchTask(query);
                Globals.Forms.TasksForm.TaskManager.AddTask(newTask);
            }
        }

        private void cardListView_BeforeSorting(object sender, BeforeSortingEventArgs e)
        {
            e.Handled = true;
            if (!addingToCLV)
            {
                if (e.ColumnToSort != null && (e.ColumnToSort.AspectName == "DisplayName" || e.ColumnToSort.AspectName == "ManaCost" || e.ColumnToSort.AspectName == "Set" || e.ColumnToSort.AspectName == "Price"))
                {
                    cardListView.LastSortColumn = e.ColumnToSort;
                    cardListView.LastSortOrder = e.SortOrder;
                    DoScryfallQuery();
                }
                else
                    e.Canceled = true;
            }
        }

        private void manaCostComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            manaCostTextBox.AppendText(manaCostComboBox.Text.Substring(0, manaCostComboBox.Text.IndexOf(" |")));
        }

        private void attributesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (attributesComboBox.SelectedIndex > -1)
            {
                var item = new OLVAttributeItem();
                var split = attributesComboBox.Text.Split('|');
                item.Attribute = split[0].Trim();
                if (split.Length > 1)
                    item.Description = split[1].Trim();
                attributesObjectListView.AddObject(item);
                attributesObjectListView.AutoResizeColumns();
            }
        }

        private void attributesComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
            if (e.Index > -1)
            {
                var split = attributesComboBox.Items[e.Index].ToString().Split('|');
                string text = split[0].Trim();
                string description = split.Length > 1 ? split[1].Trim() : "";
                var attributeTextSize = TextRenderer.MeasureText(e.Graphics, text, new Font(e.Font, FontStyle.Bold), e.Bounds.Size, TextFormatFlags.Left);
                TextRenderer.DrawText(e.Graphics, text, new Font(e.Font, FontStyle.Bold), e.Bounds, e.ForeColor, TextFormatFlags.Left);
                if (description != "")
                    TextRenderer.DrawText(e.Graphics, description, new Font(e.Font.FontFamily, e.Font.Size - 1), new Rectangle(e.Bounds.X + attributeTextSize.Width, e.Bounds.Y, e.Bounds.Width - attributeTextSize.Width, e.Bounds.Height), e.BackColor.Name != "Window" ? Color.White : Color.Gray, TextFormatFlags.Left);
            }
        }

        private void manaCostTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            manaCostComboBox.Items.Clear();
            if (manaCostTypeComboBox.SelectedIndex == 0) // standard mana symbols
            {
                manaCostComboBox.Items.AddRange(new string[] { "{W} | 1 white mana", "{U} | 1 blue mana", "{B} | 1 black mana", "{R} | 1 red mana", "{G} | 1 green mana", "{C} | 1 colorless mana", "{0} | zero mana", "{S} | 1 snow mana" });
            }
            else if (manaCostTypeComboBox.SelectedIndex == 1) // generic mana symbols
            {
                manaCostComboBox.Items.AddRange(new string[] { "{X} | X generic mana", "{1} | 1 generic mana", "{2} | 2 generic mana", "{3} | 3 generic mana", "{4} | 4 generic mana", "{5} | 5 generic mana", "{6} | 6 generic mana", "{7} | 7 generic mana", "{8} | 8 generic mana", "{9} | 9 generic mana", "{10} | 10 generic mana", "{11} | 11 generic mana", "{12} | 12 generic mana", "{13} | 13 generic mana", "{14} | 14 generic mana", "{15} | 15 generic mana" });
            }
            else if (manaCostTypeComboBox.SelectedIndex == 2) // hybrid mana symbols
            {
                manaCostComboBox.Items.AddRange(new string[] { "{W/U} | 1 white or blue mana", "{W/B} | 1 white or black mana", "{U/B} | 1 blue or black mana", "{U/R} | 1 blue or red mana", "{B/R} | 1 black or red mana", "{B/G} | 1 black or green mana", "{R/G} | 1 red or green mana", "{R/W} | 1 red or white mana", "{G/W} | 1 green or white mana", "{G/U} | 1 green or blue mana", "{2/W} | 2 generic or 1 white mana", "{2/U} | 2 generic or 1 blue mana", "{2/B} | 2 generic or 1 black mana", "{2/R} | 2 generic or 1 red mana", "{2/G} | 2 generic or 1 green mana" });                 
            }
            else if (manaCostTypeComboBox.SelectedIndex == 3) // phyrexian mana symbols
            {
                manaCostComboBox.Items.AddRange(new string[] { "{W/P} | 1 white mana or 2 life", "{U/P} | 1 blue mana or 2 life", "{B/P} | 1 black mana or 2 life", "{R/P} | 1 red mana or 2 life", "{G/P} | 1 green mana or 2 life", "{G/U/P} | 1 green or blue mana or 2 life", "{G/W/P} | 1 green or white mana or 2 life" });
            }
        }
    }
}