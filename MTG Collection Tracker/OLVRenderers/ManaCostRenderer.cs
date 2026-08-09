using BrightIdeasSoftware;
using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace MTG_Librarian
{
    public class ManaCostRenderer : BaseRenderer
    {
        public override void Render(Graphics g, Rectangle r)
        {
            var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
            var rowObj = ListItem.RowObject;
            Image cachedImage = null;
            if (rowObj is InventoryCard card)
                cachedImage = card.ManaCostImage;
            else if (rowObj is InventoryCardCluster cluster)
                cachedImage = cluster.ManaCostImage;
            else if (rowObj is OLVCardItem olvCard)
                cachedImage = olvCard.MagicCard.ManaCostImage;

            if (cachedImage != null && cachedImage.Width == backgroundRect.Width && cachedImage.Height == backgroundRect.Height && !IsItemSelected)
            {
                g.DrawImage(cachedImage, backgroundRect);
                return;
            }

            // Create a bitmap that represents only the cell area. All drawing below is relative to (0,0).
            cachedImage = new Bitmap(Math.Max(1, backgroundRect.Width), Math.Max(1, backgroundRect.Height));
            using (Graphics g2 = Graphics.FromImage(cachedImage))
            {
                g2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Determine background color depending on selection state.
                var backColor = ListItem.BackColor;
                if (IsItemSelected)
                    backColor = ListView.Focused ? ListView.SelectedBackColor : ListView.UnfocusedSelectedBackColor;

                using (var solidBrush = new SolidBrush(backColor))
                {
                    // Fill the entire bitmap (use bitmap-local coordinates).
                    g2.FillRectangle(solidBrush, 0, 0, cachedImage.Width, cachedImage.Height);
                }

                // Extract mana cost string from the row object.
                string manaCost = "";
                if (rowObj is InventoryCard card2)
                {
                    if (card2.mana_cost != null)
                        manaCost = card2.mana_cost;
                    else if (card2.card_faces != null && card2.card_faces.Length > 0)
                        manaCost = card2.card_faces[0].mana_cost;
                }
                else if (rowObj is InventoryCardCluster cluster2)
                {
                    if (cluster2.mana_cost != null)
                        manaCost = cluster2.mana_cost;
                    else if (cluster2.card_faces != null && cluster2.card_faces.Length > 0)
                        manaCost = cluster2.card_faces[0].mana_cost;
                }
                else if (rowObj is OLVCardItem olvCard2)
                {
                    if (olvCard2.MagicCard.mana_cost != null)
                        manaCost = olvCard2.MagicCard.mana_cost;
                    else if (olvCard2.MagicCard.card_faces != null && olvCard2.MagicCard.card_faces.Length > 0)
                        manaCost = olvCard2.MagicCard.card_faces[0].mana_cost;
                }

                if (!string.IsNullOrEmpty(manaCost))
                {
                    var reg = new Regex("{[A-Z0-9/]+}|//");
                    var costParts = reg.Matches(manaCost);
                    var imageList = Globals.ImageLists.SymbolIcons16;

                    // Coordinates are relative to the bitmap (not the control). Start with inner padding.
                    int rectLeft = 10; // for text drawing and measurement (bitmap-local)
                    int left = 5; // for symbol image drawing (bitmap-local)
                    foreach (Match part in costParts)
                    {
                        if (part.Value == "//")
                        {
                            using (Font f = new Font(Font.FontFamily, 10, FontStyle.Regular))
                            {
                                // Draw text relative to bitmap
                                float textY = 3f;
                                g2.DrawString("//", f, Brushes.Black, rectLeft, textY, StringFormat.GenericDefault);
                                int width = (int)Math.Ceiling(g2.MeasureString("//", f).Width);
                                left += width + 10;
                                rectLeft += width + 10;
                            }
                        }
                        else
                        {
                            int? imgIndex = imageList?.Images?.IndexOfKey(part.Value);
                            if (imgIndex.HasValue && imgIndex.Value != -1)
                                imageList.Draw(g2, left, 3, imgIndex.Value);
                            left += 18;
                            rectLeft += 18;
                        }
                    }
                }
            }

            // Draw the generated bitmap into the actual control graphics at the intended location.
            g.DrawImage(cachedImage, backgroundRect);

            if (!IsItemSelected)
            {
                // Cache the bitmap on the row object so future paints can reuse it.
                if (rowObj is InventoryCard card3)
                    card3.ManaCostImage = cachedImage;
                else if (rowObj is InventoryCardCluster cluster3)
                    cluster3.ManaCostImage = cachedImage;
                else if (rowObj is OLVCardItem olvCard3)
                    olvCard3.MagicCard.ManaCostImage = cachedImage;
            }
        }
    }
}
