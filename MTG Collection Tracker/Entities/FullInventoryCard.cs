using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Windows.Forms;

namespace MTG_Librarian
{
    public class FullInventoryCard : ScryfallMagicCardBase
    {
        [Key]
        public int InventoryId { get; set; }

        public int CollectionId { get; set; }
        public int? Count { get; set; }
        public double? Cost { get; set; }
        [NotMapped]
        public double? Price { get; set; }
        public string Tags { get; set; }
        public bool Foil { get; set; }
        public bool Virtual { get; set; }
        private DateTime? _timeAdded;
        public DateTime? TimeAdded 
        { 
            get => _timeAdded; 
            set 
            {
                _timeAdded = value; 
                UpdateSortableTimeAdded(); 
            } 
        }
        private int? _insertionIndex;
        public int? InsertionIndex { get => _insertionIndex; set { _insertionIndex = value; UpdateSortableTimeAdded(); } }
        public string Condition { get; set; }
        public string Finish { get; set; }
        public string Platform { get; set; }
        [NotMapped]
        public string SortableTimeAdded
        {
            get
            {
                if (_sortableTimeAdded == null)
                    _sortableTimeAdded = TimeAdded.HasValue ? $"{TimeAdded.Value.ToString("s")}{InsertionIndex.ToString().PadLeft(5)}" : "";
                    
                return _sortableTimeAdded;
            }
        }

        private string _sortableTimeAdded;
        [NotMapped]
        public double? Delta 
        { 
            get 
            { 
                if (Price.HasValue && Cost.HasValue)
                    return Price.Value - Cost.Value;
                else
                    return null;
            } 
        }
        [NotMapped]
        public double? Percent 
        { 
            get
            {
                if (Price.HasValue && Cost.HasValue && Cost.Value != 0.0)
                    return 100 * (Price.Value - Cost.Value) / Cost.Value;
                else
                    return null;
            }
        }
        [NotMapped]
        public string ImageKey => $"{SymbolCode}: {rarity}";

        [NotMapped]
        public string PaddedName => DisplayName != null ? DisplayName?.PadRight(500) : Name.PadRight(500);

        [NotMapped]
        public InventoryCard InventoryCard
        {
            get
            {
                return new InventoryCard
                {
                    CollectionId = CollectionId,
                    Cost = Cost,
                    Count = Count,
                    InsertionIndex = InsertionIndex,
                    InventoryId = InventoryId,
                    Tags = Tags,
                    TimeAdded = TimeAdded,
                    ScryfallId = ScryfallId,
                    Foil = Foil,
                    DisplayName = DisplayName,
                    Virtual = Virtual,
                    Condition = Condition,
                    Finish = Finish,
                    Platform = Platform
                };
            }
        }

        private void UpdateSortableTimeAdded()
        {
            if (TimeAdded.HasValue && InsertionIndex.HasValue) _sortableTimeAdded = $"{TimeAdded.Value.ToString("s")} {InsertionIndex.ToString().PadLeft(10)}";
        }

        public void CopyFromMagicCard(ScryfallMagicCard magicCard)
        {
            set_id = magicCard.set_id;
            set_name = magicCard.set_name;
            ScryfallId = magicCard.ScryfallId;
            oracle_id = magicCard.oracle_id;
            flavor_text = magicCard.flavor_text;
            multiverse_ids = magicCard.multiverse_ids;
            mtgo_id = magicCard.mtgo_id;
            mtgo_foil_id = magicCard.mtgo_foil_id;
            tcgplayer_product_id = magicCard.tcgplayer_product_id;
            cardmarket_product_id = magicCard.cardmarket_product_id;
            Name = magicCard.Name;
            lang = magicCard.lang;
            released_at = magicCard.released_at;
            Uri = magicCard.Uri;
            scryfall_uri = magicCard.scryfall_uri;
            layout = magicCard.layout;
            highres_image = magicCard.highres_image;
            image_status = magicCard.image_status;
            image_uris = magicCard.image_uris;
            mana_cost = magicCard.mana_cost;
            cmc = magicCard.cmc;
            type_line = magicCard.type_line;
            text = magicCard.text;
            oracle_text = magicCard.oracle_text;
            power = magicCard.power;
            toughness = magicCard.toughness;
            colors = magicCard.colors;
            color_identity = magicCard.color_identity;
            keywords = magicCard.keywords;
            legalities = magicCard.legalities;
            games = magicCard.games;
            reserved = magicCard.reserved;
            has_foil = magicCard.has_foil;
            has_nonfoil = magicCard.has_nonfoil;
            finishes = magicCard.finishes;
            oversized = magicCard.oversized;
            promo = magicCard.promo;
            reprint = magicCard.reprint;
            variation = magicCard.variation;
            set_id = magicCard.set_id;
            set = magicCard.set;
            set_name = magicCard.set_name;
            set_type = magicCard.set_type;
            set_uri = magicCard.set_uri;
            set_search_uri = magicCard.set_search_uri;
            scryfall_set_uri = magicCard.scryfall_set_uri;
            rulings_uri = magicCard.rulings_uri;
            prints_search_uri = magicCard.prints_search_uri;
            collector_number = magicCard.collector_number;
            security_stamp = magicCard.security_stamp;
            digital = magicCard.digital;
            rarity = magicCard.rarity;
            card_back_id = magicCard.card_back_id;
            artist = magicCard.artist;
            artist_ids = magicCard.artist_ids;
            illustration_id = magicCard.illustration_id;
            border_color = magicCard.border_color;
            frame_version = magicCard.frame_version;
            full_art = magicCard.full_art;
            textless = magicCard.textless;
            booster = magicCard.booster;
            story_spotlight = magicCard.story_spotlight;
            edhrec_rank = magicCard.edhrec_rank;
            penny_rank = magicCard.penny_rank;
            prices = magicCard.prices;
            related_uris = magicCard.related_uris;
            purchase_uris = magicCard.purchase_uris;
            
            PartB = magicCard.PartB;            
        }
    }
}