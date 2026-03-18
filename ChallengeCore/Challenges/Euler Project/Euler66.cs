using System.Numerics;
using static System.Console;
using static NumberTheory.Pells;
using static NumberTheory.Utilities;


namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Euler Project", "Prob 66", "https://projecteuler.net/problem=66")]
        // ReSharper disable once UnusedMember.Global
        public class Euler66 : IChallenge
        {
            public void Solve()
            {
                BigInteger largest = 0;
                BigInteger largestD = 0;

                for (BigInteger i = 0; i < 1000; i++)
                {
                    var sqrt = i.IntegerSqrt();
                    if (i == sqrt * sqrt)
                    {
                        continue;
                    }

                    SolvePells(i, 1, out var x, out _);
                    if (x > largest)
                    {
                        largest = x;
                        largestD = i;
                    }
                }

                WriteLine((long) largestD);
            }

            public string RetrieveSampleInput()
            {
                return null;
            }

            public string RetrieveSampleOutput()
            {
                return @"
661
";
            }
        }
    }
}
