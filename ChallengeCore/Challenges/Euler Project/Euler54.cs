using System;
using static System.Console;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Euler Project", "Prob 54", "https://projecteuler.net/problem=54")]
        // ReSharper disable once UnusedMember.Global
        public class Euler54 : IChallenge
        {
            public void Solve()
            {
                var totalPlayer1Wins = 0;
                using (var file = new System.IO.StreamReader("Data Files/poker.txt"))
                {
                    string line;
                    var player1 = new PlayingCard[5];
                    var player2 = new PlayingCard[5];

                    while ((line = file.ReadLine()) != null)
                    {
                        var cardCodes = line.Split(' ');
                        for (var iCard = 0; iCard < 5; iCard++)
                        {
                            player1[iCard] = new PlayingCard(cardCodes[iCard]);
                            player2[iCard] = new PlayingCard(cardCodes[iCard + 5]);
                        }

                        Array.Sort(player1);
                        Array.Sort(player2);
                        if (CompareHands(player1, player2) > 0)
                        {
                            totalPlayer1Wins++;
                        }
                    }
                }

                WriteLine(totalPlayer1Wins);
            }

            public string RetrieveSampleInput()
            {
                return null;
            }

            public string RetrieveSampleOutput()
            {
                return @"
376
";
            }

            private static int HighCard(PlayingCard[] hand)
            {
                return hand[4].Rank;
            }

            private delegate bool IsType(PlayingCard[] hand, out int high);

            private static readonly IsType[] CheckFns = new IsType[]
            {
                IsRoyalFlush,
                IsStraightFlush,
                IsFourOfAKind,
                IsFullHouse,
                IsFlush,
                IsStraight,
                IsThreeOfAKind,
                IsTwoPairs,
                IsOnePair,
                IsHigh
            };

            private static int CompareHands(PlayingCard[] hand1, PlayingCard[] hand2)
            {
                foreach (var checkFn in CheckFns)
                {
                    var isInHand1 = checkFn(hand1, out var high1);
                    var isInHand2 = checkFn(hand2, out var high2);

                    if (!isInHand1 && !isInHand2)
                    {
                        continue;
                    }

                    if (!isInHand1)
                    {
                        return -1;
                    }

                    if (!isInHand2)
                    {
                        return 1;
                    }

                    if (high1 != high2)
                    {
                        return high1.CompareTo(high2);
                    }

                    for (var iCard = 4; iCard >= 0; iCard--)
                    {
                        if (hand1[iCard].Rank != hand2[iCard].Rank)
                        {
                            return hand1[iCard].Rank.CompareTo(hand2[iCard].Rank);
                        }
                    }
                }

                return 0;
            }

            private static bool IsHigh(PlayingCard[] hand, out int high)
            {
                high = hand[4].Rank;
                return true;
            }

            private static bool IsOnePair(PlayingCard[] hand, out int high)
            {
                high = 0;
                for (var iCard = 1; iCard < 5; iCard++)
                {
                    if (hand[iCard].Rank == hand[iCard - 1].Rank)
                    {
                        high = hand[iCard].Rank;
                        return true;
                    }
                }

                return false;
            }

            private static bool IsTwoPairs(PlayingCard[] hand, out int high)
            {
                high = 0;
                int pairs = 0;
                for (int iCard = 1; iCard < 5; iCard++)
                {
                    if (hand[iCard].Rank == hand[iCard - 1].Rank)
                    {
                        high = hand[iCard].Rank;
                        if (++pairs == 2)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            private static bool IsThreeOfAKind(PlayingCard[] hand, out int high)
            {
                high = hand[2].Rank;
                return hand[0].Rank == hand[2].Rank ||
                       hand[1].Rank == hand[3].Rank ||
                       hand[2].Rank == hand[4].Rank;
            }

            private static bool IsFullHouse(PlayingCard[] hand, out int high)
            {
                high = hand[4].Rank;
                return (hand[0].Rank == hand[1].Rank && hand[2].Rank == hand[4].Rank) ||
                       (hand[0].Rank == hand[2].Rank && hand[3].Rank == hand[4].Rank);
            }

            private static bool IsFourOfAKind(PlayingCard[] hand, out int high)
            {
                high = hand[2].Rank;
                return hand[0].Rank == hand[3].Rank || hand[1].Rank == hand[4].Rank;
            }

            private static bool IsStraightFlush(PlayingCard[] hand, out int high)
            {
                return IsFlush(hand, out high) && IsStraight(hand, out high);
            }

            private static bool IsRoyalFlush(PlayingCard[] hand, out int high)
            {
                return IsStraightFlush(hand, out high) && high == 12;
            }

            static bool IsStraight(PlayingCard[] hand, out int high)
            {
                high = HighCard(hand);
                for (var iCard = 1; iCard < 5; iCard++)
                {
                    if (hand[iCard].Rank != hand[0].Rank + iCard)
                    {
                        return false;
                    }
                }

                return true;
            }

            static bool IsFlush(PlayingCard[] hand, out int high)
            {
                high = HighCard(hand);
                for (var iCard = 1; iCard < 5; iCard++)
                {
                    if (hand[iCard].Suit != hand[0].Suit)
                    {
                        return false;
                    }
                }

                return true;
            }

            class PlayingCard : IComparable
            {
                private static readonly char[] Suits = new[] {'C', 'D', 'H', 'S'};

                private static readonly char[] Ranks = new[]
                    {'2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'};

                public int Suit { get; }
                public int Rank { get; }

                internal PlayingCard(string code)
                {
                    // Assuming code is valid
                    Rank = Array.FindIndex(Ranks, c => c == code[0]);
                    Suit = Array.FindIndex(Suits, c => c == code[1]);
                }

                public int CompareTo(object obj)
                {
                    var other = (PlayingCard) obj;
                    var bigger = Rank.CompareTo(other.Rank);
                    return bigger == 0 ? Suit.CompareTo(other.Suit) : bigger;
                }
            }
        }
    }
}
