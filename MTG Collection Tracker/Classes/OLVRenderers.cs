using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTG_Collection_Tracker
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class ManaCostRenderer : BaseRenderer
    {
        public override void Render(Graphics g, Rectangle r)
        {
            Color backColor = ListView.BackColor;
            Rectangle backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width, r.Height + 1);
            if (IsItemSelected)
                backColor = ListView.Focused ? ListView.SelectedBackColor : ListView.UnfocusedSelectedBackColor;

            g.FillRectangle(new SolidBrush(backColor), backgroundRect);
            string manaCost = "";
            if (ListItem.RowObject is CardInstance)
                manaCost = (ListItem.RowObject as CardInstance).ManaCost;
            else if (ListItem.RowObject is OLVCardItem)
                manaCost = (ListItem.RowObject as OLVCardItem).MCard.ManaCost;
            Regex reg = new Regex("{[A-Z0-9/]+}");
            var costParts = reg.Matches(manaCost);
            var imageList = MainForm.SymbolIcons16;
            int left = 5;
            foreach (Match part in costParts)
            {
                int? imgIndex = imageList?.Images?.IndexOfKey(part.Value);
                if (imgIndex.HasValue && imgIndex.Value != -1)
                    imageList.Draw(g, left, 3, imgIndex.Value);

                left += 18;
            }
        }
    }

    public class CardInstanceNameRenderer : BaseRenderer
    {
        public override void Render(Graphics g, Rectangle r)
        {
            var cardInstance = ListItem.RowObject as CardInstance;
            int? imgIndex = ListView.SmallImageList?.Images?.IndexOfKey(cardInstance.ImageKey);
            Rectangle backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
            if (IsItemSelected)
            {
                if (ListView.Focused)
                    g.FillRectangle(new SolidBrush(ListView.SelectedBackColor), backgroundRect);
                else
                    g.FillRectangle(new SolidBrush(ListView.UnfocusedSelectedBackColor), backgroundRect);
            }
            else
                g.FillRectangle(new SolidBrush(ListView.BackColor), backgroundRect);

            Brush fontColor = IsItemSelected && ListView.Focused ? Brushes.White : Brushes.Black;
            if (imgIndex.HasValue && imgIndex.Value != -1)
            {
                ListView.SmallImageList.Draw(g, 2, 0, imgIndex.Value);
                g.DrawString(GetText(), Font, fontColor, r.Left + 25, r.Top + 3);
            }
            else
                g.DrawString(GetText(), Font, fontColor, r.Left + 5, r.Top + 3);
        }
    }

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
            else
            {
                Font font = new Font(FontFamily.Families.Where(x => x.Name == "Segoe UI").First(), 10, FontStyle.Bold);
                g.DrawString(task.Caption, font, Brushes.Black, r.Left + 3, r.Top + 7);
                g.DrawString(task.Runtime.ToString() + "s", new Font(font, FontStyle.Regular), Brushes.Black, r.Left + 5 + (int)g.MeasureString(task.Caption, font).Width, r.Top + 7);
            }
        }
    }

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
