﻿using static System.Math;
using static System.Console;

namespace ChallengeCore.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Euler Project", "Prob 172",
            "https://projecteuler.net/problem=172")]
		public class Euler172 : IChallenge
		{
			public void Solve()
			{
				var ret = 0UL;
				var fact18 = Fact(18);

				for (var m3 = 0; m3 <= 6; m3++)
				{
					for (var m2 = 0; m2 <= 9; m2++)
					{
						var m1 = 18 - 3 * m3 - 2 * m2;
						if (m1 < 0 || m1 + m2 + m3 > 10)
						{
							continue;
						}
						ret += (fact18 / ((ulong)Pow(6L, m3) * (ulong)Pow(2L, m2))) *
							Comb(10, m1) * Comb(10 - m1, m2) * Comb(10 - m1 - m2, m3);
					}
				}
				WriteLine(ret * 9 / 10);
			}

			public string RetrieveSampleInput() { return null; }
			public string RetrieveSampleOutput()
			{
				return @"
227485267000992000
";
			}
		}
	}
}
