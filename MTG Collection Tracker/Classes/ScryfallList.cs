using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    internal class ScryfallSetList
    {
        public string Object { get; set; }
        public ScryfallCardSet[] data { get; set; }
        public bool has_more { get; set; }
        public string next_page { get; set; }
        public int total_cards { get; set; }
        public string[] warnings { get; set; }
    }

    internal class ScryfallCardList
    {
        public string Object { get; set; }
        public ScryfallCard[] data { get; set; }
        public bool has_more { get; set; }
        public string next_page { get; set; }
        public int total_cards { get; set; }
        public string[] warnings { get; set; }
        public int status { get; set; }
    }

    internal class  ScryfallRulingsList
    {
        public string Object { get; set; }
        public ScryfallCardRuling[] data { get; set; }
        public bool has_more { get; set; }
        public string next_page { get; set; }
        public int total_cards { get; set; }
        public string[] warnings { get; set; }
        public int status { get; set; }
    }

    public class ScryfallCardRuling
    {
        public string Object { get; set; }
        public string oracle_id { get; set; }
        public string source { get; set; }
        public string published_at { get; set; }
        public string comment { get; set; }
    }
}
