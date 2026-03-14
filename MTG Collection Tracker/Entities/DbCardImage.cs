using System.ComponentModel.DataAnnotations;

namespace MTG_Librarian
{
    public class DbCardImage
    {
        [Key]
        public string ScryfallId { get; set; }
        public string Side { get; set; }
        public byte[] CardImageBytes { get; set; }
    }
}
