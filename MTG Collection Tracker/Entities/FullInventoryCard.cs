using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Collection_Tracker
{
    public class FullInventoryCard : MagicCardBase
    {
        [Key]
        public int          CardInstanceId { get => InventoryCard.CardInstanceId; set => InventoryCard.CardInstanceId = value; }
        public int?         CatalogID { get => InventoryCard.CatalogID; set => InventoryCard.CatalogID = value; }
        public int          CollectionId { get => InventoryCard.CollectionId; set => InventoryCard.CollectionId = value; }
        public int          MVid { get => InventoryCard.MVid; set =>InventoryCard.MVid = value; }
        public int?         Count { get =>InventoryCard.Count; set =>InventoryCard.Count = value; }
        public double?      Cost { get =>InventoryCard.Cost; set =>InventoryCard.Cost = value; }
        public string       Tags { get =>InventoryCard.Tags; set =>InventoryCard.Tags = value; }
        public DateTime?    TimeAdded { get =>InventoryCard.TimeAdded; set {InventoryCard.TimeAdded = value; UpdateSortableTimeAdded(); } }
        public int?         InsertionIndex { get =>InventoryCard.InsertionIndex; set {InventoryCard.InsertionIndex = value; UpdateSortableTimeAdded();  } }
        [NotMapped]
        public String       SortableTimeAdded { get; set; }
                            
        [NotMapped]
        public string       ImageKey => $"{Edition}: {rarity}";
        [NotMapped]
        public string       PaddedName => name.PadRight(500);
        [NotMapped]
        public InventoryCard InventoryCard { get; set; } = new InventoryCard();

        private void UpdateSortableTimeAdded()
        {
            if (TimeAdded.HasValue) SortableTimeAdded = $"{ TimeAdded.Value.ToString("s") } {InsertionIndex.ToString().PadLeft(5) }";
            //else return "";
        }
    }
}
