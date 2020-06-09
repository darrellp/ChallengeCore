using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace ChallengeCore.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("UVA", "Firetruck",
			"https://onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=4&page=show_problem&problem=144")]
		// ReSharper disable once InconsistentNaming
		public class Firetruck : IChallenge
		{
			public void Solve()
            {
                int? destN;
                var iCase = 0;
                while ((destN = GetValue()) != null)
                {
                    iCase++;
                    var dest = destN.Value;
					var openRoads = new Dictionary<int, List<int>>();
                    while (true)
                    {
                        var route = GetVals();
                        if (route[0] == 0 && route[1] == 0)
                        {
                            break;
                        }
                        AddRoute(openRoads, route[0], route[1]);
                    }

                    var routes = FindRoutes(openRoads, 0, 1, dest, new List<int>());
                    WriteLine($"CASE {iCase}:");
                    foreach (var route in routes)
                    {
                        var fFirst = true;
                        foreach (var node in route)
                        {
                            if (!fFirst)
                            {
                                Write(" ");
                            }

                            fFirst = false;
                            Write(node);
                        }

                        WriteLine();
                    }
                    WriteLine($"There are {routes.Count} routes from the firestation to streetcorner {dest}.");
                }
			}

            private List<List<int>> FindRoutes(Dictionary<int, List<int>> map, uint prohibited, int start, int end, List<int> routeThusFar)
            {
                var ret = new List<List<int>>();
                var newRouteThusFar = routeThusFar.ToList();
                newRouteThusFar.Add(start);
                if (start == end)
                {
                    ret.Add(newRouteThusFar);
                    return ret;
                }

                var newProhibited = prohibited | (uint)(1 << start);

                foreach (var nextNode in map[start].Where(nextNode => (prohibited & (1 << nextNode)) == 0))
                {
                    ret.AddRange(FindRoutes(map, newProhibited, nextNode, end, newRouteThusFar));
                }

                return ret;
            }

            private void AddRoute(Dictionary<int, List<int>> openRoads, int i1, int i2)
            {
                AddOneWayRoute(openRoads, i1, i2);
                AddOneWayRoute(openRoads, i2, i1);
            }

            private void AddOneWayRoute(Dictionary<int, List<int>> openRoads, int start, int end)
            {
                if (!openRoads.ContainsKey(start))
                {
                    openRoads[start] = new List<int>();
                }
                openRoads[start].Add(end);
            }

            public string RetrieveSampleInput()
			{
				return @"
6
1 2
1 3
3 4
3 5
4 6
5 6
2 3
2 4
0 0
4
2 3
3 4
5 1
1 6
7 8
8 9
2 5
5 7
3 1
1 8
4 6
6 9
0 0
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
CASE 1:
1 2 3 4 6
1 2 3 5 6
1 2 4 3 5 6
1 2 4 6
1 3 4 6
1 3 5 6
1 3 2 4 6
There are 7 routes from the firestation to streetcorner 6.
CASE 2:
1 5 2 3 4
1 5 7 8 9 6 4
1 6 4
1 6 9 8 7 5 2 3 4
1 3 2 5 7 8 9 6 4
1 3 4
1 8 7 5 2 3 4
1 8 9 6 4
There are 8 routes from the firestation to streetcorner 4.
";
			}
		}
	}
}
