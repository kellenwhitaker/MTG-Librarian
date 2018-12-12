using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Collection_Tracker
{
    [Table("Library")]
    public class InventoryCard
    {
        [Key]
        public int          CardInstanceId { get; set; }
        public int?         CatalogID { get; set; }
        public int          CollectionId { get; set; }
        public int          MVid { get; set; }
        public int?         Count { get; set; }
        public double?      Cost { get; set; }
        public string       Tags { get; set; }
        public DateTime?    TimeAdded { get; set; }
        public int?         InsertionIndex { get; set; }
        public string       uuid { get; set; }
    }
}
