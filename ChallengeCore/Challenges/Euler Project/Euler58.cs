using NumberTheoryLong;
using static System.Console;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Euler Project", "Prob 58",
            "https://projecteuler.net/problem=58")]
        // ReSharper disable once UnusedMember.Global
        public class Euler58 : IChallenge
        {
            public void Solve()
            {
                var cPrimes = 0;
                var corner = 1;
                for (var side = 3; side < int.MaxValue; side += 2)
                {
                    var inc = side - 1;
                    cPrimes +=
                        (Primes.IsPrime(corner + inc) ? 1 : 0) +
                        (Primes.IsPrime(corner + 2 * inc) ? 1 : 0) +
                        (Primes.IsPrime(corner + 3 * inc) ? 1 : 0) +
                        (Primes.IsPrime(corner + 4 * inc) ? 1 : 0);
                    if (10 * cPrimes < 2 * side - 1)
                    {
                        WriteLine(side);
                        break;
                    }

                    corner += 4 * inc;
                }
            }

            public string RetrieveSampleInput()
            {
                return null;
            }

            public string RetrieveSampleOutput()
            {
                return @"
26241
";
            }
        }
    }
}
