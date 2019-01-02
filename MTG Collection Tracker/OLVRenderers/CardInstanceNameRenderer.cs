using BrightIdeasSoftware;
using System.Drawing;

namespace MTG_Librarian
{
    public class CardInstanceNameRenderer : BaseRenderer
    {
        private void FillBackground(Graphics g, Rectangle backgroundRect)
        {
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
            if (ListItem.RowObject is FullInventoryCard cardInstance)
            {
                int? imgIndex = ListView.SmallImageList?.Images?.IndexOfKey(cardInstance.ImageKey);
                int imgWidth = ListView.SmallImageList?.ImageSize.Width ?? 0;
                var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
                FillBackground(g, backgroundRect);
                var fontColor = IsItemSelected && ListView.Focused ? Brushes.White : Brushes.Black;
                if (imgIndex.HasValue && imgIndex.Value != -1)
                {
                    ListView.SmallImageList.Draw(g, 2, 0, imgIndex.Value);
                    g.DrawString(GetText(), Font, fontColor, r.Left + imgWidth + 3, r.Top + 3);
                }
                else
                    g.DrawString(GetText(), Font, fontColor, r.Left + 5, r.Top + 3);
            }
            else if (ListItem.RowObject is InventoryTotalsItem totals)
            {
                using (var boldFont = new Font(Font, FontStyle.Bold))
                {
                    var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
                    FillBackground(g, backgroundRect);
                    var fontColor = IsItemSelected && ListView.Focused ? Brushes.White : Brushes.Black;
                    g.DrawString(GetText(), boldFont, fontColor, r.Left + 5, r.Top + 3);
                }
            }
        }
    }
}
