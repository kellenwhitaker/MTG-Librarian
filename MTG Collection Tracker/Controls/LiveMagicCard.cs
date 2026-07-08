using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public partial class LiveMagicCard : UserControl
    {
        private List<ScryfallMagicCardBase > Cards { get; set; }
        public string type_line => Cards != null && Cards.Count > 0 ? Cards[0].type_line : string.Empty;
        public string CardName => Cards != null && Cards.Count > 0 ? Cards[0].Name : string.Empty;
        public ScryfallMagicCardBase GetCard() => Cards != null && Cards.Count > 0 ? Cards[0] : null;
        public int Count => Cards != null ? Cards.Count : 0;        
        private bool tapped = false;
        private Image untappedImage;
        public bool Tapped 
        {
            get { return tapped; }
            set 
            { 
                tapped = value;
                if (tapped)
                {
                    untappedImage = pictureBox.Image;
                    var copy = pictureBox.Image.GetCopyOf();
                    copy.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    pictureBox.Image = copy;
                    pictureBox.Width = copy.Width;
                    pictureBox.Top = Height - copy.Height - 2;
                    Width = copy.Width;
                }
                else
                {
                    if (untappedImage != null)
                    {
                        pictureBox.Image = untappedImage;
                        pictureBox.Width = untappedImage.Width;
                        pictureBox.Top = 0;
                        Width = untappedImage.Width;
                    }
                }
            }
        }
        public GameZone Zone { get; set; } = GameZone.Library;
        private void UpdateTextOverlay()
        {
            bool isPositive = p1p1Counters > 0;
            var p1p1Label = string.Empty;
            var countLabel = Count > 1 ? $"x{Count}" : string.Empty;
            var countersLabel = Counters != 0 ? $"Counters: {Counters}" : string.Empty;
            if (p1p1Counters != 0)
                p1p1Label = isPositive ? $"+{p1p1Counters} / +{p1p1Counters}" : $"{p1p1Counters} / {p1p1Counters}";
            if (string.IsNullOrEmpty(countLabel) && string.IsNullOrEmpty(p1p1Label) && string.IsNullOrEmpty(countersLabel))
            {
                pictureBox.Image = CardImageCache.GetScaledImage(Cards[0].ScryfallId, Cards[0].set_name, pictureBox.Width, pictureBox.Height);
                return;
            }
            
            var copy = CardImageCache.GetScaledImage(Cards[0].ScryfallId, Cards[0].set_name, pictureBox.Width, pictureBox.Height).GetCopyOf();
            using (Graphics g = Graphics.FromImage(copy))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                Font font = new Font("Arial", 12, FontStyle.Bold);
                Brush brush = Brushes.White;

                if (!string.IsNullOrEmpty(p1p1Label))
                {
                    DrawLabel(g, p1p1Label, font, brush, new PointF(100, 180));
                }
                if (!string.IsNullOrEmpty(countLabel))
                {
                    DrawLabel(g, countLabel, font, brush, new PointF(10, 30));
                }
                if (!string.IsNullOrEmpty(countersLabel))
                {
                    DrawLabel(g, countersLabel, font, brush, new PointF(10, 30));
                }
            }
            if (!Tapped)
                pictureBox.Image = copy;
            else
            {
                untappedImage = copy;
                var rotatedCopy = copy.GetCopyOf();
                rotatedCopy.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox.Image = rotatedCopy; 
            }
        }
        private void DrawLabel(Graphics g, string text, Font font, Brush brush, PointF position)
        {
            var textSize = g.MeasureString(text, font);
            var textRect = new RectangleF(position.X, position.Y, textSize.Width, textSize.Height);
            g.FillRectangle(Brushes.Black, textRect);
            g.DrawString(text, font, brush, position);
        }

        public void ClearCounters()
        {
            P1P1Counters = 0;
            Counters = 0;
        }
        private int p1p1Counters = 0;
        public int P1P1Counters 
        {
            get { return p1p1Counters; }
            set 
            { 
                p1p1Counters = value;
                UpdateTextOverlay();
            }
        }
        private int counters = 0;
        public int Counters 
        {
            get { return counters; }
            set 
            {
                counters = value;
                UpdateTextOverlay();
            } 
        }
        public bool CanStack => Counters == 0 && P1P1Counters == 0 && !Tapped;
        public enum GameZone
        {
            Library,
            Hand,
            Battlefield,
            Graveyard,
            Exile
        }
        public LiveMagicCard()
        {
            InitializeComponent();
        }

        public void HideButtons()
        {
            playButton.Visible = discardButton.Visible = false;
            pictureBox.Top = 0;
            Height = pictureBox.Height + 1;
        }

        public void ShowButtons()
        {
            playButton.Visible = discardButton.Visible = true;
            pictureBox.Top = 26;
            Height = pictureBox.Height + 26;
        }
        public LiveMagicCard(ScryfallMagicCardBase card)
        {
            InitializeComponent();
            this.Cards = new List<ScryfallMagicCardBase> { card };
            pictureBox.Image = CardImageCache.GetScaledImage(card.ScryfallId, card.set_name, pictureBox.Width, pictureBox.Height);
        }

        public void AddCard(ScryfallMagicCardBase card)
        {
            if (Cards == null)
            {
                Cards = new List<ScryfallMagicCardBase>();
            }
            Cards.Add(card);
            UpdateTextOverlay();
        }

        public ScryfallMagicCardBase RemoveCard()
        {
            if (Cards == null || Cards.Count == 0)
                return null;
            var removedCard = Cards[0];
            Cards.RemoveAt(0);
            UpdateTextOverlay();
            return removedCard;
        }
    }
}
