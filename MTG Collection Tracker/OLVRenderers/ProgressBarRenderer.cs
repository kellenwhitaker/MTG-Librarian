using BrightIdeasSoftware;
using System.Drawing;
using System.Linq;

namespace MTG_Librarian
{
    public class ProgressBarRenderer : BaseRenderer
    {
        public override void Render(Graphics g, Rectangle r)
        {
            var task = ListItem.RowObject as BackgroundTask;
            var bmp = new Bitmap(task.ProgressBar.Width, task.ProgressBar.Height);
            task.ProgressBar.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            var backgroundRect = new Rectangle(r.Left, r.Top - 1, r.Width, r.Height + 1);
            if (IsItemSelected)
                using (var solidBrush = new SolidBrush(Color.AliceBlue))
                {
                    g.FillRectangle(solidBrush, backgroundRect);
                }
            else
                g.FillRectangle(Brushes.White, backgroundRect);
            if (task.Running)
            {
                g.DrawImageUnscaled(bmp, r.Left + 5, r.Top + 20);
                var font = new Font(FontFamily.Families.First(x => x.Name == "Segoe UI"), 9, FontStyle.Regular);
                g.DrawString(GetText(), font, Brushes.Black, r.Left + 3, r.Top + 2);
                using (var font1 = new Font(font, FontStyle.Bold))
                {
                    g.DrawString(task.Runtime + "s", font1, Brushes.Green, r.Left + 5 + (int)g.MeasureString(task.Caption, font).Width, r.Top + 2);
                }
            }
            else if (task.RunState == RunState.Completed)
            {
                var font = new Font(FontFamily.Families.First(x => x.Name == "Segoe UI"), 10, FontStyle.Bold);
                g.DrawString(task.Caption, font, Brushes.Black, r.Left + 3, r.Top + 7);
                using (var font1 = new Font(font, FontStyle.Regular))
                {
                    g.DrawString(task.Runtime + "s", font1, Brushes.Black, r.Left + 5 + (int)g.MeasureString(task.Caption, font).Width, r.Top + 7);
                }
            }
            else if (task.RunState == RunState.Initialized)
            {
                var font = new Font(FontFamily.Families.First(x => x.Name == "Segoe UI"), 10, FontStyle.Regular);
                g.DrawString(task.Caption, font, Brushes.Black, r.Left + 3, r.Top + 7);
            }
            else if (task.RunState == RunState.Failed)
            {
                var font = new Font(FontFamily.Families.First(x => x.Name == "Segoe UI"), 10, FontStyle.Bold);
                g.DrawString(task.Caption, font, Brushes.DarkRed, r.Left + 3, r.Top + 7);
            }
        }
    }
}
