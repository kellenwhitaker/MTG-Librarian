using System.ComponentModel.DataAnnotations;

namespace MTG_Collection_Tracker
{
    public class DbCardImage
    {
        [Key]
        public string uuid { get; set; }
        public byte[] CardImageBytes { get; set; }
    }
}
