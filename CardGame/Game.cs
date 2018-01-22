using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public List<Card> DrawPile { get; set; }
        public List<Trick> Tricks {get; set;}
        public List<Player> ComputerPlayers { get; set; }
        public static List<Card> AllPlayedCards { get; set; }
       
        private Game(List<Player> players, List<Card> drawPile)
        {
            Players = players;
            DrawPile = drawPile;
            Tricks = new List<Trick>();
            AllPlayedCards = new List<Card>();
            ComputerPlayers = new List<Player>() { Players[1], Players[2], Players[3] };
        }

        public static Game CreateInstance()
        {
            List<Player> Players = CreatePlayers();
            List<Card> DrawPile = CreateCardDeck();
            return new Game(Players, DrawPile);
        }

        public static Game CreateInstanceOfNextRound(Game game)
        {
            var oldPoints = new List<int>();
            for (int i = 0; i < game.Players.Count(); i++)
            {
                oldPoints.Add( game.Players[i].TotalPoints);
            }

            List<Player> Players = new List<Player> { new Player(game.Players[0].Name), new Player(game.Players[1].Name), new Player(game.Players[2].Name), new Player(game.Players[3].Name) };

            for (int i = 0; i < game.Players.Count(); i++)
            {
                Players[i].TotalPoints += oldPoints[i];
            }
            List<Card> DrawPile = CreateCardDeck();

            return new Game(Players, DrawPile);
        }

        public static List<Player> CreatePlayers()
        {
            GameView.PlayerNameQuestion();
            Player Player1 = GetInput.PlayerName();
            var Player2 = new Player("Kryspin");
            var Player3 = new Player("Tomek");
            var Player4 = new Player("Janusz");
            var Players = new List<Player> { Player1, Player2, Player3, Player4 };

            Console.Clear();

            return Players;
        }

        public static List<Card> CreateCardDeck()
        {
            var CardSymbols = new List<string> { "A", "K", "Q", "J", "10", "9" };
            var CardSuits = new List<string> { GameView.Suit("spades"), GameView.Suit("hearts"), GameView.Suit("clubs"), GameView.Suit("diamonds") };
            var DrawPile = new List<Card>();
            foreach (var symbol in CardSymbols)
            {
                foreach (var suit in CardSuits)
                {
                    var card = new Card(symbol, suit);
                    DrawPile.Add(card);
                }
            }
            return DrawPile;
        }
    }
}
