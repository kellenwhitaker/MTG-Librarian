using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Librarian
{
    public class CardQuantityHistory
    {
        [Key]
        public int rowid { get; set; }
        public int InventoryId { get; set; }
        public int Quantity { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
