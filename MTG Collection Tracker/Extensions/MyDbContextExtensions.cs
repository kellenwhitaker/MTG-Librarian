using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MTG_Librarian
{
    public static class MyDbContextExtensions
    {
        public static void Upsert(this ScryfallCardsDbContext context, ScryfallCardSet set)
        {
            var existing = context.Sets.AsNoTracking().FirstOrDefault(x => x.name == set.name);
            if (existing == null) // new set
                context.Add(set);
            else // update existing set
            {
                context.Update(set);
            }
        }
        public static void Upsert(this ScryfallCardsDbContext context, CardSet set)
        {
            var existing = context.Sets.AsNoTracking().FirstOrDefault(x => x.name == set.Name);
            if (existing == null) // new set
                context.Add(set);
            else // update existing set
            {
                context.Update(set);
            }
        }
        public static void Upsert(this ScryfallCardsDbContext context, ScryfallCard card)
        {
            if (card.text == null)
                card.text = "";
            var existing = context.Catalog.AsNoTracking().FirstOrDefault(x => x.ScryfallId == card.ScryfallId);
            if (existing == null) // new card
            {
                context.Add(card);
            }
            else // update existing card
            {
                // don't overwrite existing prices
                card.prices = existing.prices;
                context.Update(card);
            }
        }
        public static void Upsert(this ScryfallCardsDbContext context, ScryfallMagicCard card)
        {
            if (card.text == null)
                card.text = "";
            var existing = context.Catalog.AsNoTracking().FirstOrDefault(x => x.ScryfallId == card.ScryfallId);
            if (existing == null) // new card
            {
                context.Add(card);
            }
            else // update existing card
            {
                // don't overwrite existing prices
                card.prices = existing.prices;
                context.Update(card);
            }
        }
    }
}