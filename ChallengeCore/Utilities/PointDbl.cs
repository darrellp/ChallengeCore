using System;

namespace ChallengeCore.Challenges
{
	public struct PointDbl
	{
		public readonly double X;
		public readonly double Y;

		public PointDbl(double x, double y)
		{
			X = x;
			Y = y;
		}

		public double DistanceTo(PointDbl pt)
		{
			var dx = X - pt.X;
			var dy = Y - pt.Y;
			return Math.Sqrt(dx * dx + dy * dy);
		}

		public override string ToString()
		{
			return "(" + X.ToString("F3") + "," + Y.ToString("F3") + ")";
		}
	}
}
