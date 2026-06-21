using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace MTG_Librarian
{
    public class OLVCardItem : OLVItem
    {
        public string DisplayName
        {
            get => MagicCard.DisplayName;
        }
        public string DisplayTypeLine
        {
            get => MagicCard.DisplayTypeLine;
        }
        public string DisplayText
        {
            get => MagicCard.DisplayText;
        }

        public string Name => MagicCard.Name;
        public ScryfallMagicCard MagicCard { get; set; }
        public string Type => MagicCard.type_line;
        public string ManaCost => MagicCard.mana_cost;
        public string Set => MagicCard.set_name;
        public string CollectorNumber => MagicCard.collector_number;
        public AlphaNumericString SortableNumber => new AlphaNumericString(MagicCard.collector_number);
        public string Rarity => MagicCard.rarity;
        public int CopiesOwned => MagicCard.CopiesOwned;
        public double? Price => MagicCard.Price;
        public string Text => MagicCard.text;
        public override string ImageKey => $"{MagicCard.SymbolCode}: {MagicCard.rarity}";
        public override OLVItem Parent { get; set; }
        public override Predicate<object> Filter => throw new NotImplementedException();

        public OLVCardItem(ScryfallMagicCard mCard)
        {
            MagicCard = mCard;
        }
    }

    public class OLVRarityItem : OLVItem, IComparable<OLVRarityItem>
    {
        public List<ScryfallMagicCard> Cards { get; set; } = new List<ScryfallMagicCard>();
        public string Rarity { get; set; }
        public string Set { get; set; }
        public int Count => Cards?.Count ?? 0;
        public string Complete => $"{100 * Cards.Count(x => x.CopiesOwned > 0) / Cards.Count}%";
        public string Complete4 => $"{100 * Cards.Count(x => x.CopiesOwned > 3) / Cards.Count}%";
        public string Text => ToString();
        public override OLVItem Parent { get; set; }
        public override string ImageKey => $"{Set}: {Rarity}";
        public override Predicate<object> Filter => x => (x as OLVCardItem).Rarity == Rarity;
        public string ReleaseDate => SortValue.ToString(); // ensures correct sorting when TreeListView is sorted by ReleaseDate

        public int SortValue;

        public OLVRarityItem(OLVItem parent, string set, string rarity)
        {
            Parent = parent;
            Set = set;
            Rarity = rarity;
            switch (rarity)
            {
                case "basic land": SortValue = 0; break;
                case "common": SortValue = 1; break;
                case "uncommon": SortValue = 2; break;
                case "rare": SortValue = 3; break;
                case "mythic": SortValue = 4; break;
                default: SortValue = 5; break;
            }
        }

        public override string ToString()
        {
            return $"{Rarity} [{Count}]";
        }

        public int CompareTo(OLVRarityItem other)
        {
            return SortValue.CompareTo(other.SortValue);
        }
    }

    public class OLVSetItem : OLVItem
    {
        public string Name;
        public DateTime ReleaseDate => DateTime.TryParse(CardSet.released_at, out DateTime value) ? value : DateTime.MinValue;
        public Dictionary<string, ScryfallMagicCard> Cards = new Dictionary<string, ScryfallMagicCard>();
        public List<OLVRarityItem> Rarities = new List<OLVRarityItem>();
        public ScryfallCardSet CardSet { get; set; } = new ScryfallCardSet();
        public override OLVItem Parent { get; set; }
        public override Predicate<object> Filter => x => (x as OLVCardItem).Set == Name;
        public override string ImageKey 
        {
            get
            {
                string Rarity;
                if (CardSet.MythicRareIcon != null)
                    Rarity = "mythic";
                else if (CardSet.RareIcon != null)
                    Rarity = "rare";
                else if (CardSet.UncommonIcon != null)
                    Rarity = "uncommon";
                else
                    Rarity = "common";

                return $"{CardSet.SymbolCode}: {Rarity}"; ;
            }
        }
        public int CardCount => CardSet != null ? CardSet.card_count : 0;
        public string Complete => CardCount > 0 ? $"{100 * Cards.Values.Count(x => x.CopiesOwned > 0) / CardCount}%" : "0%";
        public string Complete4 => CardCount > 0 ? $"{100 * Cards.Values.Count(x => x.CopiesOwned > 3) / CardCount}%" : "0%";
        public string Text => $"{Name} [{CardCount}]".PadRight(500);

        public OLVSetItem(ScryfallCardSet set)
        {
            Name = set.name;
            CardSet = set;
        }

        public OLVSetItem(string name)
        {
            Name = name;
        }

        public void AddCard(ScryfallMagicCard card)
        {
            ScryfallMagicCard existing;
            if (Cards.TryGetValue(card.collector_number, out existing))
                existing.CopiesOwned += card.CopiesOwned;
            else
            {
                Cards.Add(card.collector_number, card);
            }
        }

        public void AddRarity(OLVRarityItem rarity)
        {
            Rarities.Add(rarity);
        }
    }

    public class InventoryTotalsItem : OLVItem
    {
        public override OLVItem Parent { get; set; }
        public override string ImageKey => null;
        public override Predicate<object> Filter => throw new NotImplementedException();
        public string DisplayName { get; set; }
        public string PaddedName => DisplayName.PadRight(500);
        public double Price;
        public double Cost;
        public int Count;
        public double Delta => Price - Cost;
        public double Percent => Cost != 0.0 ? 100 * Delta / Cost : 0.0;
    }

    public class OLVAttributeItem : OLVItem
    {
        public override OLVItem Parent { get; set; }
        public override string ImageKey => null;
        public override Predicate<object> Filter => throw new NotImplementedException();
        public string Attribute { get; set; }
        public string Description { get; set; }
        public bool Not { get; set; } = false;
    }

    public abstract class OLVItem
    {
        public abstract OLVItem Parent { get; set; }
        public abstract string ImageKey { get; }
        public abstract Predicate<object> Filter { get; }
    }
}