using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public class ScryfallSearchCollection
    {
        public List<ScryfallSearchCollectionIdentifier> identifiers { get; set; } = new List<ScryfallSearchCollectionIdentifier>();
    }

    public class ScryfallSearchCollectionIdentifier
    {
        public string id { get; set; }
        public string mtgo_id { get; set; }
        public string multiverse_id { get; set; }
        public string oracle_id { get; set; }
        public string illlustration_id { get; set; }
        public string name { get; set; }
        public string set { get; set; }
        public string collector_number { get; set; }
    }
}
