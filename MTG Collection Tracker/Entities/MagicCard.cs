using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Collection_Tracker
{
    [Table("Catalog")]
    public class MagicCard : MagicCardBase
    {
        [Key]
        public int CatalogID { get; set; }
        public int multiverseId { get; set; }
    }
}
