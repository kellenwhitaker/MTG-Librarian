using BrightIdeasSoftware;
using System.Drawing;
using System.Text.RegularExpressions;

namespace MTG_Librarian
{
    public class ManaCostRenderer : BaseRenderer
    {
        public override void Render(Graphics g, Rectangle r)
        {
            var backColor = ListView.BackColor;
            var backgroundRect = new Rectangle(r.Left - 1, r.Top - 1, r.Width, r.Height + 1);
            if (IsItemSelected)
                backColor = ListView.Focused ? ListView.SelectedBackColor : ListView.UnfocusedSelectedBackColor;

            using (var solidBrush = new SolidBrush(backColor))
            {
                g.FillRectangle(solidBrush, backgroundRect);
            }
            string manaCost = "";
            if (ListItem.RowObject is FullInventoryCard)
                manaCost = (ListItem.RowObject as FullInventoryCard).manaCost;
            else if (ListItem.RowObject is OLVCardItem)
                manaCost = (ListItem.RowObject as OLVCardItem).MagicCard.manaCost;
            if (manaCost != "" && manaCost != null)
            {
                var reg = new Regex("{[A-Z0-9/]+}");
                var costParts = reg.Matches(manaCost);
                var imageList = Globals.ImageLists.SymbolIcons16;
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
