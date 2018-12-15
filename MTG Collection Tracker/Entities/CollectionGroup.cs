using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Librarian
{
    [Table("CollectionGroups")]
    public class CollectionGroup
    {
        [Key]
        public int      Id { get; set; }
        public string   GroupName { get; set; }
        public bool     Permanent { get; set; }
    }
}
