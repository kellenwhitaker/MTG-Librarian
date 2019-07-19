using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTG_Librarian
{
    public class FullInventoryCard : MagicCardBase
    {
        [Key]
        public int InventoryId { get; set; }

        public int CollectionId { get; set; }
        public int? Count { get; set; }
        public double? Cost { get; set; }
        public string Tags { get; set; }
        public bool Foil { get; set; }
        new public string DisplayName { get; set; }
        public bool Virtual { get; set; }
        private DateTime? _timeAdded;
        public DateTime? TimeAdded { get => _timeAdded; set { _timeAdded = value; UpdateSortableTimeAdded(); } }
        private int? _insertionIndex;
        public int? InsertionIndex { get => _insertionIndex; set { _insertionIndex = value; UpdateSortableTimeAdded(); } }
        public string Condition { get; set; }
        [NotMapped]
        public string SortableTimeAdded
        {
            get
            {
                if (_sortableTimeAdded == null)
                    _sortableTimeAdded = TimeAdded.HasValue ? $"{TimeAdded.Value.ToString("s")}{InsertionIndex.ToString().PadLeft(5)}" : "";
                return _sortableTimeAdded;
            }
        }

        private string _sortableTimeAdded;

        [NotMapped]
        public string ImageKey => $"{Edition}: {rarity}";

        [NotMapped]
        public string PaddedName => DisplayName.PadRight(500);

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
                    DisplayName = DisplayName,
                    Virtual = Virtual,
                    Condition = Condition
                };
            }
        }

        private void UpdateSortableTimeAdded()
        {
            if (TimeAdded.HasValue && InsertionIndex.HasValue) _sortableTimeAdded = $"{ TimeAdded.Value.ToString("s") } { InsertionIndex.ToString().PadLeft(10) }";
        }

        public void CopyFromMagicCard(MagicCard magicCard)
        {
            DisplayName = magicCard.DisplayName;
            SetCode = magicCard.SetCode;
            Edition = magicCard.Edition;
            tcgplayerMarketPrice = magicCard.tcgplayerMarketPrice;
            tcgplayerLowPrice = magicCard.tcgplayerLowPrice;
            tcgplayerMidPrice = magicCard.tcgplayerMidPrice;
            tcgplayerHighPrice = magicCard.tcgplayerHighPrice;
            PartB = magicCard.PartB;
            artist = magicCard.artist;
            borderColor = magicCard.borderColor;
            colorIdentity = magicCard.colorIdentity;
            colorIndicator = magicCard.colorIndicator;
            colors = magicCard.colors;
            convertedManaCost = magicCard.convertedManaCost;
            flavorText = magicCard.flavorText;
            foreignData = magicCard.foreignData;
            frameVersion = magicCard.frameVersion;
            hasFoil = magicCard.hasFoil;
            hasNonFoil = magicCard.hasNonFoil;
            isFoilOnly = magicCard.isFoilOnly;
            isOnlineOnly = magicCard.isOnlineOnly;
            isOversized = magicCard.isOversized;
            isReserved = magicCard.isReserved;
            layout = magicCard.layout;
            loyalty = magicCard.loyalty;
            manaCost = magicCard.manaCost;
            multiverseId = magicCard.multiverseId;
            name = magicCard.name;
            names = magicCard.names;
            number = magicCard.number;
            originalText = magicCard.originalText;
            originalType = magicCard.originalType;
            printings = magicCard.printings;
            power = magicCard.power;
            rarity = magicCard.rarity;
            side = magicCard.side;
            subtypes = magicCard.subtypes;
            supertypes = magicCard.supertypes;
            tcgplayerProductId = magicCard.tcgplayerProductId;
            text = magicCard.text;
            timeshifted = magicCard.timeshifted;
            toughness = magicCard.toughness;
            type = magicCard.type;
            types = magicCard.types;
            uuid = magicCard.uuid;
            watermark = magicCard.watermark;
            legalities = magicCard.legalities;
            rulings = magicCard.rulings;
        }
    }
}