using BrightIdeasSoftware;
using System.Collections;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public class CollectionSortingDataSource : FastObjectListDataSource
    {
        public CollectionSortingDataSource(FastObjectListView listView) : base(listView)
        {
        }

        override public void Sort(OLVColumn column, SortOrder order)
        {
            var comparer = new FullInventoryCardComparer { SortOrder = order, AspectName = column.AspectName };
            FilteredObjectList.Sort(comparer);
            RebuildIndexMap();
        }

        private class FullInventoryCardComparer : IComparer
        {
            public SortOrder SortOrder;
            public string AspectName;

            private static int CompareNullableDoubles(double? x, double? y)
            {
                if (!x.HasValue && y.HasValue)
                    return -1;
                else if (x.HasValue && !y.HasValue)
                    return 1;
                else if (!x.HasValue && !y.HasValue)
                    return 0;
                else
                    return x.Value.CompareTo(y.Value);
            }

            private static int CompareText(string x, string y)
            {
                if (x == null && y != null)
                    return -1;
                else if (x != null && y == null)
                    return 1;
                else if (x == null && y == null)
                    return 0;
                else
                    return x.CompareTo(y);
            }
            private static int ComparePT(string x, string y)
            {
                if (x == null && y != null)
                    return -1;
                else if (x != null && y == null)
                    return 1;
                else if (x == null && y == null)
                    return 0;
                else
                {
                    if (x == "*" && y != "*")
                        return 1;
                    else if (x != "*" && y == "*")
                        return -1;
                    else if (x == "1+*" && y != "1+*")
                        return 1;
                    else if (x != "1+*" && y == "1+*")
                        return -1;
                    else
                    {
                        int intx, inty;
                        int.TryParse(x, out intx);
                        int.TryParse(y, out inty);
                        return intx.CompareTo(inty);
                    }
                }
            }
            public int Compare(object x, object y)
            {
                if (x is InventoryTotalsItem)
                    return -1;
                else if (y is InventoryTotalsItem)
                    return 1;
                else
                {
                    int result = 0;
                    if (AspectName == "number")
                        result = (x as InventoryCard).SortableNumber.CompareTo((y as InventoryCard).SortableNumber);
                    else if (AspectName == "PaddedName")
                        result = (x as InventoryCard).Name.CompareTo((y as InventoryCard).Name);
                    else if (AspectName == "type")
                        result = (x as InventoryCard).type_line.CompareTo((y as InventoryCard).type_line);
                    else if (AspectName == "Edition")
                        result = (x as InventoryCard).set_name.CompareTo((y as InventoryCard).set_name);
                    else if (AspectName == "ManaCost")
                    {
                        string valueX = (x as InventoryCard).mana_cost ?? "";
                        string valueY = (y as InventoryCard).mana_cost ?? "";
                        result = valueX.CompareTo(valueY);
                    }
                    else if (AspectName == "TimeAdded")
                    {
                        result = (x as InventoryCard).SortableTimeAdded.CompareTo((y as InventoryCard).SortableTimeAdded);
                        if (result == 0)
                            result = (x as InventoryCard).InventoryId.CompareTo((y as InventoryCard).InventoryId);
                    }
                    else if (AspectName == "Foil")
                        result = (x as InventoryCard).Foil.CompareTo((y as InventoryCard).Foil);
                    else if (AspectName == "Cost")
                        result = CompareNullableDoubles((x as InventoryCard).Cost, (y as InventoryCard).Cost);
                    else if (AspectName == "Price")
                        result = CompareNullableDoubles((x as InventoryCard).Price, (y as InventoryCard).Price);
                    else if (AspectName == "Count")
                    {
                        var cx = (x as InventoryCard).Count;
                        var cy = (y as InventoryCard).Count;
                        if (!cx.HasValue && cy.HasValue)
                            result = -1;
                        else if (cx.HasValue && !cy.HasValue)
                            result = 1;
                        else if (!cx.HasValue && !cy.HasValue)
                            result = 0;
                        else
                            result = cx.Value.CompareTo(cy.Value);
                    }
                    else if (AspectName == "Tags")
                        result = CompareText((x as InventoryCard).Tags, (y as InventoryCard).Tags);
                    else if (AspectName == "text")
                        result = CompareText((x as InventoryCard).text, (y as InventoryCard).text);
                    else if (AspectName == "Delta")
                        result = CompareNullableDoubles((x as InventoryCard).Delta, (y as InventoryCard).Delta);
                    else if (AspectName == "X")
                        result = CompareNullableDoubles((x as InventoryCard).Percent, (y as InventoryCard).Percent);
                    else if (AspectName == "power")
                        result = ComparePT((x as InventoryCard).power, (y as InventoryCard).power);
                    else if (AspectName == "toughness")
                        result = ComparePT((x as InventoryCard).toughness, (y as InventoryCard).toughness);
                    return SortOrder == SortOrder.Ascending ? result : -1 * result;
                }
            }
        }
    }
}