using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Librarian
{
    public class MTGJSONCard
    {
        public string   artist { get; set; }
        public string   borderColor { get; set; }
        [NotMapped]
        public string[] colorIdentity { get; set; }
        [NotMapped]
        public string[] colorIndicator { get; set; }
        [NotMapped]
        public string[] colors { get; set; }
        public float    convertedManaCost { get; set; }
        public string   flavorText { get; set; }
        [NotMapped]
        public ForeignData[] foreignData { get; set; }
        public string   frameVersion { get; set; }
        public bool     hasFoil { get; set; }
        public bool     hasNonFoil { get; set; }
        public bool     isFoilOnly { get; set; }
        public bool     isOnlineOnly { get; set; }
        public bool     isOversized { get; set; }
        public bool     isReserved { get; set; }
        public string   layout { get; set; }
        public string   loyalty { get; set; }
        public string   manaCost { get; set; }
        public int      multiverseId { get; set; }
        public string   name { get; set; }
        [NotMapped]
        public string[] names { get; set; }
        [NotMapped]
        public AlphaNumericString SortableNumber => new AlphaNumericString(number);
        public string   number { get; set; }
        public string   originalText { get; set; }
        public string   originalType { get; set; }
        [NotMapped]
        public string[] printings { get; set; }
        public string   power { get; set; }
        public string   rarity { get; set; }
        public string   side { get; set; }
        [NotMapped]
        public string[] subtypes { get; set; }
        [NotMapped]
        public string[] supertypes { get; set; }
        public string   text { get; set; }
        public bool     timeshifted { get; set; }
        public string   toughness { get; set; }
        public string   type { get; set; }
        [NotMapped]
        public string[] types { get; set; }
        public string   uuid { get; set; }
        public string   watermark { get; set; }
        public Dictionary<string, string> legalities = new Dictionary<string, string>();
        public string   Legalities
        {
            get
            {
                if (legalities == null) return null;
                return JsonConvert.SerializeObject(legalities);
            }
            set
            {
                if (value == null) return;
                legalities = JsonConvert.DeserializeObject(value) as Dictionary<string, string>;
            }
        }
        public Dictionary<string, string>[] rulings;
        public string   Rulings
        {
            get
            {
                if (rulings == null) return null;
                return JsonConvert.SerializeObject(rulings);
            }
            set
            {
                if (value == null) return;
                rulings = JsonConvert.DeserializeObject(value) as Dictionary<string, string>[];
            }
        }
    }

    public class ForeignData
    {
        [Key]
        public int Id { get; set; }
        public string flavorText { get; set; }
        public string language { get; set; }
        public int multiverseId { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public string type { get; set; }
    }
}
