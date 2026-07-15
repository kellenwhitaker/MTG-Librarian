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
            Upsert(context, card.ToScryfallMagicCard());
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
        public static void Upsert(this ScryfallCardsDbContext context, CollectionGroup group)
        {
            var existing = context.CollectionGroups.AsNoTracking().FirstOrDefault(x => x.Id == group.Id);
            if (existing == null) // new group
                context.Add(group);
            else // update existing group
            {
                context.Update(group);
            }
        }
        public static void Upsert(this ScryfallCardsDbContext context, CardCollection collection)
        {
            var existing = context.Collections.AsNoTracking().FirstOrDefault(x => x.Id == collection.Id);
            if (existing == null) // new collection
                context.Add(collection);
            else // update existing collection
            {
                context.Update(collection);
            }
        }
    }
}