using KW.WinFormsUI.Docking;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
// TODO: Cleanup
namespace MTG_Librarian
{
    public partial class CardInfoForm : DockForm
    {
        #region Fields

        private ScryfallMagicCardBase MagicCard;
        private ScryfallMagicCardBase DisplayedCard;
        private string DisplayedSide = "A";

        #endregion Fields

        #region Constructors

        public CardInfoForm()
        {
            InitializeComponent();
            printingsListView.SmallImageList = Globals.ImageLists.SmallIconList;
            setColumn.ImageGetter = delegate (object row) { return (row as dynamic).ImageKey; };
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockBottom;
        }

        #endregion Constructors

        #region Methods

        private static string ManaCostToImgs(string manaCost)
        {
            string imgs = "";
            if (manaCost != null && manaCost != "")
            {
                var reg = new Regex("{([A-Z0-9/]+)}|//");
                var costParts = reg.Matches(manaCost);
                foreach (Match part in costParts)
                {
                    if (part.Value == "//")
                        imgs += "<img src='Icons/doubleslash.png'>";
                    else
                        imgs += $"<img src='Icons/{part.Groups[1].Value.Replace("/", "")}_16.png'>";
                }
            }
            return imgs;
        }

        private static string FormatFace(string power, string toughness, string text, string flavorText, string artist)
        {
            string html = "";
            if (power != null && toughness != null)
            {
                html += $"<tr>" +
                    $"<td><b>{power} / {toughness}</b></td>" +
                    $"</tr>";
            }
            html += $"<tr>" +
                $"<td><br><br>{text?.Replace("\n", "<br>")}</td>" +
                $"</tr>";

            if (flavorText != null)
                html += $"<tr>" +
                    $"<td><br><br><i>{flavorText}</i></td>" +
                    $"</tr>";
            html += $"<tr>" +
                    $"<td><br><br><b>Artist: {artist}</b></td>" +
                    $"</tr>";

            return html;
        }
        private static string GetHTMLForFace(ScryfallCardFace face)
        {
            string html =
                            $"<tr>" +
                            $"<td>{ManaCostToImgs(face.mana_cost)}</td>" +
                            $"</tr>" +
                            $"<tr>" +
                            $"<td><b>{face.DisplayName}</b></td>" +
                            $"</tr>" +
                            $"<tr>" +
                            $"<td><b>{face.DisplayTypeLine}</b></td>" +
                            $"</tr>";
            html += FormatFace(face.power, face.toughness, face.DisplayText, face.flavor_text, face.artist);
            html += "<tr><td><div style='font-size: 2px;'><hr>&nbsp;</div></td></tr>";
            return html;
        }
        private static string GetHTMLForPart(ScryfallMagicCardBase card)
        {
            string html =
                $"<tr>" +
                $"<td>{ManaCostToImgs(card.mana_cost)}</td>" +
                $"</tr>" +
                $"<tr>" +
                $"<td><b>{card.DisplayName}</b></td>" +
                $"</tr>" +
                $"<tr>" +
                $"<td><b>{card.DisplayTypeLine}</b></td>" +
                $"</tr>";
            if (card.card_faces == null)
            {
                html += FormatFace(card.power, card.toughness, card.DisplayText, card.flavor_text, card.artist);
                html += "<tr><td><div style='font-size: 2px;'><hr>&nbsp;</div></td></tr>";
            }
            else
            {
                foreach (ScryfallCardFace face in card.card_faces)
                {
                    html += FormatFace(face.power, face.toughness, face.oracle_text, face.flavor_text, face.artist);
                    html += "<tr><td><div style='font-size: 2px;'><hr>&nbsp;</div></td></tr>";
                }
            }
            return html;
        }

        private static string GetHTMLFooter(ScryfallMagicCardBase card)
        {
            return "<tr>" +
                $"<td><b>{card.set_name} [{card.set.ToUpper()}] - #{card.collector_number}</b></td>" +
                $"</tr>";
        }

        public void RulingsDownloaded(ScryfallMagicCardBase card)
        {
            if (card == MagicCard)
                FillRulingsPanel();
        }

        public void PrintingsDownloaded(ScryfallMagicCardBase card)
        {
            if (card == MagicCard)
                FillPrintingsPanel();
        }
        public void CardSelected(ScryfallMagicCardBase card)
        {
            var oldCard = MagicCard;
            MagicCard = DisplayedCard = card;
            DisplayedSide = "A";
            string html = "<table width='100%'>";
            if (card.layout == "transform")
            {
                flipButton.Visible = true;
                cardTextHtmlPanel.Top = flipButton.Bottom + 5;
                html += GetHTMLForFace(card.card_faces[0]);
            }
            else
            {
                flipButton.Visible = false;
                cardTextHtmlPanel.Top = cardPictureBox.Bottom + 5;
                html += GetHTMLForPart(card);
            }

            html += GetHTMLFooter(card) + "</table>";
            cardTextHtmlPanel.Text = html;

            legalitiesListView.ClearObjects();
            foreach (var legality in card.legalities)
            {
                var Format = char.ToUpper(legality.Key[0]) + legality.Key.Substring(1);
                var Legality = (char.ToUpper(legality.Value[0]) + legality.Value.Substring(1)).Replace("_", " ");
                var item = new FormatLegalitiesRow { Format = Format, Legality = Legality };
                legalitiesListView.AddObject(item);
            }
            legalitiesListView.AutoResizeColumns();

            attributesListView.ClearObjects();
            var attributesDictionary = new Dictionary<string, string>
            {
                { "Flavor name", card.flavor_name },
                { "Type", card.type_line },
                { "Mana Cost", card.mana_cost },
                { "CMC", card.cmc.ToString() },
                { "Power/Toughness", (card.power != null && card.toughness != null) ? $"{card.power}/{card.toughness}" : "" },
                { "Loyalty", card.loyalty },
                { "Colors", (card.colors != null) ? string.Join(", ", card.colors) : "" },
                { "Color Identity", (card.color_identity != null) ? string.Join(", ", card.color_identity) : "" },
                { "Color indicator", (card.color_indicator != null) ? string.Join(", ", card.color_indicator) : "" },
                { "Rarity", char.ToUpper(card.rarity[0]) + card.rarity.Substring(1) },
                { "Set Name", card.set_name },
                { "Set Code", card.set.ToUpper() },
                { "Collector Number", card.collector_number },
                { "Artist", card.artist },
                { "Language", card.lang },
                { "Released at", card.released_at },
                { "Layout", card.layout },
                { "Keywords", (card.keywords != null) ? string.Join(", ", card.keywords) : "" },
                { "Games", (card.games != null) ? string.Join(", ", card.games) : "" },
                { "Reserved", card.reserved.ToString() },
                { "Finishes", (card.finishes != null) ? string.Join(", ", card.finishes) : "" },
                { "Oversized", card.oversized.ToString() },
                { "Promo", card.promo.ToString() },
                { "Promo types", (card.promo_types != null) ? string.Join(", ", card.promo_types) : "" },
                { "Reprint", card.reprint.ToString() },
                { "Variation", card.variation.ToString() },
                { "Set type", card.set_type },
                { "Security stamp", card.security_stamp },
                { "Digital", card.digital.ToString() },
                { "Border color", card.border_color },
                { "Frame version", card.frame_version },
                { "Frame effects", (card.frame_effects != null) ? string.Join(", ", card.frame_effects) : "" },
                { "Full art", card.full_art.ToString() },
                { "Textless", card.textless.ToString() },
                { "Booster", card.booster.ToString() },
                { "Story spotlight", card.story_spotlight.ToString() },
                { "Edhrec rank", card.edhrec_rank.ToString() },
                { "Penny rank", card.penny_rank.ToString() },
                { "Attraction lights", (card.attraction_lights != null) ? string.Join(", ", card.attraction_lights) : "" },
                { "Watermark", card.watermark },
                { "Defense", card.defense },
                { "Game Changer", card.game_changer.ToString() },
                { "Hand modifier", card.hand_modifier },
                { "Life modifier", card.life_modifier },
                { "Produced mana", (card.produced_mana != null) ? string.Join(", ", card.produced_mana) : "" }
            };
            attributesListView.AddObjects(attributesDictionary);
            attributesListView.AutoResizeColumns();
            if (oldCard == null || oldCard.ScryfallId != MagicCard.ScryfallId)
            {
                if (tabControl.SelectedTab == rulingsTabPage)
                {
                    FillRulingsPanel();
                    if (card.rulings == null)
                    {
                        var task = new DownloadRulingsTask(card);
                        task.AddFirst = true;
                        Globals.Forms.TasksForm.TaskManager.AddTask(task);
                    }
                }
                else if (tabControl.SelectedTab == printingsTabPage)
                {
                    FillPrintingsPanel();
                    if (card.printings == null)
                    {
                        var task = new DownloadPrintingsTask(card);
                        task.AddFirst = true;
                        Globals.Forms.TasksForm.TaskManager.AddTask(task);
                    }
                }
            }
        }
        private void FillPrintingsPanel()
        {
            printingsListView.ClearObjects();
            if (MagicCard.printings != null)
            {
                if (MagicCard.printings.Count > 0)
                {
                    foreach (var card in MagicCard.printings)
                    {
                        var DefaultCurrency = SettingsManager.ApplicationSettings.DefaultCurrency;
                        string priceString;
                        double? price = null;
                        string finish = "nonfoil";
                        if (card.finishes.Count() == 1)
                            finish = card.finishes[0];
                        if (card.prices.TryGetValue($"{DefaultCurrency.ToLower()}{((finish != "nonfoil") ? $"_{finish}" : "")}", out priceString) && !string.IsNullOrEmpty(priceString))
                            price = Convert.ToDouble(priceString);
                        string SymbolCode = card.set != null && card.set.Length == 4 && (card.set_type == "token" || card.set_type == "promo" || card.set_type == "memorabilia") ? card.set.Substring(1) : card.set;
                        var ImageKey = $"{SymbolCode}: {card.rarity}";
                        printingsListView.AddObject(new { ImageKey, card.set_name, card.collector_number, Price = price });
                    }
                    printingsListView.AutoResizeColumns();
                }
                else
                {
                    printingsListView.EmptyListMsg = "No printings found for this card.";
                }
            }
            else
            {
                printingsListView.EmptyListMsg = "Printings not downloaded yet.";
            }
        }
        private void FillRulingsPanel()
        {
            if (MagicCard.rulings != null)
            {
                if (MagicCard.rulings.Count > 0)
                {
                    var html = "<table width='100%'>";
                    foreach (var ruling in MagicCard.rulings)
                    {
                        html += $"<tr>" +
                            $"<td valign='top'><b>{ruling.published_at}</b></td><td valign='top'>{ruling.comment}<br><br></td>" +
                            $"</tr>";
                    }
                    html += "</table>";
                    rulingsHtmlPanel.Text = html;
                }
                else
                {
                    rulingsHtmlPanel.Text = "No rulings found for this card.";
                }
            }
            else
            {
                rulingsHtmlPanel.Text = "Rulings not downloaded yet.";
            }
        }

        #endregion Methods

        #region Events

        public void cardImageRetrieved(object sender, CardImageRetrievedEventArgs e)
        {
            if (e.uuid == DisplayedCard.ScryfallId)
                cardPictureBox.Image = e.CardImage.ScaleImage(cardPictureBox.Width, cardPictureBox.Height);
        }

        private void flipButton_Click(object sender, System.EventArgs e)
        {
            string html = "<table width='100%'>";
            if (DisplayedSide == "A")
            {
                DisplayedSide = "B";
                html += GetHTMLForFace(MagicCard.card_faces[1]);
            }
            else
            {
                DisplayedSide = "A";
                html += GetHTMLForFace(MagicCard.card_faces[0]);
            }

            html += GetHTMLFooter(MagicCard);
            cardTextHtmlPanel.Text = html;
            CardManager.RetrieveImage(MagicCard, DisplayedSide);
        }

        #endregion Events

        private void CardInfoForm_Resize(object sender, System.EventArgs e)
        {
            cardTextHtmlPanel.Width = Width - 20;
        }

        private void legalitiesListView_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            if ((e.Model as FormatLegalitiesRow).Legality == "Legal" || (e.Model as FormatLegalitiesRow).Legality == "Restricted")
            {
                e.Item.BackColor = System.Drawing.Color.LightSteelBlue;
                e.Item.Font = new System.Drawing.Font(e.Item.Font, System.Drawing.FontStyle.Bold);
            }
        }
        private class FormatLegalitiesRow
        {
            public string Format { get; set; }
            public string Legality { get; set; }
        }

        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (tabControl.SelectedTab == rulingsTabPage && MagicCard != null)
            {
                FillRulingsPanel();
                if (MagicCard.rulings == null)
                {
                    var task = new DownloadRulingsTask(MagicCard);
                    task.AddFirst = true;
                    Globals.Forms.TasksForm.TaskManager.AddTask(task);
                }
            }
            else if (tabControl.SelectedTab == printingsTabPage && MagicCard != null)
            {
                FillPrintingsPanel();
                if (MagicCard.printings == null)
                {
                    var task = new DownloadPrintingsTask(MagicCard);
                    task.AddFirst = true;
                    Globals.Forms.TasksForm.TaskManager.AddTask(task);
                }
            }
        }

        public void DefaultCurrencyChanged(object sender, EventArgs e)
        {
            if (MagicCard != null)
                FillPrintingsPanel();
        }
    }
}