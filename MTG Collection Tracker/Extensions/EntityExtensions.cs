using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public static class DBInstanceExtensions
    {
        public static FullInventoryCard ToFullCard(this InventoryCard inventoryCard, ScryfallCardsDbContext context)
        {
            return
            (from c in context.LibraryView
             where c.InventoryId == inventoryCard.InventoryId
             select c).FirstOrDefault();
        }

        public static ScryfallMagicCard ToScryfallMagicCard(this ScryfallCard scryfallCard)
        {
            return new ScryfallMagicCard()
            {
                ScryfallId = scryfallCard.ScryfallId,
                oracle_id = scryfallCard.oracle_id,
                flavor_text = scryfallCard.flavor_text,
                multiverse_ids = scryfallCard.multiverse_ids,
                mtgo_id = scryfallCard.mtgo_id,
                mtgo_foil_id = scryfallCard.mtgo_foil_id,
                tcgplayer_product_id = scryfallCard.tcgplayer_product_id,
                cardmarket_product_id = scryfallCard.cardmarket_product_id,
                Name = scryfallCard.Name,
                lang = scryfallCard.lang,
                released_at = scryfallCard.released_at,
                Uri = scryfallCard.Uri,
                scryfall_uri = scryfallCard.scryfall_uri,
                layout = scryfallCard.layout,
                highres_image = scryfallCard.highres_image,
                image_status = scryfallCard.image_status,
                image_uris = scryfallCard.image_uris,
                mana_cost = scryfallCard.mana_cost,
                cmc = scryfallCard.cmc,
                type_line = scryfallCard.type_line,
                text = scryfallCard.text,
                oracle_text = scryfallCard.oracle_text,
                power = scryfallCard.power,
                toughness = scryfallCard.toughness,
                colors = scryfallCard.colors,
                color_identity = scryfallCard.color_identity,
                keywords = scryfallCard.keywords,
                legalities = scryfallCard.legalities,
                games = scryfallCard.games,
                reserved = scryfallCard.reserved,
                has_foil = scryfallCard.has_foil,
                has_nonfoil = scryfallCard.has_nonfoil,
                finishes = scryfallCard.finishes,
                oversized = scryfallCard.oversized,
                promo = scryfallCard.promo,
                reprint = scryfallCard.reprint,
                variation = scryfallCard.variation,
                set_id = scryfallCard.set_id,
                set = scryfallCard.set,
                set_name = scryfallCard.set_name,
                set_type = scryfallCard.set_type,
                set_uri = scryfallCard.set_uri,
                set_search_uri = scryfallCard.set_search_uri,
                scryfall_set_uri = scryfallCard.scryfall_set_uri,
                rulings_uri = scryfallCard.rulings_uri,
                prints_search_uri = scryfallCard.prints_search_uri,
                collector_number = scryfallCard.collector_number,
                security_stamp = scryfallCard.security_stamp,
                digital = scryfallCard.digital,
                rarity = scryfallCard.rarity,
                card_back_id = scryfallCard.card_back_id,
                artist = scryfallCard.artist,
                artist_ids = scryfallCard.artist_ids,
                illustration_id = scryfallCard.illustration_id,
                border_color = scryfallCard.border_color,
                frame_version = scryfallCard.frame_version,
                full_art = scryfallCard.full_art,
                textless = scryfallCard.textless,
                booster = scryfallCard.booster,
                story_spotlight = scryfallCard.story_spotlight,
                edhrec_rank = scryfallCard.edhrec_rank,
                penny_rank = scryfallCard.penny_rank,
                prices = scryfallCard.prices,
                related_uris = scryfallCard.related_uris,
                purchase_uris = scryfallCard.purchase_uris,
                card_faces = scryfallCard.card_faces,
                printed_name = scryfallCard.printed_name,
                printed_type_line = scryfallCard.printed_type_line,
                printed_text = scryfallCard.printed_text,
            };
        }
    }
}
