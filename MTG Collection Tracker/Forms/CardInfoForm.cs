using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MTG_Collection_Tracker
{
    public partial class CardInfoForm : DockContent
    {
        internal string CardFocusedUuid { get; set; }

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

        internal void CardSelected(MagicCardBase card)
        {
            string html = $"<table width='100%'>" +
                $"<tr>" +
                $"<td width='70%'><b>{card.name}</b></td><td>{card.manaCost}</td>" +
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
