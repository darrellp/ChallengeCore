using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace ChallengeCore.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Rope Crisis in Ropeland!",
			"https://onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&page=show_problem&problem=1121")]
		public class RopeCrisis : IChallenge
		{
			public void Solve()
			{
				var ret = new StringBuilder();
				// ReSharper disable AssignNullToNotNullAttribute
				var cPuzzles = int.Parse(ReadLine());
				for (var iPuzzle = 0; iPuzzle < cPuzzles; iPuzzle++)
				{
					// ReSharper disable once PossibleNullReferenceException
					var info = GetDblVals();
					SolvePuzzle(info, ret);
				}
				// ReSharper restore AssignNullToNotNullAttribute
				Write(ret.ToString());
			}

			private void SolvePuzzle(List<double> info, StringBuilder strBuilder)
			{
				var x1 = info[0];
				var y1 = info[1];
				var x2 = info[2];
				var y2 = info[3];
				var r = info[4];

				var a = y2 - y1;
				var b = x1 - x2;
				var c = y1 * (x2 - x1) + x1 * (y1 - y2);
				var distToOrigin = Math.Abs(c / Math.Sqrt(a * a + b * b));
				double length;
				if (distToOrigin > r)
				{
					var dx = x1 - x2;
					var dy = y1 - y2;
					length = Math.Sqrt(dx * dx + dy * dy);
				}
				else
				{
					var originToP1 = Math.Sqrt(x1 * x1 + y1 * y1);
					var originToP2 = Math.Sqrt(x2 * x2 + y2 * y2);
					var pointToCircle1 = Math.Sqrt(originToP1 * originToP1 - r * r);
					var pointToCircle2 = Math.Sqrt(originToP2 * originToP2 - r * r);
					var cosTotalAngle = (x1 * x2 + y1 * y2) / (originToP1 * originToP2);
					var totalAngle = Math.Acos(cosTotalAngle);
					var angle1 = Math.Atan(pointToCircle1 / r);
					var angle2 = Math.Atan(pointToCircle2 / r);
					var angleOnCircle = totalAngle - angle1 - angle2;
					var lengthOnCircle = angleOnCircle * r;
					length = pointToCircle1 + pointToCircle2 + lengthOnCircle;
				}
				strBuilder.Append(length.ToString("F3") + Environment.NewLine);
			}

			public string RetrieveSampleInput()
			{
				return @"
2
1 1 -1 -1 1
1 1 -1 1 1
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
3.571
2.000
";
			}
		}
	}
}
