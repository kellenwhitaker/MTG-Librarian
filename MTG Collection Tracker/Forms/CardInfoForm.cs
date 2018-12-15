using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
//TODO: improve formatting with long mana costs (Chromium, Progenitus, B.F.M.)
namespace MTG_Collection_Tracker
{
    public partial class CardInfoForm : DockContent
    {
        public string CardFocusedUuid { get; set; }

        public CardInfoForm()
        {
            InitializeComponent();
            MainForm.CardImageRetrieved += cardImageRetrieved;
        }

        private void cardImageRetrieved(object sender, CardImageRetrievedEventArgs e)
        {
            if (e.uuid == CardFocusedUuid)
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
                $"<td width='60%' align='top'><b>{card.name}</b></td><td align='top'>{ManaCostToImgs(card.manaCost)}</td>" +
                $"</tr>" +
                $"<tr>" +
                $"<td colspan=2><b>{card.type}</b></td>" +
                $"</tr>";
            if (card.power != null && card.toughness != null)
            {
                html += $"<tr>" +
                    $"<td><b>{card.power} / {card.toughness}</b></td>" +
                    $"</tr>";
            }
            html += $"<tr>" +
                $"<td colspan=2><br><br>{card.text.Replace("\n", "<br>")}</td>" +
                $"</tr>";
            if (card.flavorText != null)
                html += $"<tr>" +
                    $"<td colspan=2><br><br><i>{card.flavorText}</i></td>" +
                    $"</tr>";
            html += "<tr>" +
                $"<td colspan=2><hr><b>{card.Edition} [{card.SetCode.ToUpper()}] - #{card.number}</b></td>" +
                $"</tr>" +
                $"<tr>" +
                $"<td colspan=2>Artist: {card.artist}</td>" +
                $"</tr>";
            html += "</table>";
            cardTextHtmlPanel.Text = html;
        }
    }
}
