using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public static class MyDbContextExtensions
    {
        public static void Upsert(this MyDbContext context, CardSet set)
        {
            var existing = context.Sets.AsNoTracking().Where(x => x.Name == set.Name).FirstOrDefault();
            if (existing == null)
                context.Add(set);
            else
            {
                context.Update(set);
            }
        }

        public static void Upsert(this MyDbContext context, MagicCard card)
        {
            var existing = context.Catalog.AsNoTracking().Where(x => x.uuid == card.uuid).FirstOrDefault();
            if (existing == null)
            {
                Console.WriteLine("null");
                context.Add(card);
            }
            else
            {
                Console.WriteLine(card.name + ", " + card.uuid);
                context.Update(card);
            }
        }
    }
}
