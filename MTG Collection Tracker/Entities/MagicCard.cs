using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace MTG_Librarian
{
    [Table("Catalog")]
    public class ScryfallMagicCard : ScryfallMagicCardBase
    {        
        [NotMapped]
        public int CopiesOwned { get; set; }
        [NotMapped]
        public double? Price { get; set; }
    }
}
