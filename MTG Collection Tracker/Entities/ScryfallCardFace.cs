using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public class ScryfallCardFace
    {
        [JsonProperty("object")]
        public string obj { get; set; }
        public string artist { get; set; }
        public string artist_id { get; set; }
        public double? cmc { get; set; }

        public string[] color_indicator;

        public string[] colors;

        //public string defense { get; set; }
        public string flavor_text { get; set; }
        public string illustration_id { get; set; }
        public Dictionary<string, string> image_uris { get; set; }
        public string layout { get; set; }
        public string loyalty { get; set; }
        public string mana_cost { get; set; }
        public string name { get; set; }
        public string oracle_id { get; set; }
        public string oracle_text { get; set; }
        public string power { get; set; }
        public string printed_name { get; set; }
        public string printed_text { get; set; }
        public string printed_type_line { get; set; }
        public string toughness { get; set; }
        public string type_line { get; set; }
        public string watermark { get; set; }
        public string DisplayName => printed_name != null ? printed_name : name;
        public string DisplayTypeLine => printed_type_line != null ? printed_type_line : type_line;
        public string DisplayText => printed_text != null ? printed_text : oracle_text;
    }
}
