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
            var existing = context.Sets.AsNoTracking().FirstOrDefault(x => x.Name == set.Name);
            if (existing == null)
                context.Add(set);
            else
            {
                context.Update(set);
            }
        }

        public static void Upsert(this MyDbContext context, MagicCard card)
        {
            var existing = context.Catalog.AsNoTracking().FirstOrDefault(x => x.uuid == card.uuid);
            if (existing == null)
            {
                context.Add(card);
            }
            else
            {
                context.Update(card);
            }
        }
    }
}
