using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Card
    {
        public string Symbol { get; set; }
        public string Suit { get; set; }
        public int Rank { get; set; }
        public int ValueInTrick { get; set; }

        public Card(string symbol, string suit)
        {
            Symbol = symbol;
            Suit = suit;
            Rank = EvaluateRank();
            ValueInTrick = EvaluateValue();
        }

        private int EvaluateValue()
        {
            switch (Suit)
            {
                case "♥":
                    return 1;
                case "♠":
                    return EvaluateSpadesValue();
                default:
                    return 0;
             }
        }

        private int EvaluateSpadesValue()
        {
            switch (Symbol)
            {
                case "A":
                    return 7;
                case "K":
                    return 10;
                case "Q":
                    return 13;
                default:
                    return 0;
            }
        }

        private int EvaluateRank()
        {
            switch (Symbol)
            {
                case "A":
                    return 14;
                case "K":
                    return 13;
                case "Q":
                    return 12;
                case "J":
                    return 11;
                default:
                    return Int32.Parse(Symbol);
            }
        }

        public override string ToString()
        {
            return String.Format("{0}{1}",Symbol, Suit);
        }
    }
}
