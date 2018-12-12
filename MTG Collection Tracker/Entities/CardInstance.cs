using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Collection_Tracker
{
    public class CardInstance : MagicCardBase
    {
        [Key]
        public int          CardInstanceId { get => DBCardInstance.CardInstanceId; set => DBCardInstance.CardInstanceId = value; }
        public int?         CatalogID { get => DBCardInstance.CatalogID; set => DBCardInstance.CatalogID = value; }
        public int          CollectionId { get => DBCardInstance.CollectionId; set => DBCardInstance.CollectionId = value; }
        public int          MVid { get => DBCardInstance.MVid; set => DBCardInstance.MVid = value; }
        public int?         Count { get => DBCardInstance.Count; set => DBCardInstance.Count = value; }
        public double?      Cost { get => DBCardInstance.Cost; set => DBCardInstance.Cost = value; }
        public string       Tags { get => DBCardInstance.Tags; set => DBCardInstance.Tags = value; }
        public DateTime?    TimeAdded { get => DBCardInstance.TimeAdded; set { DBCardInstance.TimeAdded = value; UpdateSortableTimeAdded(); } }
        public int?         InsertionIndex { get => DBCardInstance.InsertionIndex; set { DBCardInstance.InsertionIndex = value; UpdateSortableTimeAdded();  } }
        [NotMapped]
        public String       SortableTimeAdded { get; set; }
                            
        [NotMapped]
        public string       ImageKey => $"{Edition}: {rarity}";
        [NotMapped]
        public string       PaddedName => name.PadRight(500);
        [NotMapped]
        public DBCardInstance DBCardInstance { get; set; } = new DBCardInstance();

        private void UpdateSortableTimeAdded()
        {
            if (TimeAdded.HasValue) SortableTimeAdded = $"{ TimeAdded.Value.ToString("s") } {InsertionIndex.ToString().PadLeft(5) }";
            //else return "";
        }
    }
}
