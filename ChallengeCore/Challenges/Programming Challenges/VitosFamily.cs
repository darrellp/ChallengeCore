using System;
using System.Linq;
using System.Text;
using static System.Console;

namespace ChallengeCore.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Vito's Family",
			"https://onlinejudge.org/index.php?option=onlinejudge&Itemid=8&page=show_problem&problem=982")]
		public class VitosFamily : IChallenge
		{
			public void Solve()
			{
				var sbRet = new StringBuilder();
				var cCases = GetVal();

				for (var iCase = 0; iCase < cCases; iCase++)
				{
					// Best position for vito is position with half the houses to the left and
					// half to the right.
					var vals = GetVals();
					var vitoIndex = vals[0] / 2;
					vals = vals.Skip(1).ToList();
					vals.Sort();
					var vitoPos = vals[vitoIndex];
					var minimalDistance = vals.Select(hn => Math.Abs(hn - vitoPos)).Sum();
					sbRet.Append(minimalDistance + Environment.NewLine);
				}
				Write(sbRet.ToString());
			}

			public string RetrieveSampleInput()
			{
				return @"
2
2 2 4
3 2 4 6
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
2
4
";
			}
		}
	}
}
