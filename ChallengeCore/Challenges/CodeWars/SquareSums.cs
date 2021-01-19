using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Microsoft.FSharp.Linq.RuntimeHelpers;
using NumberTheoryBig;
using static System.Console;


namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Code Wars", "Square Sums", "https://www.codewars.com/kata/5a667236145c462103000091")]
        // ReSharper disable once UnusedMember.Global
        public class SquareSums : IChallenge
        {
            private const int NCases = 1350;
            private static readonly BitArray PerfectSquares = new BitArray(2 * NCases - 1);
            private readonly BitArray _leafPossible = new BitArray(NCases);

            public void Solve()
            {
                for (var i = 0; i < (int)Math.Floor(Math.Sqrt(NCases * 2 - 1)); i++)
                {
                    PerfectSquares[i * i] = true;
                }

                var leaves = new HashSet<int>(){1, 2, 4, 5, 6, 7};

                // We initialize to n = 7 which is the first value where everything has at least valence 1.
                // Thus we start with n = 8.
                for (var n = 8; n < NCases; n++)
                {
                    var cSq = 0;

                    var leavesToRemove = new int[leaves.Count];
                    foreach (var leaf in leaves)
                    {
                        if (IsPS(n + leaf))
                        {
                            leavesToRemove[cSq++] = leaf;
                        }
                    }

                    for (var i = 0; i < cSq; i++)
                    {
                        leaves.Remove(leavesToRemove[i]);
                    }

                    if (ReachableSquareCount(n) == 1)
                    {
                        leaves.Add(n);
                    }

                    Debug.Write($"n = {n}:");
                    foreach (var leaf in leaves)
                    {
                        Debug.Write($" {leaf}");
                    }

                    Debug.WriteLine("");
                }
            }

            // ReSharper disable once UnusedMember.Local
            private static bool IsPS(int n)
            {
                return PerfectSquares[n];
            }

            private static int ReachableSquareCount(int n) =>
                (int)(Math.Floor(Math.Sqrt(2 * n - 1)) - Math.Floor(Math.Sqrt(n)));

            private static bool DoublePS(int n)
            {
                return (n & 1) == 0 && IsPS(n / 2);
            }

            public string RetrieveSampleInput()
            {
                return null;
            }

            public string RetrieveSampleOutput()
            {
                return null;
            }
        }
    }
}