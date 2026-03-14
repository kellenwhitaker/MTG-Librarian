using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MTG_Librarian
{
    [Table("Catalog")]
    public class ScryfallMagicCard : ScryfallMagicCardBase
    {        
        [NotMapped]
        public int CopiesOwned { get; set; }
    }
}
