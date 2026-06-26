using BrightIdeasSoftware;
using System.Drawing;

namespace MTG_Librarian
{
    public class DeltaRenderer : BaseRenderer
    {
        private void FillBackground(Graphics g, Rectangle backgroundRect)
        {
            if (RowObject is InventoryTotalsItem)
            {
                using (var solidBrush = new SolidBrush(ListItem.BackColor))
                {
                    g.FillRectangle(solidBrush, backgroundRect);
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
            if (ListItem.RowObject is InventoryCard cardInstance)
            {
                var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
                FillBackground(g, backgroundRect);
                Brush fontColor;
                bool isPositive = false;
                if (cardInstance.Delta.HasValue && cardInstance.Delta.Value > 0)
                {
                    isPositive = true;
                    fontColor = Brushes.Green;
                }
                else if (cardInstance.Delta.HasValue && cardInstance.Delta.Value < 0)
                    fontColor = Brushes.Red;
                else
                    fontColor = Brushes.Black;
                var textRect = new Rectangle(r.Left, r.Top + 5, r.Width, r.Height - 5);
                g.DrawString(isPositive ? "+" + GetText() : GetText(), Font, fontColor, textRect, new StringFormat() { Alignment = StringAlignment.Far });
            }
            else if (ListItem.RowObject is InventoryTotalsItem item)
            {
                var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
                FillBackground(g, backgroundRect);
                double value = 0;
                var isPositive = false;
                Brush fontColor = Brushes.White;
                if (Column.AspectName == "Delta")
                    value = item.Delta;
                else if (Column.AspectName == "Percent")
                    value = item.Percent;
                if (value > 0)
                    isPositive = true;
                var textRect = new Rectangle(r.Left, r.Top + 5, r.Width, r.Height - 5);
                g.DrawString(isPositive ? "+" + GetText() : GetText(), Font, fontColor, textRect, new StringFormat() { Alignment = StringAlignment.Far });
            }
        }
    }
}
