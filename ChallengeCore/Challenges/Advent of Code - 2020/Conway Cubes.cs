using System;
using System.Collections.Generic;
using static System.Console;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code 2020", "Conway Cubes (17)", "https://adventofcode.com/2020/day/17")]
        public class ConwayCubes : IChallenge
        {
            private const int CycleCount = 6;
            private int _dimInput;
            private bool[,,] _universe3;
            private bool[,,,] _universe4;
            private int _finalDimXY;
            private int _finalDimWZ;

            public void Solve()
            {
                // Reading the first line for dimension info and assuming the initial
                // layout is square
                var line = ReadLine();
                // ReSharper disable once PossibleNullReferenceException
                _dimInput = line.Length;
                _finalDimXY = _dimInput + 2 * CycleCount;
                _finalDimWZ = CycleCount * 2 + 1;
                _universe3 = new bool[_finalDimXY, _finalDimXY, _finalDimWZ];
                _universe4 = new bool[_finalDimXY, _finalDimXY, _finalDimWZ, _finalDimWZ];
                var iRowInit = CycleCount;

                do
                {
                    for (var iCol = CycleCount; iCol < CycleCount + _dimInput; iCol++)
                    {
                        if (line[iCol - CycleCount] == '#')
                        {
                            _universe3[iRowInit, iCol, CycleCount] = true;
                            _universe4[iRowInit, iCol, CycleCount, CycleCount] = true;
                        }
                    }

                    iRowInit++;
                } while ((line = ReadLine()) != null);

                // Part 1
                for (var iGen = 1; iGen <= 6; iGen++)
                {
                    IterateRules(iGen);
                }

                var total = 0;

                for (var iRow = 0; iRow < _finalDimXY; iRow++)
                {
                    for (var iCol = 0; iCol < _finalDimXY; iCol++)
                    {
                        for (var iPlane = 0; iPlane < _finalDimWZ; iPlane++)
                        {
                            if (_universe3[iRow, iCol, iPlane])
                            {
                                total++;
                            }
                        }
                    }
                }

                WriteLine(total);

                // Part 2
                for (var iGen = 1; iGen <= 6; iGen++)
                {
                    IterateRules2(iGen);
                }

                total = 0;

                for (var iRow = 0; iRow < _finalDimXY; iRow++)
                {
                    for (var iCol = 0; iCol < _finalDimXY; iCol++)
                    {
                        for (var iPlane1 = 0; iPlane1 < _finalDimWZ; iPlane1++)
                        {
                            for (var iPlane2 = 0; iPlane2 < _finalDimWZ; iPlane2++)
                            {
                                if (_universe4[iRow, iCol, iPlane1, iPlane2])
                                {
                                    total++;
                                }
                            }
                        }
                    }
                }

                WriteLine(total);
            }

            private void IterateRules(int iGen)
            {
                var nextGen = new bool[_finalDimXY, _finalDimXY, _finalDimWZ];

                // Only live cubes are from CycleCount - iGen to CycleCount + _dimInput + iGen in x and y directions and
                // from CycleCount - iGen to CycleCount + iGen in the Z direction.

                for (var iRow = CycleCount - iGen;  iRow < CycleCount + _dimInput + iGen; iRow++)
                {
                    for (var iCol = CycleCount - iGen; iCol < CycleCount + _dimInput + iGen; iCol++)
                    {
                        for (var iPlane = CycleCount - iGen; iPlane <= CycleCount + iGen; iPlane++)
                        {
                            var count = CountNeighbors(iRow, iCol, iPlane);
                            bool newVal;
                            var oldVal = _universe3[iRow, iCol, iPlane];
                            if (oldVal)
                            {
                                newVal = (count == 2 || count == 3);
                            }
                            else
                            {
                                newVal = count == 3;
                            }

                            nextGen[iRow, iCol, iPlane] = newVal;
                        }
                    }
                }

                _universe3 = nextGen;
            }

            private int CountNeighbors(int row, int col, int plane)
            {
                int sum = 0;

                foreach (var loc in Neighbors(row, col, plane))
                {
                    var iRow = loc.Item1;
                    var iCol = loc.Item2;
                    var iPlane = loc.Item3;
                    if (_universe3[iRow, iCol, iPlane])
                    {
                        sum++;
                    }
                }

                return sum;
            }

            private IEnumerable<(int, int, int)> Neighbors(int row, int col, int plane)
            {
                for (var iRow = Math.Max(0, row - 1); iRow <= Math.Min(_finalDimXY - 1, row + 1); iRow++)
                {
                    for (var iCol = Math.Max(0, col - 1); iCol <= Math.Min(_finalDimXY - 1, col + 1); iCol++)
                    {
                        for (var iPlane = Math.Max(0, plane - 1); iPlane <= Math.Min(_finalDimWZ - 1, plane + 1); iPlane++)
                        {
                            if (row != iRow || col != iCol || plane != iPlane)
                            {
                                yield return (iRow, iCol, iPlane);
                            }
                        }
                    }
                }
            }

            private void IterateRules2(int iGen)
            {
                var nextGen = new bool[_finalDimXY, _finalDimXY, _finalDimWZ, _finalDimWZ];

                // Only live cubes are from CycleCount - iGen to CycleCount + _dimInput + iGen in x and y directions and
                // from CycleCount - iGen to CycleCount + iGen in the Z/W direction.

                for (var iRow = CycleCount - iGen; iRow < CycleCount + _dimInput + iGen; iRow++)
                {
                    for (var iCol = CycleCount - iGen; iCol < CycleCount + _dimInput + iGen; iCol++)
                    {
                        for (var iPlane1 = CycleCount - iGen; iPlane1 <= CycleCount + iGen; iPlane1++)
                        {
                            for (var iPlane2 = CycleCount - iGen; iPlane2 <= CycleCount + iGen; iPlane2++)
                            {
                                var count = CountNeighbors2(iRow, iCol, iPlane1, iPlane2);
                                bool newVal;
                                var oldVal = _universe4[iRow, iCol, iPlane1, iPlane2];
                                if (oldVal)
                                {
                                    newVal = (count == 2 || count == 3);
                                }
                                else
                                {
                                    newVal = count == 3;
                                }

                                nextGen[iRow, iCol, iPlane1, iPlane2] = newVal;
                            }
                        }
                    }
                }

                _universe4 = nextGen;
            }

            private int CountNeighbors2(int row, int col, int plane1, int plane2)
            {
                int sum = 0;

                foreach (var loc in Neighbors2(row, col, plane1, plane2))
                {
                    var iRow = loc.Item1;
                    var iCol = loc.Item2;
                    var iPlane1 = loc.Item3;
                    var iPlane2 = loc.Item4;
                    if (_universe4[iRow, iCol, iPlane1, iPlane2])
                    {
                        sum++;
                    }
                }

                return sum;
            }

            private IEnumerable<(int, int, int, int)> Neighbors2(int row, int col, int plane1, int plane2)
            {
                for (var iRow = Math.Max(0, row - 1); iRow <= Math.Min(_finalDimXY - 1, row + 1); iRow++)
                {
                    for (var iCol = Math.Max(0, col - 1); iCol <= Math.Min(_finalDimXY - 1, col + 1); iCol++)
                    {
                        for (var iPlane1 = Math.Max(0, plane1 - 1); iPlane1 <= Math.Min(_finalDimWZ - 1, plane1 + 1); iPlane1++)
                        {
                            for (var iPlane2 = Math.Max(0, plane2 - 1);
                                iPlane2 <= Math.Min(_finalDimWZ - 1, plane2 + 1);
                                iPlane2++)
                            {
                                if (row != iRow || col != iCol || plane1 != iPlane1 || plane2 != iPlane2)
                                {
                                    yield return (iRow, iCol, iPlane1, iPlane2);
                                }
                            }
                        }
                    }
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
#.#####.
#..##...
.##..#..
#.##.###
.#.#.#..
#.##..#.
#####..#
..#.#.##";
            }

            public string RetrieveSampleOutput()
            {
                return @"
353
2472
";
            }
        }
    }
}
