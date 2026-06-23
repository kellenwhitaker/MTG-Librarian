using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Librarian
{
    [Table("Library")]
    public class InventoryCard
    {
        [Key]
        public int          InventoryId { get; set; }
        public int          CollectionId { get; set; }
        public int?         Count { get; set; }
        public double?      Cost { get; set; }
        public string       Tags { get; set; }
        public DateTime?    TimeAdded { get; set; }
        public int?         InsertionIndex { get; set; }
        public string       ScryfallId { get; set; }
        public bool         Foil { get; set; }
        public string       PartB_ScryfallId { get; set; }
        [NotMapped]
        public string       DisplayName { get; set; }
        public bool         Virtual { get; set; }
        public string       Condition { get; set; }
        public string       Finish { get; set; }
        public string       Platform { get; set; }
        public string       Board { get; set; }
        public bool?        IsCommander { get; set; }
    }
}
