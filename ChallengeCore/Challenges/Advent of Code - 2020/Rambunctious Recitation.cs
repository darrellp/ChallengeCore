using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code 2020", "Rambunctious Recitation (15)", "https://adventofcode.com/2020/day/15")]
        public class RambunctiousRecitation : IChallenge
        {
            public void Solve()
            {
                var init = ReadLine().Split(',').Select(int.Parse).ToList();

                // Part 1
                var mpWordStep = new Dictionary<int, int>();
                var thisStep = 1;

                for(var i = 0; i < init.Count - 1; i++)
                {
                    mpWordStep[init[i]] = ++thisStep;
                }

                var lastSpoken = init[^1];

                // 0 based indices means our 2020'th word spoken when thisStep==2019
                while (thisStep != 2020)
                {
                    speak(ref lastSpoken, ++thisStep, mpWordStep);
                    init.Add(lastSpoken);
                }
                WriteLine(lastSpoken);

                // Part 2
                mpWordStep = new Dictionary<int, int>();
                thisStep = 1;

                for (var i = 0; i < init.Count - 1; i++)
                {
                    mpWordStep[init[i]] = ++thisStep;
                }

                lastSpoken = init[^1];

                // 0 based indices means our 2020'th word spoken when thisStep==2019
                while (thisStep != 30_000_000)
                {
                    speak(ref lastSpoken, ++thisStep, mpWordStep);
                    init.Add(lastSpoken);
                }
                WriteLine(lastSpoken);
            }

            private void speak(ref int i, int thisStep, Dictionary<int, int> steps)
            {
                var ret = 0;

                if (steps.ContainsKey(i))
                {
                    ret = thisStep - steps[i];
                }

                steps[i] = thisStep;
                i = ret;
            }

            public string RetrieveSampleInput()
            {
				return @"
1,0,18,10,19,6";
			}

            public string RetrieveSampleOutput()
            {
                return @"
441
10613991
";
            }
        }
    }
}
