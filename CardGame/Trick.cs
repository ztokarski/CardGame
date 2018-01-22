using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Trick
    {
        public Player FirstPlayer { get; set; }
        public Card FirstCard { get; set; }
        public List<Card> Cards {get; set;}
        public Card WinningCard { get; set; }

        public Trick()
        {
            FirstPlayer = null;
            FirstCard = null;
            Cards = new List<Card>();
            WinningCard = null;
        }
    }
}
