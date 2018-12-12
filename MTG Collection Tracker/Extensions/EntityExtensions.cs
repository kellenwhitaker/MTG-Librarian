using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Collection_Tracker
{
    public static class DBInstanceExtensions
    {
        public static FullInventoryCard ToCardInstance(this InventoryCard dbInstance, MyDbContext context)
        {
            var instance =
            (from c in context.LibraryView
             where c.CardInstanceId == dbInstance.CardInstanceId
             select c).FirstOrDefault();
            return instance;
        }
    }
}
