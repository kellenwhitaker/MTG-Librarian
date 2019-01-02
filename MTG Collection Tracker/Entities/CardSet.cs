using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MTG_Librarian
{
    [Table("Sets")]
    public class CardSet
    {
        [Key]
        public string Name { get; set; }
        public string ScrapedName { get; set; }
        public string Code { get; set; }
        public string Code2 { get; set; }
        public string ReleaseDate { get; set; }
        public string Type { get; set; }
        public string Block { get; set; }
        public int? BaseSetSize { get; set; }
        public string MTGOCode { get; set; }
        public int? TotalSetSize { get; set; }
        [NotMapped]
        public object[] BoosterV3 { get; set; }
        public string BoosterV3Json
        {
            get => JsonConvert.SerializeObject(BoosterV3);
            set
            {
                if (value != null)
                    BoosterV3 = JsonConvert.DeserializeObject<object[]>(value);
            } 
        }
        public bool?   IsOnlineOnly { get; set; }
        [NotMapped]
        public List<MagicCard> Cards { get; set; } = new List<MagicCard>();
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
