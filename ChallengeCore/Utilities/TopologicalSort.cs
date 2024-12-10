using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

// ReSharper disable IdentifierTypo

namespace ChallengeCore.Utilities
{
    internal class TopologicalSort
    {
        private Dictionary<int, TopoSortNode> _mapValueToNode = new Dictionary<int, TopoSortNode>();
        private Stack<int> _valueStack { get; } = new Stack<int>();

        public IEnumerable<int> SortedValues => _valueStack;

        /// <summary>
        /// Does a topological sort based on integers in the orderings
        /// </summary>
        ///
        /// <remarks>
        /// Each tuple (a, b) in orderings indicates that a should come before b in the topological sort.  These orderings
        /// need to describe a DAG - any cycles will result in an exception.  SortedValues will retrieve an IEnumerable of
        /// the values in an order such that all the orderings are satisfied.
        ///
        /// Based on the "Depth-first search" section here:
        /// https://en.wikipedia.org/wiki/Topological_sorting
        /// </remarks>
        /// 
        /// <param name="orderings">DAG orderings of the topological sort</param>
        public TopologicalSort(IEnumerable<(int, int)> orderings)
        {
            foreach (var ordering in orderings)
            {
                var nodeHigh = RegisterNode(ordering.Item1);
                var nodeLow = RegisterNode(ordering.Item2);

                nodeHigh.Children.Add(nodeLow.Value);
            }

            while (true)
            {
                var next = _mapValueToNode.Values.FirstOrDefault(t => !t.FPermanentMark);
                if (next == null)
                {
                    break;
                }

                Visit(next);
            }
        }

        private void Visit(TopoSortNode curNode)
        {
            if (curNode.FPermanentMark)
            {
                return;
            }

            if (curNode.FTemporaryMark)
            {
                throw new InvalidOperationException("Cycle in TopologicalSort");
            }

            curNode.FTemporaryMark = true;

            foreach (var child in curNode.Children)
            {
                Visit(_mapValueToNode[child]);
            }

            curNode.FPermanentMark = true;
            _valueStack.Push(curNode.Value);
        }

        private TopoSortNode RegisterNode(int value)
        {
            if (_mapValueToNode.TryGetValue(value, out var node))
            {
                return node;
            }
            var ret = new TopoSortNode(value);
            _mapValueToNode[value] = ret;
            return ret;
        }

        private class TopoSortNode
        {
            public int Value { get; }

            public List<int> Children { get; } = new List<int>();
            public bool FTemporaryMark { get; set; } = false;
            public bool FPermanentMark { get; set; } = false;

            internal TopoSortNode(int value)
            {
                Value = value;
            }
        }
    }
}
