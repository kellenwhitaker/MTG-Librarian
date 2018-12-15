using System.ComponentModel.DataAnnotations;

namespace MTG_Librarian
{
    public class DbCardImage
    {
        [Key]
        public string uuid { get; set; }
        public byte[] CardImageBytes { get; set; }
    }
}
