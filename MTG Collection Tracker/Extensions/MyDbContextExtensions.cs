using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MTG_Librarian
{
    public static class MyDbContextExtensions
    {
        public static void Upsert(this MyDbContext context, CardSet set)
        {
            var existing = context.Sets.AsNoTracking().FirstOrDefault(x => x.Name == set.Name);
            if (existing == null) // new set
                context.Add(set);
            else // update existing set
            {
                context.Update(set);
            }
        }

        public static void Upsert(this MyDbContext context, MagicCard card)
        {
            var existing = context.Catalog.AsNoTracking().FirstOrDefault(x => x.uuid == card.uuid);
            if (existing == null) // new card
            {
                context.Add(card);
            }
            else // update existing card
            {
                // don't overwrite existing prices
                card.tcgplayerMarketPrice = existing.tcgplayerMarketPrice;
                card.tcgplayerLowPrice = existing.tcgplayerLowPrice;
                card.tcgplayerMidPrice = existing.tcgplayerMidPrice;
                card.tcgplayerHighPrice = existing.tcgplayerHighPrice;
                context.Update(card);
            }
        }
    }
}
