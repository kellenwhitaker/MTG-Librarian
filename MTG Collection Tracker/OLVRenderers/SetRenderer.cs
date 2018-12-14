using BrightIdeasSoftware;
using System.Drawing;
using static BrightIdeasSoftware.TreeListView;

namespace MTG_Collection_Tracker
{
    public class SetRenderer : TreeRenderer
    {
        public override void Render(Graphics g, Rectangle r)
        {
            var TreeView = ListView as TreeListView;
            int margin = 3;
            Rectangle backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width + 1, r.Height + 1);
            g.FillRectangle(new SolidBrush(Color.White), backgroundRect);
            Rectangle contentRect = new Rectangle { X = backgroundRect.Left, Y = backgroundRect.Y, Width = backgroundRect.Width, Height = backgroundRect.Height };
            int? imgIndex = ListView.SmallImageList?.Images?.IndexOfKey((ListItem.RowObject as OLVItem)?.ImageKey);
            if (ListItem.RowObject is OLVSetItem cardSet)
            {
                if (ListItem.SubItems.Count > 0)
                {
                    Rectangle arrowRect = new Rectangle(backgroundRect.Left, backgroundRect.Top, 11, backgroundRect.Height);
                    contentRect.X = arrowRect.Right + margin;
                    if (!TreeView.IsExpanded(cardSet))
                        using (Pen p = new Pen(Brushes.Gray, 2))
                            g.DrawLines(p, new Point[] { new Point(arrowRect.Left + 5, arrowRect.Top + 8), new Point(arrowRect.Left + 11, arrowRect.Top + 12), new Point(arrowRect.Left + 5, arrowRect.Top + 17) });
                    else
                        using (Pen p = new Pen(Brushes.Gray, 2))
                            g.DrawLines(p, new Point[] { new Point(arrowRect.Left + 3, arrowRect.Top + 8), new Point(arrowRect.Left + 7, arrowRect.Top + 14), new Point(arrowRect.Left + 12, arrowRect.Top + 8) });
                }
            }
            else if (ListItem.RowObject is OLVRarityItem rarityItem)
            {
                contentRect.X += 25;
            }
            int iconWidth = imgIndex.HasValue && imgIndex.Value != -1 ? ListView.SmallImageList?.ImageSize.Width ?? 0 : 0;
            Rectangle textRect = new Rectangle(contentRect.Left + iconWidth + margin, contentRect.Top + 3, contentRect.Width - iconWidth, contentRect.Height);
            if (IsItemSelected)
            {
                if (ListView.Focused)
                    g.FillRectangle(new SolidBrush(ListView.SelectedBackColor), textRect);
                else
                    g.FillRectangle(new SolidBrush(ListView.UnfocusedSelectedBackColor), textRect);
            }
            Brush fontColor = IsItemSelected && ListView.Focused ? Brushes.White : Brushes.Black;
            if (imgIndex.HasValue && imgIndex.Value != -1)
                ListView.SmallImageList.Draw(g, contentRect.Left + margin, 1, imgIndex.Value);
            g.DrawString(GetText(), Font, fontColor, textRect.Left, textRect.Top + 3);
        }
    }
}
