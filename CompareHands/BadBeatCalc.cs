using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareHands
{
    public class BadBeatCalc
    {
        public List<Card> DetermineBadBeat(String eventualWinner, Dictionary<String, List<Card>> hands, out String sixCardWinners)
        {
            sixCardWinners = String.Empty;
            // verify eventual winner has a hand, there are 2 hands and each hand has 6 cards
            if (!hands.ContainsKey(eventualWinner)) { return null; }
            if (hands.Count < 2) { return null; }
            foreach(List<Card> hand in hands.Values)
            {
                if (hand.Count != 6) { return null; }
            }
            // see if the eventual winner was in the lead with 6 cards
            AnalyzeHands analyzeHands = new AnalyzeHands();
            List<HandInfo> results = analyzeHands.OrderHands(hands);
            List<Card> cardOuts = new List<Card>();

            // special case, multiple players tied for the six card winner
            long sixCardHighRank = results[0].handRank;
            int playersWithSixCardHighRank = results.Count(x => x.handRank == sixCardHighRank);
            if (playersWithSixCardHighRank == 1)
            {
                sixCardWinners = results[0].name;
            }
            // special case, multiple players tied for the six card winner
            else
            {
                foreach (HandInfo player in results)
                {
                    if (player.handRank == sixCardHighRank && player.name != eventualWinner)
                    {
                        if (sixCardWinners == String.Empty) { sixCardWinners = player.name; }
                        else { sixCardWinners = sixCardWinners + ", " + player.name; }
                    }
                }
            }

            //sixCardWinners = results[0].name;
            if (sixCardWinners != eventualWinner)
            {
                // eventual winner won on the last card
                Deck deckInUse = new Deck(analyzeHands.DeckInUse);

                // loop through the 52 cards and determine which ones were a winner
                foreach (CardInDeck card in deckInUse)
                {
                    if (!card.cardInUse)
                    {
                        //var newHands = hands.ToDictionary(entry => entry.Key, entry => entry.Value);
                        //Dictionary<String, List<Card>> newHands = new Dictionary<string, List<Card>>(hands);
                        // add the seventh card to each hand
                        foreach (List<Card> hand in hands.Values)
                        {
                            if (hand.Count == 6) { hand.Add(card); }
                            else 
                            {
                                hand[6] = card;
                            }
                        }
                        List<HandInfo> badBeatResults = analyzeHands.OrderHands(hands);
                        if (badBeatResults[0].name == eventualWinner)
                        {
                            cardOuts.Add((Card)card);
                        }

                    }
                }

            }

            return cardOuts;
        }
    }
}
