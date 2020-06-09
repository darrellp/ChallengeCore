using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using static System.Console;

namespace ChallengeCore.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "The Trip",
			"https://onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&page=show_problem&problem=1078")]
		public class Trip : IChallenge
		{
			public void Solve()
			{
				var ret = new StringBuilder();
				while (true)
				{
					var nextCase = GetCase();
					if (nextCase == null)
					{
						break;
					}
					var remainder = nextCase.Sum() % nextCase.Count();
					var baseval = nextCase.Sum() / nextCase.Count();
					var countMore = nextCase.Count;
					var amt = 0;

					foreach (var t in nextCase.Where(t => t <= baseval))
					{
						countMore--;
						amt += baseval - t;
					}
					var res = (amt + Math.Max(0, remainder - countMore)).ToString();
					var newval = "$" + res.Substring(0, res.Length - 2) + "." + res.Substring(res.Length - 2);
					ret.Append(newval + Environment.NewLine);
				}
				Write(ret.ToString());
			}

			private List<int> GetCase()
			{
				// ReSharper disable AssignNullToNotNullAttribute
				var cStudents = int.Parse(ReadLine());
				if (cStudents == 0)
				{
					return null;
				}
				var ret = new List<int>();
				for (var iStudent = 0; iStudent < cStudents; iStudent++)
				{
					ret.Add(int.Parse(new string(ReadLine().Where(Char.IsDigit).ToArray())));
				}
				return ret;
				// ReSharper restore AssignNullToNotNullAttribute
			}

			public string RetrieveSampleInput()
			{
				return @"
3
10.00
20.00
30.00
4
15.00
15.01
3.00
3.01
0";
			}

			public string RetrieveSampleOutput()
			{
				return @"
$10.00
$11.99
";
			}
		}
	}
}
