using BrightIdeasSoftware;
using System.Drawing;

namespace MTG_Librarian
{
    public class IconRenderer : BaseRenderer
    {
        public override void Render(Graphics g, Rectangle r)
        {
            var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
            if (IsItemSelected)
                using (var solidBrush = new SolidBrush(Color.AliceBlue))
                {
                    g.FillRectangle(solidBrush, backgroundRect);
                }
            else
                g.FillRectangle(Brushes.White, backgroundRect);

            var task = ListItem.RowObject as BackgroundTask;
            var icon = task.Icon;
            if (icon != null)
            {
                int xinc = (r.Width - icon.Width) / 2;
                int yinc = (r.Height - icon.Height) / 2;
                g.DrawImage(icon, new Rectangle(r.X + xinc, r.Y + yinc, icon.Width, icon.Height));
            }
        }
    }
}
