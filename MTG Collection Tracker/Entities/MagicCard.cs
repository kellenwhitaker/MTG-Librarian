using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Librarian
{
    [Table("Catalog")]
    public class MagicCard : MagicCardBase
    {
        public int CatalogID { get; set; }
        [NotMapped]
        public int CopiesOwned { get; set; }
    }
}
