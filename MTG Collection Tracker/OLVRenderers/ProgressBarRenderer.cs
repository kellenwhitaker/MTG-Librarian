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
            Bitmap bmp = new Bitmap(task.ProgressBar.Width, task.ProgressBar.Height);
            task.ProgressBar.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            Rectangle backgroundRect = new Rectangle(r.Left, r.Top - 1, r.Width, r.Height + 1);
            if (IsItemSelected)
                g.FillRectangle(new SolidBrush(Color.AliceBlue), backgroundRect);
            else
                g.FillRectangle(Brushes.White, backgroundRect);
            if (task.Running)
            {
                g.DrawImageUnscaled(bmp, r.Left + 5, r.Top + 20);
                Font font = new Font(FontFamily.Families.Where(x => x.Name == "Segoe UI").First(), 9, FontStyle.Regular);
                g.DrawString(GetText(), font, Brushes.Black, r.Left + 3, r.Top + 2);
                g.DrawString(task.Runtime.ToString() + "s", new Font(font, FontStyle.Bold), Brushes.Green, r.Left + 5 + (int)g.MeasureString(task.Caption, font).Width, r.Top + 2);
            }
            else if (task.RunState == RunState.Completed)
            {
                Font font = new Font(FontFamily.Families.Where(x => x.Name == "Segoe UI").First(), 10, FontStyle.Bold);
                g.DrawString(task.Caption, font, Brushes.Black, r.Left + 3, r.Top + 7);
                g.DrawString(task.Runtime.ToString() + "s", new Font(font, FontStyle.Regular), Brushes.Black, r.Left + 5 + (int)g.MeasureString(task.Caption, font).Width, r.Top + 7);
            }
            else if (task.RunState == RunState.Initialized)
            {
                Font font = new Font(FontFamily.Families.Where(x => x.Name == "Segoe UI").First(), 10, FontStyle.Regular);
                g.DrawString(task.Caption, font, Brushes.Black, r.Left + 3, r.Top + 7);
            }
            else if (task.RunState == RunState.Failed)
            {
                Font font = new Font(FontFamily.Families.Where(x => x.Name == "Segoe UI").First(), 10, FontStyle.Bold);
                g.DrawString(task.Caption, font, Brushes.DarkRed, r.Left + 3, r.Top + 7);
            }
        }
    }
}
