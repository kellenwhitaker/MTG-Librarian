using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Librarian
{
    [Table("Collections")]
    public class CardCollection
    {
        [Key]
        public int      Id { get; set; }
        public string   CollectionName { get; set; }
        public string   GroupName { get; set; }
        public int      GroupId { get; set; }
        public string   Type { get; set; }
        public bool     Virtual { get; set; }
        public bool     Permanent { get; set; }
    }
}
