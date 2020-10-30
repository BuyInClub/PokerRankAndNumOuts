using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompareHands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareHands.Tests
{
    [TestClass()]
    public class AnalyzeHandsTests
    {
        [TestMethod()]
        public void IsStraightAceHigh()
        {
            AnalyzeHands analyzeHands = new AnalyzeHands();
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    new Card("10", Suit.Club),
                    new Card("J", Suit.Club),
                    new Card("5", Suit.Club),
                    new Card("6", Suit.Spade),
                    new Card("Q", Suit.Diamond), 
                    new Card("K", Suit.Club),
                    new Card("A", Suit.Heart)
                } },
            };

            List<HandInfo> results = analyzeHands.OrderHands(hands);
            // Ace high straight
            long finalRank = HandInfo.CalculateRank(HandRanking.Straight, new List<int> { 14 }, new List<int> { });
            Assert.AreEqual(results[0].handRank, finalRank);
        }

        [TestMethod()]
        public void IsStraightFiveHigh()
        {
            AnalyzeHands analyzeHands = new AnalyzeHands();
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    new Card("10", Suit.Club),
                    new Card("J", Suit.Club),
                    new Card("2", Suit.Club),
                    new Card("5", Suit.Spade),
                    new Card("3", Suit.Diamond),
                    new Card("4", Suit.Club),
                    new Card("A", Suit.Heart)
                } },
            };

            List<HandInfo> results = analyzeHands.OrderHands(hands);

            // Ace high straight
            long finalRank = HandInfo.CalculateRank(HandRanking.Straight, new List<int> { 5 }, new List<int> { });
            Assert.AreEqual(results[0].handRank, finalRank);
        }
        [TestMethod()]
        public void IsStraightFlush()
        {
            AnalyzeHands analyzeHands = new AnalyzeHands();
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    new Card("2", Suit.Heart),
                    new Card("J", Suit.Diamond),
                    new Card("2", Suit.Club),
                    new Card("5", Suit.Club),
                    new Card("3", Suit.Club),
                    new Card("4", Suit.Club),
                    new Card("A", Suit.Club)
                } },
            };

            List<HandInfo> results = analyzeHands.OrderHands(hands);

            // Ace high straight flush
            long finalRank = HandInfo.CalculateRank(HandRanking.StraightFlush, new List<int> { 5 }, new List<int> { });
            Assert.AreEqual(results[0].handRank, finalRank);
        }
        [TestMethod()]
        public void IsRoyalStraightFlush()
        {
            AnalyzeHands analyzeHands = new AnalyzeHands();
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    new Card("K", Suit.Diamond),
                    new Card("J", Suit.Diamond),
                    new Card("2", Suit.Club),
                    new Card("10", Suit.Diamond),
                    new Card("Q", Suit.Diamond),
                    new Card("4", Suit.Club),
                    new Card("A", Suit.Diamond)
                } },
            };

            List<HandInfo> results = analyzeHands.OrderHands(hands);

            // royal straight flush
            long finalRank = HandInfo.CalculateRank(HandRanking.RoyalStraightFlush, new List<int> { 14 }, new List<int> { });
            Assert.AreEqual(results[0].handRank, finalRank);
        }

        [TestMethod()]
        public void IsFourOfAKind()
        {
            AnalyzeHands analyzeHands = new AnalyzeHands();
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    new Card("K", Suit.Diamond),
                    new Card("J", Suit.Diamond),
                    new Card("K", Suit.Club),
                    new Card("K", Suit.Spade),
                    new Card("Q", Suit.Diamond),
                    new Card("4", Suit.Club),
                    new Card("K", Suit.Heart)
                } },
            };

            List<HandInfo> results = analyzeHands.OrderHands(hands);

            // four of a kind
            long finalRank = HandInfo.CalculateRank(HandRanking.FourOfAKind, new List<int> { 13 }, new List<int> { 12 });
            Assert.AreEqual(results[0].handRank, finalRank);
        }

        [TestMethod()]
        public void IsFullHouse()
        {
            AnalyzeHands analyzeHands = new AnalyzeHands();
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    new Card("K", Suit.Diamond),
                    new Card("J", Suit.Diamond),
                    new Card("J", Suit.Club),
                    new Card("K", Suit.Spade),
                    new Card("J", Suit.Heart),
                    new Card("4", Suit.Club),
                    new Card("K", Suit.Heart)
                } },
            };

            List<HandInfo> results = analyzeHands.OrderHands(hands);

            // full house
            long finalRank = HandInfo.CalculateRank(HandRanking.FullHouse, new List<int> { 13 }, new List<int> { 11 });
            Assert.AreEqual(results[0].handRank, finalRank);
        }

        [TestMethod()]
        public void IsFlush()
        {
            AnalyzeHands analyzeHands = new AnalyzeHands();
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    new Card("K", Suit.Diamond),
                    new Card("J", Suit.Diamond),
                    new Card("J", Suit.Club),
                    new Card("9", Suit.Diamond),
                    new Card("8", Suit.Diamond),
                    new Card("4", Suit.Diamond),
                    new Card("K", Suit.Heart)
                } },
            };

            List<HandInfo> results = analyzeHands.OrderHands(hands);

            // flush
            long finalRank = HandInfo.CalculateRank(HandRanking.Flush, new List<int> { 13, 11, 9, 8, 4 }, new List<int> { });
            Assert.AreEqual(results[0].handRank, finalRank);
        }

        [TestMethod()]
        public void IsThreeOfAKind()
        {
            AnalyzeHands analyzeHands = new AnalyzeHands();
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    new Card("K", Suit.Diamond),
                    new Card("J", Suit.Diamond),
                    new Card("J", Suit.Club),
                    new Card("J", Suit.Spade),
                    new Card("8", Suit.Diamond),
                    new Card("4", Suit.Diamond),
                    new Card("10", Suit.Heart)
                } },
            };

            List<HandInfo> results = analyzeHands.OrderHands(hands);

            // three of a kind
            long finalRank = HandInfo.CalculateRank(HandRanking.ThreeOfAKind, new List<int> { 11 }, new List<int> { 13, 10 });
            Assert.AreEqual(results[0].handRank, finalRank);
        }
        [TestMethod()]
        public void IsTwoPair()
        {
            AnalyzeHands analyzeHands = new AnalyzeHands();
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    new Card("K", Suit.Diamond),
                    new Card("2", Suit.Diamond),
                    new Card("J", Suit.Club),
                    new Card("K", Suit.Spade),
                    new Card("J", Suit.Diamond),
                    new Card("4", Suit.Diamond),
                    new Card("10", Suit.Heart)
                } },
            };

            List<HandInfo> results = analyzeHands.OrderHands(hands);

            // Two of a kind
            long finalRank = HandInfo.CalculateRank(HandRanking.TwoPair, new List<int> { 13, 11 }, new List<int> { 10 });
            Assert.AreEqual(results[0].handRank, finalRank);
        }

        [TestMethod()]
        public void IsPair()
        {
            AnalyzeHands analyzeHands = new AnalyzeHands();
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    new Card("K", Suit.Diamond),
                    new Card("2", Suit.Diamond),
                    new Card("J", Suit.Club),
                    new Card("K", Suit.Spade),
                    new Card("8", Suit.Diamond),
                    new Card("4", Suit.Diamond),
                    new Card("10", Suit.Heart)
                } },
            };

            List<HandInfo> results = analyzeHands.OrderHands(hands);

            // Pair
            long finalRank = HandInfo.CalculateRank(HandRanking.Pair, new List<int> { 13 }, new List<int> { 11, 10, 8 });
            Assert.AreEqual(results[0].handRank, finalRank);
        }

        [TestMethod()]
        public void IsHighCard()
        {
            AnalyzeHands analyzeHands = new AnalyzeHands();
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    new Card("K", Suit.Diamond),
                    new Card("2", Suit.Diamond),
                    new Card("J", Suit.Club),
                    new Card("3", Suit.Spade),
                    new Card("8", Suit.Diamond),
                    new Card("4", Suit.Diamond),
                    new Card("10", Suit.Heart)
                } },
            };

            List<HandInfo> results = analyzeHands.OrderHands(hands);

            // High card
            long finalRank = HandInfo.CalculateRank(HandRanking.HighCard, new List<int> { }, new List<int> { 13, 11, 10, 8, 4 });
            Assert.AreEqual(results[0].handRank, finalRank);
        }

    }
}