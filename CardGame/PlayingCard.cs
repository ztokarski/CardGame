using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class PlayingCard
    {

        public static void ForComputer(Trick trick, Player player, Player currentPlayer)
        {
            Card chosenComputerCard;
            if (player == currentPlayer) //rozpoczyna lewę
            {
                if (AtLeastOneHeartWasPlayed()) //rozpoczyna lewę i kiery zostały już zagrane w tej rundzie
                {
                    chosenComputerCard = LowestCard(player);
                    trick.FirstCard = chosenComputerCard;
                }
                else
                {
                    if (AtLeastOneNotHeartInHand(player)) // rozpoczyna lewę, kiery nie zostały jeszcze zagrane i MA na ręce inne kolory niż kier
                    {
                        chosenComputerCard = LowestCardNotHeart(player);
                        trick.FirstCard = chosenComputerCard;
                    }
                    else // rozpoczyna lewę, kiery nie zostały jeszcze zagrane, ale ma tylko kiery
                    {
                        chosenComputerCard = LowestCard(player);
                        trick.FirstCard = chosenComputerCard;
                    }
                }
            }
            else // nie rozpoczyna lewy
            {
                trick.FirstCard = GameController.DetermineTrickWinningCard(trick);
                var currentSuit = trick.FirstCard.Suit;

                if (HaveHigherCardInMatchingSuitNotHeartsOrSpades(trick, player, currentSuit)) // nie rozpoczyna lewy i ma wyższą kartę w obowiązującym kolorze, (nie pik i nie kier!)
                {
                    chosenComputerCard = HighestCardInMatchingSuit(player, currentSuit);
                }
                else if (HaveMatchingSuit(player, currentSuit)) //nie rozpoczyna lewy, ma tylko niższe karty w obowiązującym kolorze
                {
                    chosenComputerCard = LowestCardInMatchingSuit(player, currentSuit);
                }
                else // nie rozpoczyna lewy, nie ma kart w obowiązującym kolorze
                {
                    if (HighestValueCardSpade(player) != null) //ma Asa, Króla lub Damę pik
                    {
                        chosenComputerCard = HighestValueCardSpade(player); //zagrywa w kolejności Damę, Króla lub Asa pik
                    }
                    else if (HighestCardHeart(player) != null) // Ma kiery
                    {
                        chosenComputerCard = HighestCardHeart(player); // zagrywa najwyższego kiera
                    }
                    else
                    {
                        chosenComputerCard = HighestCard(player); // zagrywa najwyższą kartę
                    }
                }
            }
            trick.Cards.Add(chosenComputerCard);
            player.Discard(chosenComputerCard);

            Game.AllPlayedCards.Add(chosenComputerCard);

            GameView.ChosenComputerCard(player, chosenComputerCard);

        }

        

        private static bool HaveHigherCardInMatchingSuitNotHeartsOrSpades(Trick trick, Player player, string currentSuit)
        {
            return HaveMatchingSuit(player, currentSuit) && HighestCardInMatchingSuit(player, currentSuit).Rank > trick.FirstCard.Rank
                                && currentSuit != GameView.Suit("spades") && currentSuit != GameView.Suit("hearts") && (trick.Cards.Select(z => z.ValueInTrick).Sum() < 1);
        }

        private static bool AtLeastOneHeartWasPlayed()
        {
            return Game.AllPlayedCards.Select(z => z).Where(z => z.Suit == GameView.Suit("hearts")).FirstOrDefault() != null;
        }

        private static bool AtLeastOneNotHeartInHand(Player player)
        {
            return player.Hand.Where(z => z.Suit != GameView.Suit("hearts")).FirstOrDefault() != null;
        }

        private static Card LowestCard(Player player)
        {
            return player.Hand.OrderBy(z => z.Rank).FirstOrDefault();
        }

        private static Card LowestCardNotHeart(Player player)
        {
            return player.Hand.OrderBy(z => z.Rank).Where(z => z.Suit != GameView.Suit("hearts")).FirstOrDefault();
        }

        private static Card LowestCardSpade(Player player)
        {
            return player.Hand.OrderBy(z => z.Rank).Where(z => z.Suit == GameView.Suit("spades")).FirstOrDefault();
        }

        private static Card HighestCard(Player player)
        {
            return player.Hand.OrderByDescending(z => z.Rank).FirstOrDefault();
        }

        private static Card HighestCardHeart(Player player)
        {
            return player.Hand.OrderByDescending(z => z.Rank).Where(z => z.Suit == GameView.Suit("hearts")).FirstOrDefault();
        }

        private static Card HighestValueCardSpade(Player player)
        {
            return player.Hand.OrderByDescending(z => z.ValueInTrick).Where(z => z.Suit == GameView.Suit("spades") && z.ValueInTrick>5).FirstOrDefault();
        }

        private static Card LowestCardInMatchingSuit(Player player, string currentSuit)
        {
            return player.Hand.Where(z => z.Suit == currentSuit).OrderBy(z => z.Rank).ThenBy(z => z.ValueInTrick).FirstOrDefault();
        }

        private static Card HighestCardInMatchingSuit(Player player, string currentSuit)
        {
            return player.Hand.Where(z => z.Suit == currentSuit).OrderByDescending(z => z.Rank).FirstOrDefault();
        }

        private static bool HaveMatchingSuit(Player player, string currentSuit)
        {
            return player.Hand.Where(z => z.Suit == currentSuit).ToList().Count > 0;
        }


        public static void ForHuman(Trick trick, Player player, Player currentPlayer)
        {

            var askHumanForChoice = true;
            while (askHumanForChoice)
            {
                Console.WriteLine("\nCHOOSE ONE CARD FROM YOUR HAND, PLEASE\n");
                var chosenNumber = Console.ReadLine();

                if (YouAreFirstPlayer(player, currentPlayer))
                {
                    if (IsInputCorrect(player, chosenNumber, out int number))
                    {
                        askHumanForChoice = UpdateChosenCard(trick, player, currentPlayer, number);
                    }
                    else
                    {
                        Console.WriteLine("Type correct number");
                    }
                }
                else // you are not the first player
                {
                    if (IsCardInMatchingSuitInPlayerHand(trick, player)) // you have card in matching suit
                    {
                        if (IsInputAndSuitCorrect(trick, player, chosenNumber, out int number)) // check input type and card suit
                        {
                            askHumanForChoice = UpdateChosenCard(trick, player, currentPlayer, number);
                        }
                        else
                        {
                            GameView.WrongSuitStatement();
                        }
                    }
                    else // you don't have card in matching suit
                    {
                        if (IsInputCorrect(player, chosenNumber, out int number))
                        {
                            askHumanForChoice = UpdateChosenCard(trick, player, currentPlayer, number);
                        }
                        else
                        {
                            Console.WriteLine("Type correct number");
                        }
                    }
                }

            }

        }

        

        private static bool IsCardInMatchingSuitInPlayerHand(Trick trick, Player player)
        {
            return player.Hand.Where(z => z.Suit == trick.FirstCard.Suit).FirstOrDefault() != null;
        }

        private static bool YouAreFirstPlayer(Player player, Player currentPlayer)
        {
            return player == currentPlayer;
        }

        private static bool UpdateChosenCard(Trick trick, Player player, Player currentPlayer, int number)
        {
            bool askHumanForChoice;
            var chosenCard = player.SortedHand()[number - 1];
            trick.Cards.Add(chosenCard);
            player.Discard(chosenCard);
            askHumanForChoice = false;
            Console.WriteLine(String.Format("{0} plays {1}", player.Name, chosenCard));
            if (player == currentPlayer)
            {
                trick.FirstCard = chosenCard;
            }

            return askHumanForChoice;
        }

        private static bool IsInputAndSuitCorrect(Trick trick, Player player, string chosenNumber, out int number)
        {
            return Int32.TryParse(chosenNumber, out number) && (number <= player.Hand.Count()) && (player.SortedHand()[number - 1].Suit == trick.FirstCard.Suit);
        }

        private static bool IsInputCorrect(Player player, string chosenNumber, out int number)
        {
            return Int32.TryParse(chosenNumber, out number) && (number <= player.Hand.Count());
        }

       
    }
}
