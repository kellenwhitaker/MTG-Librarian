using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Librarian
{
    [Table("Catalog")]
    public class MagicCard : MagicCardBase
    {
        [NotMapped]
        public int     CopiesOwned { get; set; }
    }
}
