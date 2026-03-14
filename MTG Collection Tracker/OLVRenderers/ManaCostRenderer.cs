using BrightIdeasSoftware;
using System;
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
            if (ListItem.RowObject is FullInventoryCard card)
            {
                if (card.mana_cost != null)
                    manaCost = card.mana_cost;
                else if (card.card_faces != null)
                    manaCost = card.card_faces[0].mana_cost;
            }
            else if (ListItem.RowObject is OLVCardItem olvCard)
            {
                if (olvCard.MagicCard.mana_cost != null)
                    manaCost = olvCard.MagicCard.mana_cost;
                else if (olvCard.MagicCard.card_faces != null)
                    manaCost = olvCard.MagicCard.card_faces[0].mana_cost;
            }
            if (manaCost != "" && manaCost != null)
            {
                var reg = new Regex("{[A-Z0-9/]+}|//");
                var costParts = reg.Matches(manaCost);
                var imageList = Globals.ImageLists.SymbolIcons16;
                int rectLeft = backgroundRect.Left + 10;
                int left = 5;
                foreach (Match part in costParts)
                {
                    if (part.Value == "//")
                    {
                        Font f = new Font(Font.FontFamily, 10,  FontStyle.Regular);
                        g.DrawString("//", f, Brushes.Black, rectLeft, backgroundRect.Top + 3, StringFormat.GenericDefault);
                        int width = Convert.ToInt32(g.MeasureString("//", f, new PointF(rectLeft, 3), StringFormat.GenericDefault).Width);
                        left += width + 10;
                        rectLeft += width + 10;
                    }
                    else
                    {                        
                        int? imgIndex = imageList?.Images?.IndexOfKey(part.Value);
                        if (imgIndex.HasValue && imgIndex.Value != -1)
                            imageList.Draw(g, left, 3, imgIndex.Value);
                        left += 18;
                        rectLeft += 18;
                    }
                }
            }
        }
    }
}
