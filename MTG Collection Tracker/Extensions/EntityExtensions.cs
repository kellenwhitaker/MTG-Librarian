using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Collection_Tracker
{
    public static class DBInstanceExtensions
    {
        public static FullInventoryCard ToFullCard(this InventoryCard inventoryCard, MyDbContext context)
        {
            var fullCard =
            (from c in context.LibraryView
             where c.InventoryId == inventoryCard.InventoryId
             select c).FirstOrDefault();
            return fullCard;
        }
    }
}
