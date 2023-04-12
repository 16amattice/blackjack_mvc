namespace drew_blackjack_mvc.Models
{
    public class Card
    {
        public string Value { get; set; }
        public string Suit { get; set; }
        public bool IsHidden { get; set; }

        public Card(string value, string suit)
        {
            Value = value;
            Suit = suit;
            IsHidden = false;
        }
    }
}
