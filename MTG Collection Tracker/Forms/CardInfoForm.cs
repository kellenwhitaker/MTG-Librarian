using System.Text.RegularExpressions;
using KW.WinFormsUI.Docking;

namespace MTG_Librarian
{
    public partial class CardInfoForm : DockForm
    {
        #region Fields

        private MagicCardBase MagicCard;
        private MagicCardBase DisplayedCard;

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
                var reg = new Regex("{([A-Z0-9/]+)}");
                var costParts = reg.Matches(manaCost);
                foreach (Match part in costParts)
                    imgs += $"<img src='Icons/{part.Groups[1].Value.Replace("/", "")}_16.png'>";
            }
            return imgs;
        }

        private static string GetHTMLForPart(MagicCardBase card)
        {
            string html =
                $"<tr>" +
                $"<td>{ManaCostToImgs(card.manaCost)}</td>" +
                $"</tr>" +
                $"<tr>" +
                $"<td><b>{card.name}</b></td>" +
                $"</tr>" +
                $"<tr>" +
                $"<td><b>{card.type}</b></td>" +
                $"</tr>";
            if (card.power != null && card.toughness != null)
            {
                html += $"<tr>" +
                    $"<td><b>{card.power} / {card.toughness}</b></td>" +
                    $"</tr>";
            }
            html += $"<tr>" +
                $"<td><br><br>{card.text?.Replace("\n", "<br>")}</td>" +
                $"</tr>";
            if (card.flavorText != null)
                html += $"<tr>" +
                    $"<td><br><br><i>{card.flavorText}</i></td>" +
                    $"</tr>";
            html += "<tr><td><br><div style='font-size: 2px;'><hr>&nbsp;</div></td></tr>";
            return html;
        }

        private static string GetHTMLFooter(MagicCardBase card)
        {
            return "<tr>" +
                $"<td><b>{card.Edition} [{card.SetCode.ToUpper()}] - #{card.number}</b></td>" +
                $"</tr>" +
                $"<tr>" +
                $"<td>Artist: {card.artist}</td>" +
                $"</tr>";
        }

        public void CardSelected(MagicCardBase card)
        {
            MagicCard = DisplayedCard = card;
            if (card.layout == "transform")
            {
                flipButton.Visible = true;
                cardTextHtmlPanel.Top = flipButton.Bottom + 5;
            }
            else
            {
                flipButton.Visible = false;
                cardTextHtmlPanel.Top = cardPictureBox.Bottom + 5;
            }
            string html = $"<table width='100%'>" + GetHTMLForPart(card);
            if (card.PartB != null)
                html += GetHTMLForPart(card.PartB);

            html += GetHTMLFooter(card) + "</table>";
            cardTextHtmlPanel.Text = html;
        }

        #endregion Methods

        #region Events

        public void cardImageRetrieved(object sender, CardImageRetrievedEventArgs e)
        {
            if (e.uuid == DisplayedCard.uuid)
                cardPictureBox.Image = e.CardImage.ScaleImage(cardPictureBox.Width, cardPictureBox.Height);
        }

        #endregion Events

        private void flipButton_Click(object sender, System.EventArgs e)
        {
            if (DisplayedCard == MagicCard)
            {
                DisplayedCard = MagicCard.PartB;
                CardManager.RetrieveImage(MagicCard.PartB);
            }
            else
            {
                DisplayedCard = MagicCard;
                CardManager.RetrieveImage(MagicCard);
            }
        }
    }
}