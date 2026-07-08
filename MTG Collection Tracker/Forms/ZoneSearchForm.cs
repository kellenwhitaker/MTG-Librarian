using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public partial class ZoneSearchForm : Form
    {
        private PictureBox cardZoomPictureBox = new PictureBox();
        private List<LiveMagicCard> cards;
        public List<LiveMagicCard> Cards 
        {
            get { return cards; }
            set 
            { 
                cards = value;
                cardsPanel.Controls.Clear();
                int index = 0;
                foreach (var card in cards)
                {
                    card.Location = new Point(index * (card.Width + 10), 0);
                    card.HideButtons();
                    card.pictureBox.MouseDown += liveCardMouseDown;
                    card.pictureBox.MouseUp += liveCardMouseUp;
                    cardsPanel.Controls.Add(card);
                    index++;
                }
            }
        }
        public ZoneSearchForm()
        {
            InitializeComponent();
        }

        public void RemoveCard(LiveMagicCard liveCard)
        {
            cardsPanel.Controls.Remove(liveCard);
            cards.Remove(liveCard);
            int index = 0;
            foreach (var card in cards)
            {
                card.Location = new Point(index * (card.Width + 10), 0);
                index++;
            }
        }
        private void liveCardMouseUp(object sender, MouseEventArgs e)
        {            
            Controls.Remove(cardZoomPictureBox);
            cardZoomPictureBox.Image = null;
        }
        private void liveCardMouseDown(object sender, MouseEventArgs e)
        {
            var liveCard = (LiveMagicCard)((PictureBox)sender).Parent;
            if (e.Button == MouseButtons.Left)
            {
                cardZoomPictureBox.Width = (int)(liveCard.pictureBox.Width * 1.5);
                cardZoomPictureBox.Height = (int)(liveCard.pictureBox.Height * 1.5);
                cardZoomPictureBox.Location = this.PointToClient(liveCard.Parent.PointToScreen(liveCard.Location));
                if (cardZoomPictureBox.Top + cardZoomPictureBox.Height > this.Height)
                    cardZoomPictureBox.Top = this.Height - cardZoomPictureBox.Height - 30;

                var card = liveCard.GetCard();
                cardZoomPictureBox.Image = CardImageCache.GetScaledImage(card.ScryfallId, card.set_name, cardZoomPictureBox.Width, cardZoomPictureBox.Height);
                cardZoomPictureBox.Height -= 3;
                Controls.Add(cardZoomPictureBox);
                cardZoomPictureBox.BringToFront();
            }
        }
        public ZoneSearchForm(List<LiveMagicCard> cards)
        {
            InitializeComponent();
            this.Cards = cards;
            cardsPanel.Controls.AddRange(cards.ToArray());
            int index = 0;
            foreach (var card in cards)
            {
                card.Location = new Point(index * (card.Width + 10), 0);
                card.HideButtons();
                card.pictureBox.MouseDown += liveCardMouseDown;
                card.pictureBox.MouseUp += liveCardMouseUp;
                index++;
            }
        }

        private void ZoneSearchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var card in Cards)
            {
                card.pictureBox.MouseDown -= liveCardMouseDown;
                card.pictureBox.MouseUp -= liveCardMouseUp;
            }
        }
    }
}
