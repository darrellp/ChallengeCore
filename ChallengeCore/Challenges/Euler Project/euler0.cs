using static System.Console;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Euler Project", "Prob 0 (Reg Problem)", "https://projecteuler.net/register")]
        // ReSharper disable once UnusedMember.Global
        public class Euler0 : IChallenge
        {
            public void Solve()
            {
                var sum = 0L;
                for (long i = 1; i < 644000; i += 2)
                {
                    sum += i*i;
                }

                WriteLine((long) sum);
            }

            public string RetrieveSampleInput()
            {
                return null;
            }

            public string RetrieveSampleOutput()
            {
                return @"
44514997333226000
";
            }
        }
    }
}