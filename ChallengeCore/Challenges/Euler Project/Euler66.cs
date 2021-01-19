using System.Numerics;
using NumberTheoryBig;
using static System.Console;


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

                    BigInteger x, y;
                    Pells.SolvePells(i, 1, out x, out y);
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
