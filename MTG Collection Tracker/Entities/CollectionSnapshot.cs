using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public class CollectionSnapshot
    {
        [Key]
        public int rowid { get; set; }
        public int CollectionId { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public int? Count { get; set; }
        public double? Cost { get; set; }
        public double? Price { get; set; }
    }
}
