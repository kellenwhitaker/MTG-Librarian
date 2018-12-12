using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Collection_Tracker
{
    [Table("Sets")]
    public class CardSet
    {
        [Key]
        public string   Name { get; set; }
        public string   Code { get; set; }
        public string   Code2 { get; set; }
        public string   ReleaseDate { get; set; }
        public string   Type { get; set; }
        public string   Block { get; set; }
        [NotMapped]
        public List<MagicCard> Cards { get; set; } = new List<MagicCard>();
        [NotMapped]
        public string   MTGJSONURL { get; set; }
        [NotMapped]
        public Image    CommonIcon { get; set; }
        public byte[]   CommonIconBytes { get => CommonIcon?.GetCopyOf().ToByteArray(); set => CommonIcon = ImageExtensions.FromByteArray(value); }
        [NotMapped]
        public Image    UncommonIcon { get; set; }
        public byte[]   UncommonIconBytes { get => UncommonIcon?.GetCopyOf().ToByteArray(); set => UncommonIcon = ImageExtensions.FromByteArray(value); }
        [NotMapped]
        public Image    RareIcon { get; set; }
        public byte[]   RareIconBytes { get => RareIcon?.GetCopyOf().ToByteArray(); set => RareIcon = ImageExtensions.FromByteArray(value); }
        [NotMapped]
        public Image    MythicRareIcon { get; set; }
        public byte[]   MythicRareIconBytes { get => MythicRareIcon?.GetCopyOf().ToByteArray(); set => MythicRareIcon = ImageExtensions.FromByteArray(value); }
    }
}
