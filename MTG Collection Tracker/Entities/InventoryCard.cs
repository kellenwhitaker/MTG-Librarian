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
        public int          multiverseId_Inv { get; set; }
        public int?         Count { get; set; }
        public double?      Cost { get; set; }
        public string       Tags { get; set; }
        public DateTime?    TimeAdded { get; set; }
        public int?         InsertionIndex { get; set; }
        public string       uuid { get; set; }
        public bool         Foil { get; set; }
        public string       PartB_uuid { get; set; }
        public string       DisplayName { get; set; }
        public bool         Virtual { get; set; }
    }
}
