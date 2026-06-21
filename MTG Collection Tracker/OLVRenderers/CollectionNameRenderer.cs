using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using static BrightIdeasSoftware.TreeListView;

namespace MTG_Librarian
{
    public class CollectionNameRenderer : TreeRenderer
    {
        public override void Render(Graphics g, Rectangle r)
        {
            var TreeView = ListView as TreeListView;
            var text = ListItem.Text;
            const int margin = 3;
            var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
            using (var solidBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(solidBrush, backgroundRect);
            }
            var contentRect = new Rectangle { X = backgroundRect.Left, Y = backgroundRect.Y, Width = backgroundRect.Width, Height = backgroundRect.Height };
            if (ListItem.RowObject is NavigatorGroup navigatorGroup)
            {
                if (ListItem.SubItems.Count > 0)
                {
                    var arrowRect = new Rectangle(backgroundRect.Left, backgroundRect.Top, 11, backgroundRect.Height);
                    contentRect.X = arrowRect.Right + margin;
                    if (!TreeView.IsExpanded(navigatorGroup))
                        using (Pen p = new Pen(Brushes.Gray, 2))
                            g.DrawLines(p, new Point[] { new Point(arrowRect.Left + 5, arrowRect.Top + 5), new Point(arrowRect.Left + 11, arrowRect.Top + 9), new Point(arrowRect.Left + 5, arrowRect.Top + 14) });
                    else
                        using (Pen p = new Pen(Brushes.Gray, 2))
                            g.DrawLines(p, new Point[] { new Point(arrowRect.Left + 3, arrowRect.Top + 5), new Point(arrowRect.Left + 7, arrowRect.Top + 11), new Point(arrowRect.Left + 12, arrowRect.Top + 5) });
                }
            }
            else if (ListItem.RowObject is NavigatorCollection navigatorCollection)
            {
                contentRect.X += 25;
                text += $" - ";
            }
            var textRect = new Rectangle(contentRect.Left + margin, contentRect.Top, contentRect.Width, contentRect.Height);
            if (IsItemSelected)
            {
                if (ListView.Focused)
                    using (var solidBrush = new SolidBrush(ListView.SelectedBackColorOrDefault))
                    {
                        g.FillRectangle(solidBrush, textRect);
                    }
                else
                    using (var solidBrush = new SolidBrush(ListView.UnfocusedSelectedBackColorOrDefault))
                    {
                        g.FillRectangle(solidBrush, textRect);
                    }
            }
            var fontColor = IsItemSelected && ListView.Focused ? Brushes.White : Brushes.Black;
            g.DrawString(text, Font, fontColor, textRect.Left, textRect.Top + 3);
            var textSize = g.MeasureString(text, Font);
            if (ListItem.RowObject is NavigatorCollection collection)
            {
                g.DrawString(collection.CardCollection.Platform, new Font(Font, FontStyle.Italic), fontColor, contentRect.Left + textSize.Width, contentRect.Top + 3);
            }
        }

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
    }
}
