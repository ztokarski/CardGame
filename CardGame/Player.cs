using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> Discarded { get; set; }
        public List<Trick> TakenTricks { get; set; }
        public int TotalPoints { get; set; }
        
        
        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
            Discarded = new List<Card>();
            TakenTricks = new List<Trick>();
            TotalPoints = 0;
        }

        public List<Card> TakenCards()
        {
            var takenCards = new List<Card>();
            foreach (var trick in TakenTricks)
            {
                foreach (var card in trick.Cards)
                {
                    takenCards.Add(card);
                }
            }
            return takenCards;
        }

        public int EvaluatePoints()
        {
            var points = TakenCards().Select(z => z.ValueInTrick).Sum();
            return points;
        }

        public List<Card> SortedHand()
        {
            return Hand.OrderBy(z => z.Suit).ThenByDescending(z => z.Rank).ToList(); ;
        }

        public void Discard(Card card)
        {
            Hand.Remove(card);
            Discarded.Add(card);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
