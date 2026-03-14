using System.Text.RegularExpressions;
using System.Windows.Forms;
using KW.WinFormsUI.Docking;
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

        public void CardSelected(ScryfallMagicCardBase card)
        {
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
    }
}