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
            cardTextHtmlPanel.Text = $"<b>{card.name}<br>{card.type}</b><br><br>{card.text.Replace("\n", "<br>")}<br><br><i>{card.flavorText}</i>";
        }
    }
}
