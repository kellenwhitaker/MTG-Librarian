using System;
using System.Collections.Generic;
using System.Linq;

namespace MTG_Librarian
{
    public class OLVCardItem : OLVItem
    {
        public string    DisplayName
        {
            get => MagicCard.DisplayName;            
            set =>  MagicCard.DisplayName = value;
        }
        public string    Name => MagicCard.name;
        public MagicCard MagicCard { get; set; }
        public string    Type => MagicCard.type;
        public string    ManaCost => MagicCard.manaCost;
        public string    Set => MagicCard.Edition;
        public string    CollectorNumber => MagicCard.number;
        public string    Rarity => MagicCard.rarity;
        public int       CopiesOwned => MagicCard.CopiesOwned;
        public override string ImageKey => $"{MagicCard.Edition}: {MagicCard.rarity}";
        public override OLVItem Parent { get; set; }
        public override Predicate<object> Filter => throw new NotImplementedException();

        public OLVCardItem(MagicCard mCard)
        {
            MagicCard = mCard;
        }
    }

    public class OLVRarityItem : OLVItem, IComparable<OLVRarityItem>
    {
        public List<OLVCardItem> Cards { get; set; } = new List<OLVCardItem>();
        public string Rarity { get; set; }
        public string Set { get; set; }
        public int Count => Cards?.Count ?? 0;
        public string Complete => $"{100 * Cards.Count(x => x.CopiesOwned > 0) / Cards.Count}%";
        public string Complete4 => $"{100 * Cards.Count(x => x.CopiesOwned > 3) / Cards.Count}%";
        public string Text => ToString();
        public override OLVItem Parent { get; set; }
        public override string ImageKey => Rarity != "Basic Land" ? $"{Set}: {Rarity}" : null;
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
        public DateTime ReleaseDate => DateTime.TryParse(CardSet.ReleaseDate, out DateTime value) ? value : DateTime.MinValue;
        public List<OLVCardItem> Cards = new List<OLVCardItem>();
        public List<OLVRarityItem> Rarities = new List<OLVRarityItem>();
        public CardSet CardSet { get; set; }
        public override OLVItem Parent { get; set; }
        public override Predicate<object> Filter => x => (x as OLVCardItem).Set == Name;
        public override string ImageKey => $"{Rarities.Last(x => x.ImageKey != null).ImageKey}";
        public int CardCount => Cards.Count;
        public string Complete => $"{100 * Cards.Count(x => x.CopiesOwned > 0) / Cards.Count}%";
        public string Complete4 => $"{100 * Cards.Count(x => x.CopiesOwned > 3) / Cards.Count}%";
        public string Text => $"{Name} [{CardCount}]".PadRight(500);
        public OLVSetItem(CardSet set)
        {
            Name = set.Name;
            CardSet = set;
        }

        public OLVSetItem(string name)
        {
            Name = name;
        }

        public void BuildRarityItems()
        {
            foreach (var cardItem in Cards)
            {
                if (!(Rarities.FirstOrDefault(x => x.Rarity == cardItem.MagicCard.rarity)is OLVRarityItem rarityItem))
                {
                    rarityItem = new OLVRarityItem(this, cardItem.MagicCard.Edition, cardItem.MagicCard.rarity);
                    Rarities.Add(rarityItem);
                    Rarities.Sort();
                }
                rarityItem.Cards.Add(cardItem);
            }
        }

        public void AddCard(MagicCard card)
        {
            Cards.Add(new OLVCardItem(card));
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
        public double tcgplayerMarketPrice;
        public double Cost;
        public int Count;
    }

    public abstract class OLVItem
    {
        public abstract OLVItem Parent { get; set; }
        public abstract string ImageKey { get; }
        public abstract Predicate<object> Filter { get; }
    }
}
