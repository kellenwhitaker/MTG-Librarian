using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace MTG_Librarian
{
    public class ScryfallCard
    {
        [JsonProperty("id")]
        public string ScryfallId { get; set; }
        public string oracle_id { get; set; }
        public string flavor_text { get; set; }
        [NotMapped]
        public int[] multiverse_ids { get; set; }        
        public string MultiverseIds {
            get {
                return multiverse_ids == null ? null : JsonConvert.SerializeObject(multiverse_ids);
            }
            set {
                multiverse_ids = value == null ? null : JsonConvert.DeserializeObject<int[]>(value);
            }
        }
        
        public int mtgo_id { get; set; }
        public int mtgo_foil_id { get; set; }
        public int tcgplayer_product_id { get; set; }
        public int cardmarket_product_id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        public string lang { get; set; }
        public string released_at { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
        public string scryfall_uri { get; set; }
        public string layout { get; set; }
        public bool highres_image { get; set; }
        public string image_status { get; set; }
        [NotMapped]
        public Dictionary<string, string> image_uris { get; set; }
        public string ImageURIs {
            get
            {
                return image_uris == null ? null : JsonConvert.SerializeObject(image_uris);
            }
            set
            {
                image_uris = value == null ? null : JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
            }
        }
        public string mana_cost { get; set; }
        public float cmc { get; set; }
        public string type_line { get; set; }
        public string text { get; set; }
        public string oracle_text { get; set; }
        public string power { get; set; }
        public string toughness { get; set; }
        [NotMapped]
        public string[] colors;
        public string Colors {
            get {
                return colors == null ? null : JsonConvert.SerializeObject(colors);
            }
            set {
                colors = value == null ? null : JsonConvert.DeserializeObject<string[]>(value);
            }
        }
        [NotMapped]
        public string[] color_identity;
        public string ColorIdentity {
            get {
                return color_identity == null ? null : JsonConvert.SerializeObject(color_identity);
            } set {
                color_identity = value == null ? null : JsonConvert.DeserializeObject<string[]>(value);
            }
        }
        [NotMapped]
        public string[] keywords;
        public string Keywords {
            get {
                return keywords == null ? null : JsonConvert.SerializeObject(keywords);
            } set {
                keywords = value == null ? null : JsonConvert.DeserializeObject<string[]>(value);
            }
        }
        [NotMapped]
        public Dictionary<string, string> legalities;
        public string Legalities {
            get {
                return legalities == null ? null : JsonConvert.SerializeObject(legalities);
            } set {
                legalities = value == null ? null : JsonConvert.DeserializeObject<Dictionary<string, string>> (value);
            }
        }
        [NotMapped]
        public string[] games;
        public string Games {
            get {
                return games == null ? null : JsonConvert.SerializeObject(games);
            } set {
                games = value == null ? null : JsonConvert.DeserializeObject<string[]>(value);
            }
        }
        public bool reserved { get; set; }
        [JsonProperty("foil")]
        public bool has_foil { get; set; }
        [JsonProperty("nonfoil")]
        public bool has_nonfoil { get; set; }
        [NotMapped]
        public string[] finishes;
        public string Finishes {
            get {
                return finishes == null ? null : JsonConvert.SerializeObject(finishes);
            } set {
                finishes = value == null ? null : JsonConvert.DeserializeObject<string[]>(value);
            }
        }
        public bool oversized { get; set; }
        public bool promo { get; set; }
        public bool reprint { get; set; }
        public bool variation { get; set; }
        public string set_id { get; set; }
        public string set { get; set; }
        public string set_name { get; set; }
        public string set_type { get; set; }
        public string set_uri { get; set; }
        public string set_search_uri { get; set; }
        public string scryfall_set_uri { get; set; }
        public string rulings_uri { get; set; }
        public string prints_search_uri { get; set; }
        [NotMapped]
        public AlphaNumericString SortableNumber => new AlphaNumericString(collector_number);
        public string collector_number { get; set; }
        public string security_stamp { get; set; }
        public bool digital { get; set; }
        public string rarity { get; set; }
        public string card_back_id { get; set; }
        public string artist { get; set; }
        [NotMapped]
        public string[] artist_ids;
        public string ArtistIds {
            get {
                return artist_ids == null ? null : JsonConvert.SerializeObject(artist_ids);
            } set { 
                artist_ids = value == null ? null : JsonConvert.DeserializeObject<string[]>(value);
            }
        }
        public string illustration_id { get; set; }
        public string border_color { get; set; }
        public string frame_version { get; set; }
        public bool full_art { get; set; }
        public bool textless { get; set; }
        public bool booster { get; set; }
        public bool story_spotlight { get; set; }
        public int edhrec_rank { get; set; }
        public int penny_rank { get; set; }
        [NotMapped]
        public Dictionary<string, string> prices;
        public string Prices {
            get {
                return prices == null ? null : JsonConvert.SerializeObject(prices);
            } set {
                prices = value == null ? null : JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
            } 
        }
        [NotMapped]
        public Dictionary<string, string> related_uris;
        public string RelatedURIs {
            get {
                return related_uris == null ? null : JsonConvert.SerializeObject(related_uris);
            } set {
                related_uris = value == null ? null : JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
            } 
        }
        [NotMapped]
        public Dictionary<string, string> purchase_uris;
        public string PurchaseURIs { 
            get {
                return purchase_uris == null ? null : JsonConvert.SerializeObject(purchase_uris);
            } set {
                purchase_uris = value == null ? null : JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
            } 
        }
        [NotMapped]
        public ScryfallCardFace[] card_faces;
        public string CardFaces
        {
            get
            {
                return card_faces == null ? null : JsonConvert.SerializeObject(card_faces);
            } set
            {
                card_faces = value == null ? null : JsonConvert.DeserializeObject<ScryfallCardFace[]>(value);
            }
        }
        public string printed_text { get; set; }
        public string printed_name { get; set; }
        public string printed_type_line { get; set; }
    }
}
