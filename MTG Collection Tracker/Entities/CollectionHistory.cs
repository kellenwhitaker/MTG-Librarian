using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Librarian
{
    public class CollectionHistory
    {
        [Key]
        public int rowid { get; set; }
        public int InventoryId { get; set; }
        public int? SourceCollectionId { get; set; } = null;
        public int DestinationCollectionId { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
