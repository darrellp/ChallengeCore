using System;
using System.Linq;

namespace ChallengeCore.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Code Chef", "Chef and Stones", "http://www.codechef.com/JAN15/problems/CHEFSTON")]
		// ReSharper disable once InconsistentNaming
		public class ChefStone : IChallenge
		{
			public void Solve()
			{
				var cTests = GetVal();
				for (var iTest = 0; iTest < cTests; iTest++)
				{
                    // ReSharper disable once IdentifierTypo
                    var vals = GetVals();
                    var time = vals[1];
					var typeTimes = GetVals();
					var typeProfits = GetVals();

                    var maxProfit = typeTimes.Zip(typeProfits, (t,p) => (time / t) * p).Max();
					Console.WriteLine(maxProfit.ToString());
				}
			}

			public string RetrieveSampleInput()
			{
				return @"
1
3 10
3 4 5
4 4 5
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
12
";
			}
		}
	}
}
