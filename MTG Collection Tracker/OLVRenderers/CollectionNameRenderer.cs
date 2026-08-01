using BrightIdeasSoftware;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static BrightIdeasSoftware.TreeListView;

namespace MTG_Librarian
{
    public class CollectionNameRenderer : TreeRenderer
    {
        public override void Render(Graphics g, Rectangle r)
        {
            // High quality rendering
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            var treeView = ListView as TreeListView;
            var item = ListItem.RowObject as NavigatorItem;
            string text = item?.Text ?? string.Empty;
            const int margin = 3;

            var backgroundRect = new Rectangle(r.Left - 1, r.Top, r.Width + 1, r.Height + 1);

            // Clear background (keeps original behaviour of white background)
            g.FillRectangle(Brushes.White, backgroundRect);

            var contentRect = new Rectangle
            {
                X = backgroundRect.Left,
                Y = backgroundRect.Y + 3,
                Width = backgroundRect.Width,
                Height = backgroundRect.Height
            };

            // Handle group arrow and collection indent
            if (ListItem.RowObject is NavigatorGroup navigatorGroup)
            {
                if (ListItem.SubItems.Count > 0)
                {
                    var arrowRect = new Rectangle(backgroundRect.Left, backgroundRect.Top + 5, 11, backgroundRect.Height);
                    contentRect.X = arrowRect.Right + margin;
                    DrawExpandCollapseArrow(g, arrowRect, treeView != null && treeView.IsExpanded(navigatorGroup));
                }
            }
            else if (ListItem.RowObject is NavigatorCollection)
            {
                contentRect.X += 25;
            }

            var textRect = new Rectangle(contentRect.Left + margin, contentRect.Top - 7, contentRect.Width - margin, contentRect.Height);

            // Selection background
            if (IsItemSelected)
            {
                var fillRect = new Rectangle(textRect.Left, textRect.Top + 6, textRect.Width, textRect.Height - 6);
                DrawSelectionFill(g, fillRect);
            }

            using (var boldFont = new Font(Font, FontStyle.Bold))
            {
                SizeF textSize;
                // Group header band
                if (ListItem.RowObject is NavigatorGroup)
                {
                    var fillRect = new Rectangle(textRect.Left, textRect.Top + 5, textRect.Width, textRect.Height / 2 - 3);
                    using (var solidBrush = new SolidBrush(Color.FromArgb(255, 205, 220, 235)))
                        g.FillRectangle(solidBrush, fillRect);
                    g.DrawLine(Pens.DarkGray, fillRect.Left, fillRect.Top + fillRect.Height, fillRect.Right, fillRect.Top + fillRect.Height);
                }
                else
                {
                    textSize = g.MeasureString(text, boldFont);
                    var fillRect = new Rectangle(textRect.Left, textRect.Top + 4, (int)textSize.Width + 3, textRect.Height / 2 - 4);
                    if (IsItemSelected && ListView.Focused)
                            g.FillRectangle(Brushes.LightBlue, fillRect); 
                    else
                        using (var gradientBrush = new LinearGradientBrush(fillRect, Color.White, Color.FromArgb(255, 255, 235, 205), LinearGradientMode.Vertical))
                            g.FillRectangle(gradientBrush, fillRect);
                    g.DrawLine(Pens.DarkGray, fillRect.Left, fillRect.Top + fillRect.Height, fillRect.Right, fillRect.Top + fillRect.Height);
                }

                var fontColor = IsItemSelected && ListView.Focused ? Brushes.White : Brushes.Black;

                // Draw main text with bold font
                var textDrawColor = ListItem.RowObject is NavigatorGroup ? Brushes.Black : fontColor;
                g.DrawString(text, boldFont, Brushes.Black, textRect.Left, textRect.Top + 3);

                textSize = g.MeasureString(text, Font);
                int rectLeft = textRect.Left + (int)textSize.Width;
                int rectTop = (int)(contentRect.Top + textSize.Height - 2);

                if (ListItem.RowObject is NavigatorGroup group)
                {
                    var count = group.Collections != null ? group.Collections.Count() : 0;
                    using (var italicFont = new Font(Font, FontStyle.Italic))
                        g.DrawString($"{count} item{(count != 1 ? "s" : "")} in group", italicFont, fontColor, contentRect.Left + 2, rectTop);
                }
                else if (ListItem.RowObject is NavigatorCollection collection)
                {
                    DrawPlatformBadgeAndIcons(g, collection, contentRect, rectTop, ref rectLeft);
                }
            }
        }

        private void DrawExpandCollapseArrow(Graphics g, Rectangle arrowRect, bool expanded)
        {
            using (Pen p = new Pen(Brushes.Gray, 2))
            {
                if (!expanded)
                {
                    g.DrawLines(p, new Point[] {
                        new Point(arrowRect.Left + 5, arrowRect.Top + 5),
                        new Point(arrowRect.Left + 11, arrowRect.Top + 9),
                        new Point(arrowRect.Left + 5, arrowRect.Top + 14)
                    });
                }
                else
                {
                    g.DrawLines(p, new Point[] {
                        new Point(arrowRect.Left + 3, arrowRect.Top + 5),
                        new Point(arrowRect.Left + 7, arrowRect.Top + 11),
                        new Point(arrowRect.Left + 12, arrowRect.Top + 5)
                    });
                }
            }
        }

        private void DrawSelectionFill(Graphics g, Rectangle fillRect)
        {
            if (ListView.Focused)
            {
                if (ListItem.RowObject is NavigatorGroup)
                {
                    using (var solidBrush = new SolidBrush(ListView.SelectedBackColorOrDefault))
                        g.FillRectangle(solidBrush, fillRect);
                }
                else
                {
                    using (var gradientBrush = new LinearGradientBrush(fillRect, Color.LightBlue, ListView.SelectedBackColorOrDefault, LinearGradientMode.Vertical))
                        g.FillRectangle(gradientBrush, fillRect);
                }
            }
            else
            {
                using (var solidBrush = new SolidBrush(ListView.UnfocusedSelectedBackColorOrDefault))
                    g.FillRectangle(solidBrush, fillRect);
            }
        }

        private void DrawPlatformBadgeAndIcons(Graphics g, NavigatorCollection collection, Rectangle contentRect, int rectTop, ref int rectLeft)
        {
            // Platform badge
            var platformText = collection.CardCollection.Platform ?? string.Empty;
            var textSize = g.MeasureString(platformText, Font);

            using (var font = new Font(Font, FontStyle.Italic))
            {
                if (IsItemSelected && ListView.Focused)
                    g.DrawString(platformText, font, Brushes.White, contentRect.Left + 4, rectTop);
                else
                    g.DrawString(platformText, font, Brushes.Black, contentRect.Left + 4, rectTop);
            }
            // If deck, draw color identity icons
            if (collection.CardCollection.Type == "deck" && !string.IsNullOrEmpty(collection.CardCollection.ColorIdentity))
            {
                int imgLeft = (int)textSize.Width + 30;
                var reg = new Regex("{[A-Z0-9/]+}");
                var parts = reg.Matches(collection.CardCollection.ColorIdentity);
                var imageList = Globals.ImageLists.SymbolIcons16;

                foreach (Match part in parts)
                {
                    int? imgIndex = imageList?.Images?.IndexOfKey(part.Value);
                    if (imgIndex.HasValue && imgIndex.Value != -1)
                        imageList.Draw(g, imgLeft, (int)textSize.Height + 1, imgIndex.Value);

                    rectLeft += 18;
                    imgLeft += 18;
                }

                rectLeft += 5;
            }
        }
    }
}
