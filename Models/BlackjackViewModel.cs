using System;
using System.Collections.Generic;
using System.Linq;

namespace drew_blackjack_mvc.Models
{
    public class BlackjackViewModel
    {
        public List<Card> PlayerHand { get; set; }
        public List<Card> DealerHand { get; set; }
        public int PlayerSum { get; set; }
        public int DealerSum { get; set; }
        public string GameStatus { get; set; }
        public List<Card> Deck { get; set; }

        public BlackjackViewModel()
        {
            PlayerHand = new List<Card>();
            DealerHand = new List<Card>();
            Deck = new List<Card>();
            GameStatus = "";
        }

        public void BuildDeck()
        {
            string[] values = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            string[] suits = { "C", "D", "H", "S" };

            foreach (var suit in suits)
            {
                foreach (var value in values)
                {
                    Deck.Add(new Card(value, suit));
                }
            }
        }

        public void ShuffleDeck()
        {
            var random = new Random();
            Deck = Deck.OrderBy(card => random.Next()).ToList();
        }

        public void DealInitialCards()
        {
            GiveDealerCard(true);
            GivePlayerCard();
            GiveDealerCard();
            GivePlayerCard();

            PlayerSum = CalculateHandValue(PlayerHand);
            DealerSum = CalculateHandValue(DealerHand);
        }


        public void GivePlayerCard()
        {
            if (Deck.Count == 0) return;
            Card card = Deck.First();
            Deck.RemoveAt(0);
            PlayerHand.Add(card);
            Console.WriteLine($"Player received card: {card.Value}{card.Suit}");
            PlayerSum = CalculateHandValue(PlayerHand);
            CheckPlayerBust();
        }

        public void GiveDealerCard(bool isHidden = false)
        {
            if (Deck.Count == 0) return;
            Card card = Deck.First();
            Deck.RemoveAt(0);

            if (isHidden)
            {
                card.IsHidden = true;
            }

            DealerHand.Add(card);
            Console.WriteLine($"Dealer received card: {card.Value}{card.Suit}");
            DealerSum = CalculateHandValue(DealerHand);
        }

        public void Stay()
        {
            RevealDealerHiddenCard();
            while (DealerSum < 17)
            {
                GiveDealerCard();
            }

            if (PlayerSum > 21)
            {
                GameStatus = "Bust!";
            }
            else if (DealerSum > 21)
            {
                GameStatus = "You win!";
            }
            else if (PlayerSum == DealerSum)
            {
                GameStatus = "Push.";
            }
            else if (PlayerSum > DealerSum)
            {
                GameStatus = "You win!";
            }
            else
            {
                GameStatus = "You lose!";
            }
        }

        private int CalculateHandValue(List<Card> hand)
        {
            int sum = 0;
            int aceCount = 0;

            foreach (var card in hand)
            {
                if (card.Value == "A")
                {
                    sum += 11;
                    aceCount++;
                }
                else if (card.Value == "K" || card.Value == "Q" || card.Value == "J")
                {
                    sum += 10;
                }
                else
                {
                    sum += int.Parse(card.Value);
                }
            }

            while (sum > 21 && aceCount > 0)
            {
                sum -= 10;
                aceCount--;
            }

            return sum;
        }

        private void CheckPlayerBust()
        {
            if (PlayerSum > 21)
            {
                GameStatus = "Bust!";
            }
        }

        private void RevealDealerHiddenCard()
        {
            foreach (var card in DealerHand)
            {
                card.IsHidden = false;
            }
        }
    }
}