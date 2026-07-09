using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Librarian
{
    public class PriceHistory
    {
        [Key]
        public int rowid { get; set; }
        public string ScryfallId { get; set; }
        [NotMapped]
        public Dictionary<string, string> prices;
        public string Prices
        {
            get
            {
                return prices == null ? null : JsonConvert.SerializeObject(prices);
            }
            set
            {
                prices = value == null ? null : JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
            }
        }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
