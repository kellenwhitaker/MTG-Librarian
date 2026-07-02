using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public class CardLibrary
    {
        List<ScryfallMagicCardBase> mainboard = new List<ScryfallMagicCardBase>();
        private List<LiveMagicCard> library = new List<LiveMagicCard>();
        public CardLibrary(List<ScryfallMagicCardBase> mainboard)
        {
            this.mainboard = mainboard;
            Shuffle();
        }
        public List<LiveMagicCard> GetLibrary()
        {
            return library;
        }
        public bool IsEmpty()
        {
            return library.Count == 0;
        }
        public void PlaceCardOnTop(LiveMagicCard card)
        {
            library.Add(card);
        }
        public void PlaceCardOnBottom(LiveMagicCard card)
        {
            library.Insert(0, card);
        }
        public void Remove(LiveMagicCard card)
        {
            library.Remove(card);
        }
        public void Shuffle()
        {
            Random rng = new Random();
            var shuffled = mainboard.OrderBy(a => rng.Next()).ToList();
            library = new List<LiveMagicCard>();
            foreach (var card in shuffled)
                library.Add(new LiveMagicCard(card));
        }
        public void Reshuffle()
        {
            Random rng = new Random();
            var shuffled = library.OrderBy(a => rng.Next()).ToList();
            library = shuffled;
        }
        public LiveMagicCard Draw()
        {
            var card = library[library.Count - 1];
            library.RemoveAt(library.Count - 1);
            return card;
        }
        public List<LiveMagicCard> DrawHand()
        {
            var hand = new List<LiveMagicCard>();
            for (int i = 0; i < 7; i++)
            {
                hand.Add(Draw());
            }
            return hand;
        }
    }
}
