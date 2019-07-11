using System.Drawing;
using BrightIdeasSoftware;

namespace MTG_Librarian
{
    public class CheckBoxRenderer : BaseRenderer
    {
        public static readonly int CheckBoxWidth = 12;
        public static readonly int CheckBoxHeight = 12;

        private void FillBackground(Graphics g, Rectangle backgroundRect)
        {
            if (IsItemSelected)
            {
                if (ListView.Focused)
                    using (var solidBrush = new SolidBrush(ListView.SelectedBackColor))
                        g.FillRectangle(solidBrush, backgroundRect);
                else
                    using (var solidBrush = new SolidBrush(ListView.UnfocusedSelectedBackColor))
                        g.FillRectangle(solidBrush, backgroundRect);
            }
            else
                using (var solidBrush = new SolidBrush(ListView.BackColor))
                    g.FillRectangle(solidBrush, backgroundRect);
        }

        public override void Render(Graphics g, Rectangle r)
        {
            var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
            FillBackground(g, backgroundRect);
            if (ListItem.RowObject is FullInventoryCard cardInstance)
            {
                int boxLeft = r.Left + (r.Right - r.Left - CheckBoxWidth) / 2;
                int boxTop = r.Top + (r.Bottom - r.Top - CheckBoxHeight) / 2;
                g.DrawRectangle(Pens.LightGray, boxLeft, boxTop, CheckBoxWidth, CheckBoxHeight);
                if (cardInstance.Foil)
                {
                    Pen pen;
                    if (!IsItemSelected)
                        pen = new Pen(Brushes.DodgerBlue, 2);
                    else
                        pen = new Pen(Brushes.LightGray, 2);
                    using (pen)
                    {
                        g.DrawLine(pen, boxLeft + 2, boxTop + CheckBoxHeight - 4, boxLeft + 5, boxTop + CheckBoxHeight - 2);
                        g.DrawLine(pen, boxLeft + 5, boxTop + CheckBoxHeight - 2, boxLeft + CheckBoxWidth - 2, boxTop + 2);
                    }
                }
            }
        }
    }
}
