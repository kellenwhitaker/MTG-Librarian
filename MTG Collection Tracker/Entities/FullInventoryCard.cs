﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Librarian
{
    public class FullInventoryCard : MagicCardBase
    {
        [Key]
        public int          InventoryId { get; set; }
        public int          CollectionId { get; set; }
        public int?         Count { get; set; }
        public double?      Cost { get; set; }
        public string       Tags { get; set; }
        public bool         Foil { get; set; }
        public string       DisplayName { get; set; }
        private DateTime?   _timeAdded;
        public DateTime?    TimeAdded { get => _timeAdded; set { _timeAdded = value; UpdateSortableTimeAdded(); } }
        private int?        _insertionIndex;
        public int?         InsertionIndex { get => _insertionIndex; set { _insertionIndex = value; UpdateSortableTimeAdded(); } }
        [NotMapped]
        public string       SortableTimeAdded
        {
            get
            {
                if (_sortableTimeAdded == null)
                    _sortableTimeAdded = TimeAdded.HasValue ? $"{ TimeAdded.Value.ToString("s") } { InsertionIndex.ToString().PadLeft(5) }" : "";
                return _sortableTimeAdded;               
            }
        }
        private string      _sortableTimeAdded = null;
        [NotMapped]
        public string       ImageKey => $"{Edition}: {rarity}";
        [NotMapped]
        public string       PaddedName => DisplayName.PadRight(500);
        [NotMapped]
        public InventoryCard InventoryCard
        {
            get
            {
                return new InventoryCard
                {
                    CollectionId = CollectionId,
                    Cost = Cost,
                    Count = Count,
                    InsertionIndex = InsertionIndex,
                    InventoryId = InventoryId,
                    multiverseId_Inv = multiverseId,
                    Tags = Tags,
                    TimeAdded = TimeAdded,
                    uuid = uuid,
                    Foil = Foil,
                    DisplayName = DisplayName
                };
            }
        }

        private void UpdateSortableTimeAdded()
        {
            if (TimeAdded.HasValue && InsertionIndex.HasValue) _sortableTimeAdded = $"{ TimeAdded.Value.ToString("s") } { InsertionIndex.ToString().PadLeft(10) }";
        }
    }
}
