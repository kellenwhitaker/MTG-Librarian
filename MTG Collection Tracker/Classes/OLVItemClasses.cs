using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Collection_Tracker
{
    public class OLVCardItem : OLVItem
    {
        public string Name;
        private MagicCard mCard;
        public MagicCard MagicCard => mCard;
        public string Type => mCard.type;
        public string Cost => mCard.manaCost;
        public string Set => mCard.Edition;
        public string CollectorNumber => mCard.number;
        public string Rarity => mCard.rarity;
        public override string ImageKey => $"{mCard.Edition}: {mCard.rarity}";
        public override OLVItem Parent { get; set; }
        public override Predicate<object> Filter => throw new NotImplementedException();

        public OLVCardItem(string name)
        {
            Name = name;
        }

        public OLVCardItem(MagicCard mCard)
        {
            this.mCard = mCard;
            Name = mCard.name;
        }
    }

    public class OLVRarityItem : OLVItem, IComparable<OLVRarityItem>
    {
        public string Rarity;
        public string Set;
        public int Count;
        public string Text => ToString();
        public override OLVItem Parent { get; set; }
        public override string ImageKey => Rarity != "Basic Land" ? $"{Set}: {Rarity}" : null;
        public override Predicate<object> Filter => x => (x as OLVCardItem).Rarity == Rarity;

        public int SortValue;

        public OLVRarityItem(OLVItem parent, string set, string rarity)
        {
            Parent = parent;
            Set = set;
            Rarity = rarity;
            Count = 0;
            switch (rarity)
            {
                case "Common": SortValue = 0; break;
                case "Uncommon": SortValue = 1; break;
                case "Rare": SortValue = 2; break;
                case "Mythic Rare": SortValue = 3; break;
                default: SortValue = 4; break;
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
        public List<OLVCardItem> Cards;
        public List<OLVRarityItem> Rarities;
        public bool Expanded;
        public override OLVItem Parent { get; set; }
        public override Predicate<object> Filter => x => (x as OLVCardItem).Set == Name;
        public override string ImageKey => $"{Rarities.Where(x => x.ImageKey != null).Last().ImageKey}";
        public string Text
        {
            get
            {
                int cardCount = (from r in Rarities
                                 select r.Count).Sum();
                return $"{Name} [{cardCount}]";
            }
        }

        public OLVSetItem(string name)
        {
            Name = name;
            Cards = new List<OLVCardItem>();
            Rarities = new List<OLVRarityItem>();
        }

        public void AddCard(MagicCard card)
        {
            if (card.side != "A") return;
            if (!Rarities.Exists(x => x.Rarity == card.rarity))
            {
                Rarities.Add(new OLVRarityItem(this, card.Edition, card.rarity));
                Rarities.Sort();
            }
            var item = Rarities.Where(x => x.Rarity == card.rarity).First();
            item.Count++;
            Cards.Add(new OLVCardItem(card));
        }

        public void AddRarity(OLVRarityItem rarity)
        {
            Rarities.Add(rarity);
        }
    }

    public abstract class OLVItem
    {
        public abstract OLVItem Parent { get; set; }
        public abstract string ImageKey { get; }
        public abstract Predicate<object> Filter { get; }
    }
}
