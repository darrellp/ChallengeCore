using System.Linq;
using NumberTheoryLong;
using static System.Console;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code 2020", "Shuttle Search (13)", "https://adventofcode.com/2020/day/13")]
        public class ShuttleSearch : IChallenge
        {
            private int _startTime;
            private int[] _busIds;

            public void Solve()
            {
                _startTime = GetVal();
                _busIds = ReadLine().
                    Split(',', int.MaxValue).
                    Select(s => s[0] == 'x' ? -1 : int.Parse(s)).
                    ToArray();

                // Part 1
                var waitTime = int.MaxValue;
                var bestBus = -1;

                foreach (var busId in _busIds.Where(id => id > 0))
                {
                    var rem = _startTime % busId;
                    if (rem == 0)
                    {
                        waitTime = 0;
                        bestBus = busId;
                    }

                    var thisWait = busId - rem;
                    if (thisWait < waitTime)
                    {
                        waitTime = thisWait;
                        bestBus = busId;
                    }
                }

                WriteLine(waitTime * bestBus);

                // Part 2
                // Chinese Remainder Theorem

                var mods = _busIds.Where(v => v > 0).Select(v => (long)v).ToArray();
                var aVals = new long[mods.Length];
                var iVals = 0;

                for (var i = 0; i < _busIds.Length; i++)
                {
                    if (_busIds[i] > 0)
                    {
                        aVals[iVals++] = _busIds[i] - i;
                    }
                }

                var soln = ChineseRemainder.CRT(aVals, mods);
                WriteLine(soln);
            }

            public string RetrieveSampleInput()
            {
                return @"
1015292
19,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,743,x,x,x,x,x,x,x,x,x,x,x,x,13,17,x,x,x,x,x,x,x,x,x,x,x,x,x,x,29,x,643,x,x,x,x,x,37,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,23";
            }

            public string RetrieveSampleOutput()
            {
                return @"
3215
1001569619313439
";
            }
        }
    }
}
