using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static System.Console;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Euler Project", "Prob 62", "https://projecteuler.net/problem=62")]
        // ReSharper disable once UnusedMember.Global
        public class Euler62 : IChallenge
        {
            static readonly Dictionary<long, List<int>> Dict = new Dictionary<long, List<int>>();

            public void Solve()
            {
                Dict.Clear();
                for (var i = 346; i < int.MaxValue; i++)
                {
                    var key = GetKey(i);
                    if (Dict.ContainsKey(key))
                    {
                        var list = Dict[key];
                        list.Add(i);
                        if (list.Count == 5)
                        {
                            WriteLine((long) list[0] * list[0] * list[0]);
                            return;
                        }
                    }
                    else
                    {
                        Dict[key] = new List<int> {i};
                    }
                }
            }

            public string RetrieveSampleInput() => null;

            public string RetrieveSampleOutput() => @"
127035954683
";

            private static long GetKey(int n)
            {
                return long.Parse(string.Concat(((long) n * n * n).ToString(CultureInfo.InvariantCulture)
                    .OrderBy(c => -c)));
            }
        }
    }
}
