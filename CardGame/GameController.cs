using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    class GameController
    {
        public Game game;

        public GameController()
        {
            game = Game.CreateInstance();
        }

        public List<Card> GetAllDiscardedCards()
        {
            var allDiscardedCArds = new List<Card>();
            foreach (var player in game.Players)
            {
                foreach (var card in player.Discarded)
                {
                    allDiscardedCArds.Add(card);
                }
            }
            Game.AllPlayedCards = allDiscardedCArds;
            return Game.AllPlayedCards;
        }

        public void Run()
        {
            
            DealOutCards();
            
            Player currentPlayer = DetermineStartingPlayer();

            while (FirstPlayerHasCard())
            {

                GetTable();
                var trick = new Trick();

                CurrentTrickPlay(currentPlayer, trick);
                GameView.TrickCards(trick);

                DetermineTrickWinning(trick, ref currentPlayer);

                Console.ReadLine();

                GetAllDiscardedCards();
                Console.Clear();


            }

            if (Is100PointsReached())
            {
                GetFinalTable();
                EvaluateWinner();
                BeginNewGame();
            }
            else
            {
                GetTable();
                BeginNewRound();
            }
            
        }


        private void BeginNewGame()
        {
            GameView.NewGameQuestion();
            var answer = Console.ReadLine();
            if (answer.ToUpper() == "Y")
            {
                Console.Clear();
                game = Game.CreateInstance();
                Run();
            }
        }

      

        private void BeginNewRound()
        {
            Console.WriteLine("new round? y/n");
            var answer = Console.ReadLine();
            if (answer.ToUpper() == "Y")
            {
                Console.Clear();
                game = Game.CreateInstanceOfNextRound(game);
                Run();
            }
        }

        private bool Is100PointsReached()
        {
            for (int i = 0; i < game.Players.Count(); i++)
            {
                UpdatePlayerTotalPoints(i);
                if (game.Players[i].TotalPoints >= 100)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdatePlayerTotalPoints(int i)
        {
            game.Players[i].TotalPoints += game.Players[i].EvaluatePoints();
        }

        private bool FirstPlayerHasCard()
        {
            return game.Players[0].Hand.Count > 0;
        }

        private void CurrentTrickPlay(Player currentPlayer, Trick trick)
        {
            var playerIndex = game.Players.IndexOf(currentPlayer);

            var playedCardCounter = 0;

            while (playedCardCounter < game.Players.Count)
            {

                if (isPlayerHuman(playerIndex))
                {
                    PlayingCard.ForHuman(trick, game.Players[playerIndex], currentPlayer);
                }
                else
                {
                    PlayingCard.ForComputer(trick, game.Players[playerIndex], currentPlayer);
                }
                if (playerIndex == game.Players.Count - 1)
                {
                    playerIndex = -1;
                }
                playerIndex++;
                playedCardCounter++;
            }
        }

        private bool isPlayerHuman(int playerIndex)
        {
            return !game.ComputerPlayers.Contains(game.Players[playerIndex]);
        }

        private Player DetermineStartingPlayer()
        {
            var currentPlayer = game.Players[0];
            foreach (var player in game.Players)
            {
                foreach (var card in player.Hand)
                {
                    if (card.Symbol == "9" && card.Suit == "♣")
                    {
                        currentPlayer = player;
                    }
                }
            }
            return currentPlayer;
        }

        private void DetermineTrickWinning(Trick trick, ref Player currentPlayer)
        {
            Card trickWinningCard = DetermineTrickWinningCard(trick);

            currentPlayer = DetermineTrickWinningPlayer(trick, currentPlayer, trickWinningCard);

            GameView.TrickWinner(currentPlayer, trickWinningCard);
        }

        

        public static Card DetermineTrickWinningCard(Trick trick)
        {
            var trickWinningCard = trick.FirstCard;
            foreach (var card in trick.Cards)
            {
                if (card.Suit == trickWinningCard.Suit && card.Rank > trickWinningCard.Rank)
                {
                    trickWinningCard = card;
                }
            }
            return trickWinningCard;
        }

        private Player DetermineTrickWinningPlayer(Trick trick, Player currentPlayer, Card trickWinningCard)
        {
            foreach (var player in game.Players)
            {
                if (player.Discarded.Contains(trickWinningCard))
                {
                    currentPlayer = player;
                    player.TakenTricks.Add(trick);
                    player.TakenCards();
                }
            }
            return currentPlayer;
        }


        private void GetTable()
        {
            for (int i = 0; i < game.Players.Count; i++)
            {
                GetPlayerName(i);
                GetPlayerHand(i);
                GetPlayerPoints(i);
                GetPlayerTotalPoints(i);
            }
        }

        private void GetFinalTable()
        {
            for (int i = 0; i < game.Players.Count; i++)
            {
                GetPlayerName(i);
                GetPlayerHand(i);
                GetPlayerTotalPoints(i);
            }
        }


        private void GetPlayerHand(int i)
        {
            var playerHand = game.Players[i].SortedHand();

            foreach (var card in playerHand)
            {
                GameView.PlayerHanIndexed(playerHand, card);
            }
        }

        

        private void GetPlayerTotalPoints(int i)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(String.Format("Total: {0}",game.Players[i].TotalPoints));
            Console.ResetColor();
            Console.WriteLine("\n");
        }

        private void GetPlayerPoints(int i)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(String.Join(" ", "Points:", game.Players[i].EvaluatePoints()));
            Console.ResetColor();
        }


        private void EvaluateWinner()
        {
            var roundWinner = game.Players.OrderBy(z => z.EvaluatePoints()).FirstOrDefault();

            var winner = game.Players.OrderBy(z => z.TotalPoints).FirstOrDefault();
            GameView.WinnerName(winner);
        }

        

        private void GetPlayerName(int i)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(game.Players[i].Name);
            Console.ResetColor();
        }

        private void DealOutCards()
        {
            var rand = new Random();
            while (game.DrawPile.Count > 0)
            {

                for (int i = 0; i < game.Players.Count; i++)
                {
                    var index = rand.Next(game.DrawPile.Count);
                    game.Players[i].Hand.Add(game.DrawPile[index]);
                    game.DrawPile.Remove(game.DrawPile[index]);
                }
            }
        }
    }
}
