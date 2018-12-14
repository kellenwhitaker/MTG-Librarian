using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTG_Collection_Tracker
{
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
            if (ListItem.RowObject is FullInventoryCard)
                manaCost = (ListItem.RowObject as FullInventoryCard).manaCost;
            else if (ListItem.RowObject is OLVCardItem)
                manaCost = (ListItem.RowObject as OLVCardItem).MagicCard.manaCost;
            if (manaCost != "" && manaCost != null)
            {
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
    }
}
