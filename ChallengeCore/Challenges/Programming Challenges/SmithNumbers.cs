using System;
using System.Linq;
using System.Text;
using NumberTheoryLong;
using static System.Console;

namespace ChallengeCore.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Smith Numbers",
			"https://onlinejudge.org/index.php?option=onlinejudge&Itemid=8&page=show_problem&problem=983")]
		public class SmithNumbers : IChallenge
		{
			public void Solve()
			{
				var sbRet = new StringBuilder();
				// ReSharper disable once AssignNullToNotNullAttribute
				var cCases = int.Parse(ReadLine());
				for (var iCase = 0; iCase < cCases; iCase++)
				{
					LocalSolver.ReadCase().Solve(sbRet);
				}
				Write(sbRet.ToString());
			}

			public string RetrieveSampleInput()
			{
				return @"
1
4937774
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
4937775
";
			}

			private class LocalSolver
			{
				private long _lbound;

				private LocalSolver(long lbound)
				{
					_lbound = lbound;
				}

				public static LocalSolver ReadCase()
				{
					// ReSharper disable once AssignNullToNotNullAttribute
					return new LocalSolver(long.Parse(ReadLine()));
				}

				public void Solve(StringBuilder sbRet)
				{
					for (; ; _lbound++)
					{
						if (IsSmith(_lbound))
						{
							sbRet.Append(_lbound + Environment.NewLine);
							return;
						}
					}
				}

				private static bool IsSmith(long l)
				{
					if (l.IsPrime())
					{
						return false;
					}
					var digitSum = l.ToString().Select(c => c - '0').Sum();
					var factorSum = Factoring.Factor(l).Sum(
						factor => factor.Exp * factor.Prime.ToString().Select(c => c - '0').Sum());
					return factorSum == digitSum;
				}
			}
		}
	}
}
