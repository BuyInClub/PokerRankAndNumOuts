using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareHands
{
    class IsHand
    {
        public HandInfo IsHighCard(List<Card> hand)
        {
            HandInfo handInfo = null;
            List<int> cardRanks = new List<int>();
            foreach (Card card in hand)
            {
                cardRanks.Add(CardValues.cardValues[card.value]);
            }

            // order by count of rank and then by rank
            var cardRankGrouping = cardRanks.GroupBy(i => i).OrderByDescending(x => x.Count()).ThenByDescending(y => y.Key);
            if (cardRankGrouping.First().Count() == 1)
            {
                // add top 5 cards
                List<int> sigCardRanks = new List<int>();
                List<int> minorCardRanks = new List<int>();
                minorCardRanks.Add(cardRankGrouping.First().Key);
                minorCardRanks.Add(cardRankGrouping.ElementAt(1).Key);
                minorCardRanks.Add(cardRankGrouping.ElementAt(2).Key);
                minorCardRanks.Add(cardRankGrouping.ElementAt(3).Key);
                minorCardRanks.Add(cardRankGrouping.ElementAt(4).Key);
                handInfo = new HandInfo(HandRanking.HighCard, sigCardRanks, minorCardRanks);
            }

            return handInfo;
        }


        public HandInfo IsPair(List<Card> hand)
        {
            HandInfo handInfo = null;
            List<int> cardRanks = new List<int>();
            foreach (Card card in hand)
            {
                cardRanks.Add(CardValues.cardValues[card.value]);
            }

            // order by count of rank and then by rank
            var cardRankGrouping = cardRanks.GroupBy(i => i).OrderByDescending(x => x.Count()).ThenByDescending(y => y.Key);
            if (cardRankGrouping.First().Count() == 2 && cardRankGrouping.ElementAt(1).Count() == 1)
            {
                List<int> sigCardRanks = new List<int>();
                List<int> minorCardRanks = new List<int>();
                sigCardRanks.Add(cardRankGrouping.First().Key);
                List<int> uniqueCardRanks = cardRanks.OrderByDescending(x => x).Distinct().ToList();
                // add relevant card not in each pair
                int addThreeRelevantCards = 0;
                int curUniqueCardRank = 0;
                do
                {
                    if (uniqueCardRanks[curUniqueCardRank] != sigCardRanks[0])
                    {
                        minorCardRanks.Add(uniqueCardRanks[curUniqueCardRank]);
                        addThreeRelevantCards++;
                    }
                    curUniqueCardRank++;
                } while (addThreeRelevantCards < 3);


                handInfo = new HandInfo(HandRanking.Pair, sigCardRanks, minorCardRanks);
            }

            return handInfo;
        }

        public HandInfo IsTwoPair(List<Card> hand)
        {
            HandInfo handInfo = null;
            List<int> cardRanks = new List<int>();
            foreach (Card card in hand)
            {
                cardRanks.Add(CardValues.cardValues[card.value]);
            }

            // order by count of rank and then by rank
            var cardRankGrouping = cardRanks.GroupBy(i => i).OrderByDescending(x => x.Count()).ThenByDescending(y => y.Key);
            if (cardRankGrouping.First().Count() == 2 && cardRankGrouping.ElementAt(1).Count() == 2)
            {
                List<int> sigCardRanks = new List<int>();
                List<int> minorCardRanks = new List<int>();
                sigCardRanks.Add(cardRankGrouping.First().Key);
                sigCardRanks.Add(cardRankGrouping.ElementAt(1).Key);
                List<int> uniqueCardRanks = cardRanks.OrderByDescending(x => x).Distinct().ToList();
                // add relevant card not in each pair
                int addOneRelevantCard = 0;
                int curUniqueCardRank = 0;
                do
                {
                    if (uniqueCardRanks[curUniqueCardRank] != sigCardRanks[0] && uniqueCardRanks[curUniqueCardRank] != sigCardRanks[1])
                    {
                        minorCardRanks.Add(uniqueCardRanks[curUniqueCardRank]);
                        addOneRelevantCard++;
                    }
                    curUniqueCardRank++;
                } while (addOneRelevantCard < 1);


                handInfo = new HandInfo(HandRanking.TwoPair, sigCardRanks, minorCardRanks);
            }

            return handInfo;
        }

        public HandInfo IsThreeOfAKind(List<Card> hand)
        {
            HandInfo handInfo = null;
            List<int> sigCardRanks = new List<int>();
            List<int> minorCardRanks = new List<int>();
            List<int> cardRanks = new List<int>();
            foreach (Card card in hand)
            {
                cardRanks.Add(CardValues.cardValues[card.value]);
            }

            // order by count of rank and then by rank
            var cardRankGrouping = cardRanks.GroupBy(i => i).OrderByDescending(x => x.Count()).ThenByDescending(y => y.Key);
            if (cardRankGrouping.First().Count() == 3)
            {
                sigCardRanks.Add(cardRankGrouping.First().Key);
                List<int> uniqueCardRanks = cardRanks.OrderByDescending(x => x).Distinct().ToList();
                // add relevant cards not part of 3 of a kind to relevantCards
                int addTwoRelevantCards = 0;
                int curUniqueCardRank = 0;
                do
                {
                    if (uniqueCardRanks[curUniqueCardRank] != sigCardRanks[0])
                    {
                        minorCardRanks.Add(uniqueCardRanks[curUniqueCardRank]);
                        addTwoRelevantCards++;
                    }
                    curUniqueCardRank++;
                } while (addTwoRelevantCards < 2);
                handInfo = new HandInfo(HandRanking.ThreeOfAKind, sigCardRanks, minorCardRanks);
            }
            return handInfo;
        }

        public HandInfo IsFlush(List<Card> hand)
        {
            HandInfo handInfo = null;

            // first check to see if any hand has 5 of the same suit
            List<Card> cardsOfSameSuit = null;
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                cardsOfSameSuit = hand.Where(card => card.suit == suit).ToList();
                if (cardsOfSameSuit.Count >= 5) { break; }
            }
            if (cardsOfSameSuit != null && cardsOfSameSuit.Count >= 5)
            {
                // add all cards in flush to relevant cards as 4 could be on the board.
                cardsOfSameSuit.OrderByDescending(x => CardValues.cardValues[x.value]);
                List<int> sigCardRanks = new List<int>();
                List<int> minorCardRanks = new List<int>();
                foreach (Card card in cardsOfSameSuit)
                {
                    sigCardRanks.Add(CardValues.cardValues[card.value]);
                }
                handInfo = new HandInfo(HandRanking.Flush, sigCardRanks, minorCardRanks);
            }

            return handInfo;
        }

        public HandInfo IsFullHouse(List<Card> hand)
        {
            HandInfo handInfo = null;
            List<int> cardRanks = new List<int>();
            foreach (Card card in hand)
            {
                cardRanks.Add(CardValues.cardValues[card.value]);
            }

            // order by count of rank and then by rank
            var cardRankGrouping = cardRanks.GroupBy(i => i).OrderByDescending(x => x.Count()).ThenByDescending(y => y.Key);
            if (cardRankGrouping.First().Count() == 3 && cardRankGrouping.ElementAt(1).Count() >= 2)
            {
                List<int> sigCardRanks = new List<int>();
                List<int> minorCardRanks = new List<int>();
                sigCardRanks.Add(cardRankGrouping.First().Key);
                minorCardRanks.Add(cardRankGrouping.ElementAt(1).Key);
                handInfo = new HandInfo(HandRanking.FullHouse, sigCardRanks, minorCardRanks);
            }

            return handInfo;
        }

        public HandInfo IsFourOfAKind(List<Card> hand)
        {
            HandInfo handInfo = null;
            List<int> sigCardRanks = new List<int>();
            List<int> minorCardRanks = new List<int>();
            List<int> cardRanks = new List<int>();
            foreach (Card card in hand)
            {
                cardRanks.Add(CardValues.cardValues[card.value]);
            }

            // order by count of rank and then by rank
            var cardRankGrouping = cardRanks.GroupBy(i => i).OrderByDescending(x => x.Count()).ThenByDescending(y => y.Key);
            if (cardRankGrouping.First().Count() == 4)
            {
                sigCardRanks.Add(cardRankGrouping.First().Key);
                List<int> uniqueCardRanks = cardRanks.OrderByDescending(x => x).Distinct().ToList();
                // add highest card not part of 4 of a kind to relevantCards
                if (uniqueCardRanks[0] != sigCardRanks[0])
                {
                    minorCardRanks.Add(uniqueCardRanks[0]);
                }
                else
                {
                    minorCardRanks.Add(uniqueCardRanks[1]);
                }
                handInfo = new HandInfo(HandRanking.FourOfAKind, sigCardRanks, minorCardRanks);

            }
            return handInfo;
        }

        public HandInfo IsStraightFlush(List<Card> hand)
        {
            HandInfo handInfo = null;

            // first check to see if any hand has 5 of the same suit
            List<Card> cardsOfSameSuit = null;
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                cardsOfSameSuit = hand.Where(card => card.suit == suit).ToList();
                if (cardsOfSameSuit.Count >= 5) { break; }
            }
            if (cardsOfSameSuit != null && cardsOfSameSuit.Count >= 5)
            {
                HandInfo straightHandInfo = IsStraight(cardsOfSameSuit);
                if (straightHandInfo != null)
                {
                    if (straightHandInfo.handComparisonInfo.sigCardRanks[0] != 14)
                    {
                        handInfo = new HandInfo(HandRanking.StraightFlush, straightHandInfo.handComparisonInfo.sigCardRanks, straightHandInfo.handComparisonInfo.minorCardRanks);
                    }
                    else
                    {
                        handInfo = new HandInfo(HandRanking.RoyalStraightFlush, straightHandInfo.handComparisonInfo.sigCardRanks, straightHandInfo.handComparisonInfo.minorCardRanks);
                    }
                }
            }
            return handInfo;
        }

        public HandInfo IsStraight(List<Card> hand)
        {
            HandInfo handInfo = null;
            List<int> cardRanks = new List<int>();
            foreach (Card card in hand)
            {
                int cardRank = CardValues.cardValues[card.value];
                if (!cardRanks.Contains(cardRank))
                {
                    cardRanks.Add(cardRank);
                    if (cardRank == 14) // an ace
                    {
                        cardRanks.Add(1);
                    }
                }
            }
            cardRanks.Sort();
            List<int> sequence = Utility.LongestSequence(cardRanks);
            // only the last card is relevant in a straight
            List<int> sigCardRanks = new List<int>();
            List<int> minorCardRanks = new List<int>();
            sigCardRanks.Add(sequence.Last());
            // we have a straight
            if (sequence.Count >= 5)
            {
                handInfo = new HandInfo(HandRanking.Straight, sigCardRanks, minorCardRanks);
            }
            return handInfo;

        }

    }
}
