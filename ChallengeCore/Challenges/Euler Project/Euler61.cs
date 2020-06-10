using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Euler Project", "Prob 61", "https://projecteuler.net/problem=61")]
        // ReSharper disable once UnusedMember.Global
        public class Euler61 : IChallenge
        {
            public void Solve()
            {
                var figNodes = new List<Node>[Fns.Count()];

                for (var ifig = 0; ifig < Fns.Count(); ifig++)
                {
                    figNodes[ifig] = GetFigs(Fns[ifig]).Select(i => new Node(i, ifig + 3)).ToList();
                }

                for (var ifig = 0; ifig < Fns.Count(); ifig++)
                {
                    for (var jfig = 0; jfig < Fns.Count(); jfig++)
                    {
                        if (jfig == ifig)
                        {
                            continue;
                        }

                        foreach (var inode in figNodes[ifig])
                        {
                            inode.Incorporate(figNodes[jfig]);
                        }
                    }
                }

                foreach (var node in figNodes[5])
                {
                    List<Node> path;
                    if ((path = node.FindCycle()) != null)
                    {
                        WriteLine(path.Select(n => n.Figurate).Sum());
                    }
                }
            }

            public string RetrieveSampleInput() => null;

            public string RetrieveSampleOutput() => @"
28684
";

            struct Figurate
            {
                public readonly Func<int, int> ItoF;
                public readonly Func<int, double> FtoI;

                public Figurate(Func<int, int> itof, Func<int, double> ftoi)
                {
                    ItoF = itof;
                    FtoI = ftoi;
                }
            }

            class Node
            {
                private readonly int _firstDigs;
                private readonly int _lastDigs;
                private readonly int _type;
                private readonly List<Node> _toNodes = new List<Node>();
                private readonly List<Node> _fromNodes = new List<Node>();

                // ReSharper disable MemberHidesStaticFromOuterClass
                internal int Figurate { get; private set; }
                // ReSharper restore MemberHidesStaticFromOuterClass

                public Node(int figurate, int type)
                {
                    Figurate = figurate;
                    _type = type;
                    _firstDigs = figurate / 100;
                    _lastDigs = figurate % 100;
                }

                private bool PointsTo(Node node)
                {
                    return _lastDigs == node._firstDigs;
                }

                private bool PointedFrom(Node node)
                {
                    return node.PointsTo(this);
                }

                public void Incorporate(IEnumerable<Node> nodes)
                {
                    foreach (var node in nodes)
                    {
                        if (PointsTo(node))
                        {
                            _toNodes.Add(node);
                        }

                        if (PointedFrom(node))
                        {
                            _fromNodes.Add(node);
                        }
                    }
                }

                public List<Node> FindCycle()
                {
                    return FindCycle(new List<Node>());
                }

                private List<Node> FindCycle(IEnumerable<Node> path)
                {
                    var myPath = new List<Node>(path) {this};
                    if (myPath.Count == 6)
                    {
                        return PointsTo(myPath[0]) ? myPath : null;
                    }

                    // ReSharper disable LoopCanBeConvertedToQuery
                    foreach (var node in _toNodes)
                    {
                        if (myPath.All(n => n._type != node._type))
                        {
                            var trialPath = node.FindCycle(myPath);
                            if (trialPath != null)
                            {
                                return trialPath;
                            }
                        }
                    }
                    // ReSharper restore LoopCanBeConvertedToQuery

                    return null;
                }
            }

            // Functions to go from indices to figurate numbers and back again
            private static readonly Figurate[] Fns =
            {
                new Figurate(
                    i => i * (i + 1) / 2,
                    n => (Math.Sqrt(8 * n + 1) - 1) / 2),
                new Figurate(
                    i => i * i,
                    n => Math.Sqrt(n)),
                new Figurate(
                    i => i * (3 * i - 1) / 2,
                    n => (1 + Math.Sqrt(1 + 24 * n)) / 6),
                new Figurate(
                    i => i * (2 * i - 1),
                    n => (1 + Math.Sqrt(1 + 8 * n)) / 4),
                new Figurate(
                    i => i * (5 * i - 3) / 2,
                    n => (3 + Math.Sqrt(40 * n + 9)) / 10),
                new Figurate(
                    i => i * (3 * i - 2),
                    n => (2 + Math.Sqrt(12 * n + 4)) / 6)
            };


            private static IEnumerable<int> GetFigs(Figurate fig)
            {
                var lb = (int) fig.FtoI(1000);
                var ub = (int) fig.FtoI(10000) + 1;
                return Enumerable.Range(lb, ub - lb + 1).Select(i => fig.ItoF(i)).Where(n => 1000 <= n && n <= 10000)
                    .ToList();
            }
        }
    }
}
