using BrightIdeasSoftware;
using System.Drawing;

namespace MTG_Librarian
{
    public class XRenderer : BaseRenderer
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
            if (ListItem.RowObject is InventoryCard cardInstance)
            {
                var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
                FillBackground(g, backgroundRect);
                Brush fontColor;
                if (cardInstance.Delta.HasValue && cardInstance.Delta.Value > 0)
                {
                    fontColor = Brushes.Green;
                }
                else if (cardInstance.Delta.HasValue && cardInstance.Delta.Value < 0)
                    fontColor = Brushes.Red;
                else
                    fontColor = Brushes.Black;
                var textRect = new Rectangle(r.Left, r.Top + 5, r.Width, r.Height - 5);
                g.DrawString(GetText(), Font, fontColor, textRect, new StringFormat() { Alignment = StringAlignment.Far });
            }
            else if (ListItem.RowObject is InventoryTotalsItem)
            {
                var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
                FillBackground(g, backgroundRect);
            }
        }
    }
}
