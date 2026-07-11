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
        public Dictionary<string, string> prices
        {
            get
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(Prices);
            }
            set
            {
                Prices = JsonConvert.SerializeObject(value);
            }
        }
        public string Prices { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
