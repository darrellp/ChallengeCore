﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static System.Console;
using static System.Math;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code 2020", "Toboggan Trajectory (3))", "https://adventofcode.com/2020/day/3")]
        public class TobogganTrajectory : IChallenge
        {
            public void Solve()
            {
                var treeMap = GetLines();
                var c13 = CountTrees(treeMap, 3, 1);
                WriteLine(c13);

                // Part 2
                var c11 = CountTrees(treeMap, 1, 1);
                var c15 = CountTrees(treeMap, 5, 1);
                var c17 = CountTrees(treeMap, 7, 1);
                var c21 = CountTrees(treeMap, 1, 2);
                var answer2 = c11 * c13 * c15 * c17 * c21;
                WriteLine(answer2);
            }

            private int CountTrees(List<string> treeMap, int slpX, int slpY)
            {
                var width = treeMap[0].Length;
                var col = 0;
                var treeSum = 0;

                for (var iRow = 0; iRow < treeMap.Count; iRow += slpY)
                {
                    if (treeMap[iRow][col] == '#')
                    {
                        treeSum++;
                    }

                    col = (col + slpX) % width;
                }

                return treeSum;
            }

            public string RetrieveSampleInput()
            {
                return @"
...#..............#.#....#..#..
...#..#..#..............#..#...
....#.#.......#............#...
..##.....##.........#........##
...#...........#...##.#...#.##.
..#.#...#....#.....#........#..
....##.###.....#..#.......#....
.#..##...#.....#......#..#.....
............##.#...#.#.....#.#.
..........#....#....#.#...#...#
..##....#.#.#......#.........#.
#.#.........#..............##..
....##.##......................
....##..#...........#..........
..#..#.#........##....#......#.
..............#..#....#.....#..
.............#...#.....#...#...
.#...........#..........#...#..
.#......#.......#......#.......
#..#.............#..#....##.###
........#.#...........##.#...#.
......#..#.....##......#.......
.....#.....#....#..............
#...##.#......#......#...#.....
...........................#...
...#....................#.....#
..#.....#...#.....##.....#.....
....................#......#..#
.......#.....##......##....#...
#........##...#.....##..#...#..
........#..#.#......#..###..#.#
##.....#.............#.#....#..
..#.................#....######
.#.#..#.....#.#..........#.#...
.........#....#...#............
........#..#.....#.............
............#.#.............##.
...#....#..#......#............
.##....#.....#...#.#...........
..#..............#...........##
.....#.#.##...#................
..........#..#.#..........##..#
..#....#...#...#.....######....
....#.#..#........#....#.###...
.......................#.......
..#.....#.##................#..
.....#......#..#.....#........#
.#...###.......#.#.........#..#
............#..................
..#.........##.........##......
#...........#.#.......###.#....
.#...#.....#.........###.....#.
.#............#........#..#....
...##.#......##................
........#...#...#...#..........
.......#.##......##.#..........
....##.......#..#....#....#....
......#.........###........#...
#....#....####....##......#....
......................#....#.#.
...#.#.#....#.#...#...#......#.
......#.....##.#...........###.
#........#.........##......#.#.
....##.....#.....#..#..........
......#...#...#.........#...##.
..#........#..................#
.........#..##.#..#..#...#.#..#
.....#.....#...#.....###.....##
.............#....#...#........
..........#.#.#...#..#...#....#
#...............##.......#.....
#...#.............#..#...#....#
..#...#...##...##...#..#.......
..#..#........#.#...........#..
.....#.....#..................#
....#....##....###..###...##...
..#......###.........##....#.##
.......#.##...#.......#..#.....
...#.#.#.#.....##..#..#........
................##....#.#......
..#...#...#...#.....##.#...#..#
..#..#.....#..##....#....#.....
.###...#......#........#.....##
##......#..#........#..........
....#...#..#....##..#......####
.#.....##....#..........#......
.#...#....#.........#...#....#.
.....#..#.#..#......#..##....#.
...#.##...#...#........#......#
.#..#...#.#..#.........#...#...
#....#......##.....#.......#...
..##............##..#.#.#...#..
##.......#.......##............
#......#.##........#...#...#...
.#.#.......##.........#..#.#...
.............##.#........#.....
.#..#...###...#..#.............
.....#...#..#....#.......#.....
#.#.........#.#.#...#...#.#....
.....#.......#.##.##...#....#..
.#.##..#.....#...#.#.#.#.#..#..
..........#...................#
.....#.#.#...##.........#..#..#
.#..#....##......#...#.........
.##......#......#...#........#.
.....##.#......#............#.#
.#.....##..#...........##......
.....#......#.......##....#....
..#..##..........#.#..........#
#.#.......##..#..##.#....#.....
.......#..#.#.......##......#.#
....#...##...#..............#..
.....#.........#......#...##...
#.........#........##..#.....#.
.#.#..#.....##.......#......#..
........#..#....#.....###..#...
#.#..#.#..........#............
..#......##..#....#.........#..
#..............................
.......#............#..#..#.#..
.#.....#.#....#..#.##.#........
.......#.###.#........##.#..#..
..............#....#.....##.#..
#..............#....#.###......
.#..#..#...###............#...#
.#.##...#....#..#...#...#......
......##..#..#......#..#....#..
.........#.......##............
...........##...#..#....####...
.#..................#..........
#...#..#..................#....
..............#.....##.....#...
..#.#..#...##..#.....#.....#..#
....#....#.#.........#.....#...
.#.......#...#....#...#.#..#..#
#.........##.....##.......#...#
#..#............#....#........#
..........##...#......#....#...
.......#..##...............#...
#............#.#.#.....#.......
.#........##...#...............
..##....#.....#..#.##.#......#.
.#...#.............#...#.....#.
...##....#.......#......#.#..#.
#......................#..#.##.
...#..........#..#.........#...
..#......#.......#.#....#......
....#............#...#......#..
.....#..#..##...#...#.........#
.....#......#....#....#........
.............#..#..........#...
....#..............#.....#.#...
....#.................#.#...#.#
.........#.#...........###.#.##
#...........#..##.#....#.##.#..
#.#.....#......................
##.#.........#....##.#.#..#.#..
#..........#.#.#.#.#..#..##..#.
..#...#..###.........#......#..
.....#......#..#.#............#
...........#...#.#.#.###....#..
#....#..#.......##.#.......##..
..............#.....##.#.......
.#.....#.#..#.........#.#.#..#.
..#..#..#..#................#..
...........#..#.#...#.........#
.#..#..#...#........#...#.#..#.
...#.#..#......#..#............
........#......##.....#....#...
#...#......##.#.#..............
.#........................#....
#.#.....#.##.....#..#.#........
#..........##.#.......#....#..#
#...#..#..#.....#....#....#....
#...........#..#.#....##.##....
##......#..#........#.......##.
#........#..#...#..........#...
...#...#......##....#.#........
...##..#..#.##....#...#........
#.#..#....#...#........#.......
..........#.......#..........#.
......##...#....###...#....#...
........#..#.....#......#......
....#.........##...#..##......#
....#...........#.#..#.#.#.#..#
..#......#..#......#........#.#
#..#....#.....#.............#..
............................#..
#...#.#.....#...#....#....#....
........#...#...#...#...#......
.###........#....#.##.....##.#.
.........#.....#..........#....
.#.........#....##.#.....#.....
#..#....................##.#...
..##.#.............#....#.#....
..#.#........#............##.#.
#........#...##.....#...#.....#
.........#.#..........#....#..#
...###.#..#.#......#.#.....#...
......#.....#..#...#........#..
.......#...#.....#....#....#..#
.#.#........#......##.......#..
#.................###..........
#........#.#..#....#..#........
..##....#.#...##...#...##....#.
...#.#......##...#.....#..#....
#..#........#...###....#.......
##.#....#..#.#..........#......
....#...###...#.....#........#.
..#.#........#....##.#.........
......##.##.................##.
.#....##...#.#..#.#............
.#...###........#......#.......
##..#.#......#..#..#...#.......
.......##..#....#........#....#
......#..........#.............
....##..##..#......#.#.........
.....#....................##...
...###.....#.....#...#.#.##.#..
....#.#..#.......#..#......##..
.......#.#..#.##.#...#......#..
...#.#....#.#...#..##...#...#..
#.##...#....#..#.............#.
...#...#...#.......#..........#
.#..#.............#..##.#......
....#.......#..............#.#.
..................#..#.....##.#
.#...#..#......#..........#...#
..#.#.....#..#....#....#####.#.
.......###.......#....#....#...
......#.#........#...#.........
......#..#.#.#....#.#.#....##..
.#...#.#...##.#......#.........
#....#..##....#.#.......#....#.
..##.#.....#.....#.........#...
......#......#....#....#.....#.
...##.....#....#......#......#.
......#......##............#.#.
.##.#.......#....#...#....#....
....#..#..#...##.......#..#....
....#....#...#.#........#..#...
....#.....#..........#..#......
....#....#...#.....#..##.....#.
##...#..##......#....##..#..#..
.....##.##..............##.....
#.#....#.##..#....#...##.......
..#.....##.....#.....######...#
...#.....#.#.#......#......##.#
...........##.............#....
...##......#..#......#...#.....
....#.##......#..#....#.#..#...
.#..#....#...#..#.....##.......
.....#..#.................#..#.
................#..#...#......#
...##....#.....#..#....##......
....##...............##...#....
......#..........#..##.........
.......###.......#.........#..#
......................#....#.#.
#.#.....#...##............#....
........#......##......#.....#.
...#....#....#.#..##.#..#.#.#..
..#.#....#.##...#..#.....#.#...
............#....#..#.......#..
#...#...#.#......#...##.....#.#
......#....#....#.......#......
....#.......#..........#....#..
........#####........#....#....
......#....##..............#.#.
....#....#.......#.......#.....
.##.#....##....#...............
#.....##........#..#.#...#.#...
...#......##....#..............
.#.....#.....#.......##....##..
#....#..........#.#..#.........
......##..........##.......#...
.##......##.....#.#....#......#
....#......................#...
.#...........###........#...#..
#.#..#..#..#...##.#....#.#..#..
...##...........#.#..........#.
......#.#..#....#....#.........
....#....#.#......#.........##.
.#..#...#...##....#...#......#.
#.#......#...#.#.#...........#.
##.....#..........##....##..##.
...#.#.....#..##........#......
..#........##........#..#......
.......#...............##..#...
.......#.#....#..###...........
.............#........#...#....
#.................#......#..#..
...#....#..#..............#...#
.............#....##....#..##..
#........#..........##...##...#
............#....#.....#.#....#
.....#..............##..#...#..
..#....#......###....#.#...##..
....##......#.....#....#.......
.....#...............#.....#...
.#.....#.....#..............#..
#................#..#..........
.##....#....#.....#............
#.####...#..#..#....#..........
..##........##.....##......#..#
......#.....#...##.........##..
....##..#.....#.#.........#...#
.....##..#....#....#.#...#..#..
...#............#...........#..
.......#.#..#.#.#..#........#.#
....#.#........#.#.#..#...#...#
..#....#....#..#......#........
.#...........................#.
.#..#....####........##......#.
.#.....#..#.#.................#
.#..#...........#...#....#...#.
......##..#........#..#....#...
..#.............#....#........#
#.#..........#.##.......#.#..#.
..#....#...#...............#...
..............#..........#..#..
..#.....#.#.....#...#...#..#...
.........#...###.#...#........#";
            }

            public string RetrieveSampleOutput()
            {
                return @"
191
1478615040
";
            }
        }
    }
}
