using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public class InventoryCardCluster : InventoryCard
    {
        public List<InventoryCard> Cards { get; set; } = new List<InventoryCard>();
        
        public InventoryCardCluster(InventoryCard card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card), "Card cannot be null.");
            this.Name = card.Name;
            this.type_line = card.type_line;
            this.mana_cost = card.mana_cost;
            this.cmc = card.cmc;
            this.card_faces = card.card_faces;
            this.InventoryId = card.InventoryId;
            this.CollectionId = card.CollectionId;
            this.TimeAdded = card.TimeAdded;
            this.InsertionIndex = card.InsertionIndex;
            this.Virtual = card.Virtual;
            this.Platform = card.Platform;
            this.Board = card.Board;
            this.IsCommander = card.IsCommander;            
            Cards.Add(card);
        }
        public InventoryCardCluster(List<InventoryCard> cards)
        {
            if (cards == null || cards.Count == 0)
                throw new ArgumentException("Cards list cannot be null or empty.", nameof(cards));

            var card = cards[0];
            
            this.Name = card.Name;
            this.type_line = card.type_line;
            this.mana_cost = card.mana_cost;
            this.cmc = card.cmc;
            this.card_faces = card.card_faces;
            this.CollectionId = card.CollectionId;
            this.TimeAdded = card.TimeAdded;
            this.InsertionIndex = card.InsertionIndex;
            this.Virtual = card.Virtual;
            this.Platform = card.Platform;
            this.Board = card.Board;
            this.IsCommander = card.IsCommander;            
            
            Cards.AddRange(cards);
        }

        public new int Count => (int)Cards.Sum(c => c.Count);
        public new double Cost => (double)(Cards.Count > 0 ? Cards.Sum(c => c.Cost) / Cards.Count : 0);
        public new double Price => (double)(Cards.Count > 0 ? Cards.Sum(c => c.Price) / Cards.Count : 0);
        public new string Tags => Cards.All(x => x.Tags == Cards[0].Tags) ? Cards[0].Tags : null;
        public new string Condition => Cards.All(x => x.Condition == Cards[0].Condition) ? Cards[0].Condition : null;
        public new string Finish => Cards.All(x => x.Finish == Cards[0].Finish) ? Cards[0].Finish : null;
    }
}
