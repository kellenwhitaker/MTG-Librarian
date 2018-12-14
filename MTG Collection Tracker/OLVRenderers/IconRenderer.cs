using BrightIdeasSoftware;
using System.Drawing;

namespace MTG_Collection_Tracker
{
    public class IconRenderer : BaseRenderer
    {
        public override void Render(Graphics g, Rectangle r)
        {
            Rectangle backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
            if (IsItemSelected)
                g.FillRectangle(new SolidBrush(Color.AliceBlue), backgroundRect);
            else
                g.FillRectangle(Brushes.White, backgroundRect);

            var task = ListItem.RowObject as BackgroundTask;
            Image icon = task.Icon;
            if (icon != null)
            {
                int xinc = (r.Width - icon.Width) / 2;
                int yinc = (r.Height - icon.Height) / 2;
                g.DrawImage(icon, new Rectangle(r.X + xinc, r.Y + yinc, icon.Width, icon.Height));
            }
        }
    }
}
