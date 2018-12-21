using System.Linq;

namespace MTG_Librarian
{
    public static class DBInstanceExtensions
    {
        public static FullInventoryCard ToFullCard(this InventoryCard inventoryCard, MyDbContext context)
        {
            return
            (from c in context.LibraryView
             where c.InventoryId == inventoryCard.InventoryId
             select c).FirstOrDefault();
        }
    }
}
