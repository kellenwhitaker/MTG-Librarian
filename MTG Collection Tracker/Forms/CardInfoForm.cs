using System.Text.RegularExpressions;
using KW.WinFormsUI.Docking;

namespace MTG_Librarian
{
    public partial class CardInfoForm : DockContent
    {
        public CardInfoForm()
        {
            InitializeComponent();
            MainForm.CardImageRetrieved += cardImageRetrieved;
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockBottom;
        }

        private void cardImageRetrieved(object sender, CardImageRetrievedEventArgs e)
        {
            if (e.uuid == Globals.States.CardFocusedUuid)
                pictureBox1.Image = e.CardImage.ScaleImage(pictureBox1.Width, pictureBox1.Height);
        }

        private string ManaCostToImgs(string manaCost)
        {
            string imgs = "";
            if (manaCost != null && manaCost != "")
            {
                Regex reg = new Regex("{([A-Z0-9/]+)}");
                var costParts = reg.Matches(manaCost);
                foreach (Match part in costParts)
                {
                    imgs += $"<img src='Icons/{part.Groups[1].Value.Replace("/", "")}_16.png'>";
                }
            }
            return imgs;
        }

        public void CardSelected(MagicCardBase card)
        {
            string html = $"<table width='100%'>" +
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
                $"<td><br><br>{card.text.Replace("\n", "<br>")}</td>" +
                $"</tr>";
            if (card.flavorText != null)
                html += $"<tr>" +
                    $"<td><br><br><i>{card.flavorText}</i></td>" +
                    $"</tr>";
            html += "<tr>" +
                $"<td><hr><b>{card.Edition} [{card.SetCode.ToUpper()}] - #{card.number}</b></td>" +
                $"</tr>" +
                $"<tr>" +
                $"<td>Artist: {card.artist}</td>" +
                $"</tr>";
            html += "</table>";
            cardTextHtmlPanel.Text = html;
        }
    }
}
