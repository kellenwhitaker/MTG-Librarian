using BrightIdeasSoftware;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public class CardInstanceNameRenderer : BaseRenderer
    {
        private void FillBackground(Graphics g, Rectangle backgroundRect)
        {
            if (ListItem.RowObject is InventoryTotalsItem || ListItem.RowObject is OLVMessageRow)
            {
                using (var solidBrush = new SolidBrush(ListItem.BackColor))
                {
                    g.FillRectangle(Brushes.DodgerBlue, backgroundRect);
                }
                return;
            }
            if (IsItemSelected)
            {
                if (ListView.Focused)
                    using (var solidBrush = new SolidBrush(ListView.SelectedBackColor))
                    {
                        g.FillRectangle(solidBrush, backgroundRect);
                    }
                else
                    using (var solidBrush = new SolidBrush(ListView.UnfocusedSelectedBackColor))
                    {
                        g.FillRectangle(solidBrush, backgroundRect);
                    }
            }
            else
                using (var solidBrush = new SolidBrush(ListView.BackColor))
                {
                    g.FillRectangle(solidBrush, backgroundRect);
                }
        }

        public override void Render(Graphics g, Rectangle r)
        {
            if (ListItem.RowObject is InventoryCard || ListItem.RowObject is OLVCardItem)
            {
                bool isReserved = false;
                int? imgIndex = null;
                string imageKey = null;
                Image cachedImage = null;

                if (ListItem.RowObject is InventoryCard cardInstance)
                {
                    cachedImage = cardInstance.CardNameImage;
                    imageKey = cardInstance.ImageKey;
                    imgIndex = ListView.SmallImageList?.Images?.IndexOfKey(cardInstance.ImageKey);
                    if (cardInstance.reserved)
                        isReserved = true;
                }
                else if (ListItem.RowObject is InventoryCardCluster cluster)
                {
                    cachedImage = cluster.CardNameImage;
                    imageKey = cluster.ImageKey;
                    imgIndex = ListView.SmallImageList?.Images?.IndexOfKey(cluster.ImageKey);
                    if (cluster.reserved)
                        isReserved = true;
                }
                else if (ListItem.RowObject is OLVCardItem cardItem)
                {
                    cachedImage = cardItem.MagicCard.CardNameImage;
                    imageKey = cardItem.ImageKey;
                    imgIndex = ListView.SmallImageList?.Images?.IndexOfKey(cardItem.ImageKey);
                    if (cardItem.MagicCard.reserved)
                        isReserved = true;
                }

                var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
                if (cachedImage != null && cachedImage.Width == backgroundRect.Width && cachedImage.Height == backgroundRect.Height && !IsItemSelected)
                {
                    g.DrawImage(cachedImage, backgroundRect);
                    return;
                }

                cachedImage = new Bitmap(Math.Max(1, backgroundRect.Width), Math.Max(1, backgroundRect.Height));
                using (Graphics g2 = Graphics.FromImage(cachedImage))
                {
                    g2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    int imgWidth = ListView.SmallImageList.ImageSize.Width;
                    var bmpRect = new Rectangle(0, 0, cachedImage.Width, cachedImage.Height);
                    FillBackground(g2, bmpRect);
                    var fontColor = IsItemSelected && ListView.Focused ? Brushes.White : Brushes.Black;
                    var textLeft = 2;

                    if (imgIndex.HasValue && imgIndex.Value != -1)
                    {
                        ListView.SmallImageList.Draw(g2, 2, 2, imgIndex.Value);
                        textLeft += imgWidth;
                        if (isReserved)
                        {
                            g2.DrawImage(Properties.Resources.reserved_27, imgWidth + 2, 2, 20, 20);
                            textLeft += 22;
                        }
                        g2.DrawString(GetText(), Font, fontColor, textLeft, 5);
                    }
                    else
                    {
                        if (isReserved)
                        {
                            g2.DrawImage(Properties.Resources.reserved_27, imgWidth + 2, 2, 20, 20);
                            textLeft += 22;
                        }
                        g2.DrawString(GetText(), Font, fontColor, textLeft, 5);
                    }
                }
                g.DrawImage(cachedImage, backgroundRect);

                // Cache the bitmap on the row object so future paints can reuse it.
                if (!IsItemSelected)
                {
                    if (ListItem.RowObject is InventoryCard card3)
                        card3.CardNameImage = cachedImage;
                    else if (ListItem.RowObject is InventoryCardCluster cluster3)
                        cluster3.CardNameImage = cachedImage;
                    else if (ListItem.RowObject is OLVCardItem olvCard3)
                        olvCard3.MagicCard.CardNameImage = cachedImage;
                }
            }
            else if (ListItem.RowObject is InventoryTotalsItem || ListItem.RowObject is OLVMessageRow)
            {
                using (var boldFont = new Font(Font, FontStyle.Bold))
                {
                    var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
                    FillBackground(g, backgroundRect);
                    var fontColor = Brushes.White;
                    g.DrawString(GetText(), boldFont, fontColor, r.Left + 5, r.Top + 3);
                }
            }
        }
    }
}
