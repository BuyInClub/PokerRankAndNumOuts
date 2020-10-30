using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CompareHands
{
    // values will be used in the bitmask for ranking, they cannot collide with the
    // card ranking which will be the bits 0-12 (13 card values)
    // they need to be in order of hand ranking.
    public enum HandRanking
    {
        HighCard = 0,
        Pair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        Straight = 4,
        Flush = 5,
        FullHouse = 6,
        FourOfAKind = 7,
        StraightFlush = 8,
        RoyalStraightFlush = 9
    }
    public enum Suit
    {
        Heart,
        Spade,
        Club,
        Diamond
    }

    public class CardValues
    {
        public static readonly Dictionary<string, int> cardValues = new Dictionary<string, int>()
        {
            {"2", 2},
            {"3", 3},
            {"4", 4},
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8},
            {"9", 9},
            {"10", 10},
            {"J", 11},
            {"Q", 12},
            {"K", 13},
            {"A", 14}
          };

        public static String GetName(int rank)
        {
            String name = String.Empty;
            foreach(KeyValuePair<string, int> nameValue in cardValues)
            {
                if (nameValue.Value == rank)
                {
                    name = nameValue.Key;
                    break;
                }
            }

            return name;
        }
    }

    public class Deck : IEnumerable<CardInDeck>
    {
        Dictionary<String, CardInDeck> deck = new Dictionary<string, CardInDeck>();
        public IEnumerator<CardInDeck> GetEnumerator()
        {
            return deck.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //forces use of the non-generic implementation on the Values collection
            return ((IEnumerable)deck.Values).GetEnumerator();
        }

        // create the 52 card deck
        public Deck()
        {
            foreach (String card in CardValues.cardValues.Keys)
            {
                foreach (String suit in Enum.GetNames(typeof(Suit)))
                {
                    deck.Add(card + suit, new CardInDeck(card, (Suit)Enum.Parse(typeof(Suit), suit)));
                }
            }
        }

        // copy constructor
        public Deck(Deck otherDeck)
        {
            this.deck = new Dictionary<string, CardInDeck>(otherDeck.deck);
        }

        public void Reset()
        {
            foreach(CardInDeck card in deck.Values)
            {
                card.cardInUse = false;
            }
        }

        public bool ValidCard(Card card)
        {
            return deck.ContainsKey(GetCardKey(card));
        }

        public bool IsCardInUse(Card card)
        {
            return deck[GetCardKey(card)].cardInUse;
        }
        public void SetCardInUse(Card card)
        {
            deck[GetCardKey(card)].cardInUse = true;
        }
        private String GetCardKey(Card card)
        {
            return card.value + card.suit.ToString();
        }
    }

    public class CardInDeck : Card
    {
        public bool cardInUse;
        public CardInDeck(String value, Suit suit, bool cardInUse = false)
            : base(value, suit)
        {
            this.cardInUse = cardInUse;
        }

        // copy constructor
        public CardInDeck(CardInDeck otherCardInDeck)
            : base(otherCardInDeck)
        {
            this.cardInUse = otherCardInDeck.cardInUse;
        }

    }
    public class Card
    {
        public String value;
        public Suit suit;
        public Card(String value, Suit suit)
        {
            this.value = value;
            this.suit = suit;
        }

        // copy constructor
        public Card(Card otherCard)
        {
            this.value = otherCard.value;
            this.suit = otherCard.suit;
        }
    }

    public class HandComparisonInfo
    {
        public HandRanking handName;
        public List<int> sigCardRanks; // significant cards, part of the ranked hand
        public List<int> minorCardRanks; // single cards that make out the rest ofthe hand
    }

    public class HandInfo
    {
        public HandComparisonInfo handComparisonInfo;
        public long handRank; // sort by this get hands ordered
        public String name; // would normally be player hand, but can be anything

        public static long CalculateRank(HandRanking handName, List<int> sigCardRanks, List<int> minorCardRanks)
        {
            long finalRank = 0;
            BitMask handBitMask = new BitMask();
            handBitMask.Set((int)handName, true);

            BitMask cardBitMask = new BitMask();
            foreach (int rank in minorCardRanks)
            {
                cardBitMask.Set(rank - 2, true);
            }
            int numCardValues = CardValues.cardValues.Count;
            foreach (int rank in sigCardRanks)
            {
                cardBitMask.Set(numCardValues + rank - 2, true);
            }
            finalRank = (long)handBitMask.Data << 32 | (uint)cardBitMask.Data;
            // for debug purposes
            var binary = Convert.ToString((long)finalRank, 2);

            return finalRank;
        }

        public HandInfo(HandRanking handName, List<int> sigCardRanks, List<int> minorCardRanks, String name = "")
        {
            handComparisonInfo = new HandComparisonInfo();
            handComparisonInfo.handName = handName;
            handComparisonInfo.sigCardRanks = sigCardRanks;
            handComparisonInfo.minorCardRanks = minorCardRanks;
            this.name = name;
            handRank = HandInfo.CalculateRank(handName, sigCardRanks, minorCardRanks);
        }
    }
    public class AnalyzeHands
    {
        private Deck deck;
        public Deck DeckInUse => deck;
        public AnalyzeHands()
        {
            deck = new Deck();
        }

        public List<HandInfo> OrderHands(Dictionary<String, List<Card>> hands)
        {
            // reset deck to no cards in use
            deck.Reset();

            List<HandInfo> handInfoList = new List<HandInfo>();
            // validate input
            if (ValidateHands(hands))
            {
                foreach(KeyValuePair<String, List<Card>> hand in hands)
                {
                    handInfoList.Add(DetermineHandInfo(hand.Key, hand.Value));
                }
            }

            return handInfoList.OrderByDescending(x => x.handRank).ToList();
        }
        
        private HandInfo DetermineHandInfo(String name, List<Card> hand)
        {
            HandInfo handInfo;
            IsHand isHand = new IsHand();
            handInfo = isHand.IsStraightFlush(hand);
            if (handInfo == null) { handInfo = isHand.IsFourOfAKind(hand); }
            if (handInfo == null) { handInfo = isHand.IsFullHouse(hand); }
            if (handInfo == null) { handInfo = isHand.IsFlush(hand); }
            if (handInfo == null) { handInfo = isHand.IsStraight(hand); }
            if (handInfo == null) { handInfo = isHand.IsThreeOfAKind(hand); }
            if (handInfo == null) { handInfo = isHand.IsTwoPair(hand); }
            if (handInfo == null) { handInfo = isHand.IsPair(hand); }
            if (handInfo == null) { handInfo = isHand.IsHighCard(hand); }

            if (handInfo != null)
            {
                handInfo.name = name; 
            }

            return handInfo;
        }

        private bool ValidateHands(Dictionary<String, List<Card>> hands)
        {
            bool validHands = true;
            foreach(List<Card> hand in hands.Values)
            {
                // number of cards in a hand must be between 5 and 7
                if (hand.Count > 7 || hand.Count < 5) { return false; }
                foreach(Card card in hand)
                {
                    if (!CardValues.cardValues.ContainsKey(card.value)) { return false; }
                    deck.SetCardInUse(card);
                }
            }

            return validHands;
        }
    }
}
