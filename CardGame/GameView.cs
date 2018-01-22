using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class GameView
    {
        public static void TrickCards(Trick trick)
        {
           Console.WriteLine(String.Format("Current trick: {0}", String.Join(" ", trick.Cards)));
        }

        public static void NewGameQuestion()
        {
           Console.WriteLine("new Game? y/n");
        }

        public static void TrickWinner(Player currentPlayer, Card trickWinningCard)
        {
            Console.WriteLine(String.Format("{0} won the trick ({1})", currentPlayer, trickWinningCard));
        }

        public static void PlayerHanIndexed(List<Card> playerHand, Card card)
        {
            Console.WriteLine(String.Join(") ", playerHand.IndexOf(card) + 1, card));
        }

        public static void WinnerName(Player winner)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(String.Format("{0} won the game with {1} points. Congratulations!\n", winner.Name.ToUpper(), winner.TotalPoints));
            Console.ResetColor();
        }

        public static void PlayerNameQuestion()
        {
            Console.WriteLine("What's your name?");
        }

        public static void WrongSuitStatement()
        {
            Console.WriteLine("Wrong Suit!");
        }

        public static void ChosenComputerCard(Player player, Card chosenComputerCard)
        {
            Console.ResetColor();
            Console.WriteLine(String.Format("{0} plays {1}", player, chosenComputerCard));
        }

        public static string Suit(string type)
        {
            switch (type)
            {
                case "spades":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    return "♠";
                case "hearts":
                    return "♥";
                case "clubs":
                    return "♣";
                case "diamonds":
                    return "♦";
                default:
                    return "x";
            }
        }

       


        public static void PlayerPoints(Game game, int i)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(String.Join(" ", "Points:", game.Players[i].EvaluatePoints()));
            Console.ResetColor();
            Console.WriteLine("\n");
        }

        public static void PlayerName(Game game, int i)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(game.Players[i].Name);
            Console.ResetColor();
        }

    }
}