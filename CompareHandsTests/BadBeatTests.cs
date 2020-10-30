using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CompareHands;

namespace CompareHandsTests
{
    [TestClass]
    public class BadBeatTests
    {
        [TestMethod]
        public void BadBeatOtherPlayerHasOut()
        {
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    // board
                    new Card("8", Suit.Spade),
                    new Card("5", Suit.Spade),
                    new Card("8", Suit.Diamond),
                    new Card("3", Suit.Club),
                    // in hand
                    new Card("10", Suit.Spade),
                    new Card("6", Suit.Spade)
                } },
                { "Chris", new List<Card> {
                    // board
                    new Card("8", Suit.Spade),
                    new Card("5", Suit.Spade),
                    new Card("8", Suit.Diamond),
                    new Card("3", Suit.Club),
                    // in hand
                    new Card("8", Suit.Club),
                    new Card("6", Suit.Diamond)
                } },
            };

            BadBeatCalc badBeatCalc = new BadBeatCalc();
            String sixCardWinner;
            List<Card> numOuts = badBeatCalc.DetermineBadBeat("Dave", hands, out sixCardWinner);

            Assert.AreEqual(numOuts.Count, 8);
            Assert.AreEqual(sixCardWinner, "Chris");
        }


        [TestMethod]
        public void BadBeatTiedOnTurn()
        {
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                { "Dave", new List<Card> {
                    // board
                    new Card("2", Suit.Spade),
                    new Card("2", Suit.Club),
                    new Card("K", Suit.Heart),
                    new Card("K", Suit.Diamond),
                    // in hand
                    new Card("6", Suit.Club),
                    new Card("K", Suit.Spade)
                } },
                { "Chris", new List<Card> {
                    // board
                    new Card("2", Suit.Spade),
                    new Card("2", Suit.Club),
                    new Card("K", Suit.Heart),
                    new Card("K", Suit.Diamond),
                    // in hand
                    new Card("K", Suit.Club),
                    new Card("J", Suit.Diamond)
                } },
            };

            BadBeatCalc badBeatCalc = new BadBeatCalc();
            String sixCardWinner;
            List<Card> numOuts = badBeatCalc.DetermineBadBeat("Chris", hands, out sixCardWinner);

            Assert.AreEqual(numOuts.Count, 3);
            Assert.AreEqual(sixCardWinner, "Dave");
        }


        [TestMethod]
        public void BadBeatTwoPlayers()
        {
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                // Dave has a set of 6's, Chris an opened end straight
                // Chris had 4 outs for a 10 to hit the straight
                { "Dave", new List<Card> {
                    // board
                    new Card("5", Suit.Club),
                    new Card("6", Suit.Club),
                    new Card("J", Suit.Spade),
                    new Card("Q", Suit.Diamond),
                    // in hand
                    new Card("6", Suit.Diamond),
                    new Card("6", Suit.Heart)
                } },
                { "Chris", new List<Card> {
                    // board
                    new Card("5", Suit.Club),
                    new Card("6", Suit.Club),
                    new Card("J", Suit.Spade),
                    new Card("Q", Suit.Diamond),
                    // in hand
                    new Card("K", Suit.Club),
                    new Card("A", Suit.Heart)
                } },
            };

            BadBeatCalc badBeatCalc = new BadBeatCalc();
            String sixCardWinner;
            List<Card> numOuts = badBeatCalc.DetermineBadBeat("Chris", hands, out sixCardWinner);

            Assert.AreEqual(numOuts.Count, 4);
        }

        [TestMethod]
        public void BadBeatThreePlayers()
        {
            Dictionary<String, List<Card>> hands = new Dictionary<String, List<Card>>
            {
                // Dave has a set of 6's, Chris an opened end straight
                // Chris had 4 outs for a 10 to hit the straight
                { "Dave", new List<Card> {
                    // board
                    new Card("5", Suit.Club),
                    new Card("6", Suit.Club),
                    new Card("J", Suit.Spade),
                    new Card("Q", Suit.Diamond),
                    // in hand
                    new Card("6", Suit.Diamond),
                    new Card("6", Suit.Heart)
                } },
                { "Chris", new List<Card> {
                    // board
                    new Card("5", Suit.Club),
                    new Card("6", Suit.Club),
                    new Card("J", Suit.Spade),
                    new Card("Q", Suit.Diamond),
                    // in hand
                    new Card("K", Suit.Club),
                    new Card("A", Suit.Heart)
                } },
                { "Jim", new List<Card> {
                    // board
                    new Card("5", Suit.Club),
                    new Card("6", Suit.Club),
                    new Card("J", Suit.Spade),
                    new Card("Q", Suit.Diamond),
                    // in hand
                    new Card("Q", Suit.Club),
                    new Card("Q", Suit.Heart)
                } },
            };

            BadBeatCalc badBeatCalc = new BadBeatCalc();
            String sixCardWinner;
            List<Card> numOuts = badBeatCalc.DetermineBadBeat("Chris", hands, out sixCardWinner);

            Assert.AreEqual(numOuts.Count, 4);
            Assert.AreEqual(sixCardWinner, "Jim");
        }

    }
}
