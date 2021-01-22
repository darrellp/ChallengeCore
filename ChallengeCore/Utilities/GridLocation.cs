using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ChallengeCore.Challenges
{
	public struct GridLocation
	{
		public readonly int Row;
		public readonly int Col;

		public bool IsNoLoc()
		{
			return Row < 0;
		}

        static GridLocation()
        {
			// Put an undisposed default NeighborInfo on the stack
			var ignore = new NeighborInfo();
        }

		public GridLocation(int row, int col)
		{
			if (!NeighborStack.Peek().AllowNegative && (row < 0 || col < 0))
			{
				throw new ArgumentException();
			}
			Row = row;
			Col = col;
		}

// ReSharper disable once UnusedParameter.Local
		private GridLocation(bool dontCare)
		{
			Row = Col = -1;
		}

		public static GridLocation NoLoc()
		{
			return new GridLocation(true);
		}

		[Pure]
		public IEnumerable<GridLocation> Neighbors()
		{
			var info = NeighborStack.Peek();
			for (var idRow = -1; idRow <= 1; idRow++)
			{
				for (var idCol = -1; idCol <= 1; idCol++)
				{
					if (idCol == 0 && idRow == 0)
					{
						if (info.IncludeOriginalCell)
						{
							yield return this;
						}
						continue;
					}
					if (idCol != 0 && idRow != 0 && info.F4Neighbors)
					{
						continue;
					}
					var curRow = Row + idRow;
					var curCol = Col + idCol;
					if (info.FWrap)
					{
						// We ignore AllowNegatives in this case
						if (curRow < 0)
						{
							curRow += info.CRows;
						}
						else if (curRow >= info.CRows)
						{
							curRow -= info.CRows;
						}
						if (curCol < 0)
						{
							curCol += info.CCols;
						}
						else if (curCol >= info.CCols)
						{
							curCol -= info.CCols;
						}
					}
					else
					{
						if ((!info.AllowNegative && (curRow < 0 || curCol < 0)) || curRow >= info.CRows || curCol >= info.CCols)
						{
							continue;
						}
					}
					yield return new GridLocation(curRow, curCol);
				}
			}
		}

		public override string ToString()
		{
			return $"r:{Row} c:{Col}";
		}

        private static readonly Stack<NeighborInfo> NeighborStack = new Stack<NeighborInfo>();

		public class NeighborInfo : IDisposable
		{
			internal bool F4Neighbors { get; }
			internal bool FWrap { get; }
			internal int CRows { get; }
			internal int CCols { get; }
			internal bool IncludeOriginalCell { get; }
			internal bool AllowNegative { get; }

			public NeighborInfo(
				int cRows = int.MaxValue,
				int cCols = int.MaxValue,
				bool f4Neighbors = true,
				bool fWrap = false,
				bool includeOriginalCell = false,
                bool allowNegative = false)
			{
				F4Neighbors = f4Neighbors;
				FWrap = fWrap;
				CRows = cRows;
				CCols = cCols;
				IncludeOriginalCell = includeOriginalCell;
                AllowNegative = allowNegative;
                NeighborStack.Push(this);
            }

            public void Dispose()
            {
                NeighborStack.Pop();
            }
        }
	}
}
