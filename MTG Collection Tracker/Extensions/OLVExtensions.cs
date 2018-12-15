using BrightIdeasSoftware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public static class OLVExtensions
    {
        public static IEnumerable<OLVListItem> SelectedOLVListItems(this ObjectListView olv)
        {
            var selected = new List<OLVListItem>();
            foreach (var selectedObject in olv.SelectedObjects)
                selected.Add(olv.ModelToItem(selectedObject));
            return selected;
        }

        public static void InsertObject(this ObjectListView olv, int index, object o)
        {
            olv.InsertObjects(index, new ArrayList { o });
        }

        public static bool IsExpanded(this TreeListView tlv, object tlvObject)
        {
            foreach (var ob in tlv.ExpandedObjects)
            {
                if (ob == tlvObject)
                    return true;
            }
            return false;
        }
    }
}
