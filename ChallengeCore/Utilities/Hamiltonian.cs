using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using ChallengeCore.Challenges;
using MoreLinq;

namespace ChallengeCore.Utilities
{
    public class Hamiltonian
    {
        private List<IState> _nodesSorted;
        private int _cNodes;

		// Based on the paper here:
		//      https://www.hindawi.com/journals/jopti/2018/9328103/
		// It doesn't say it in the paper, but it seems that the Rotational Transformation
		// used is only valid for non-directed graphs so don't be using this on digraphs.
		// 
		public IState[] HybridHam(IEnumerable<IState> nodes)
        {
            var MaxValence = nodes.Select(n => n.SuccessorCount()).Max();
			_cNodes = _nodesSorted.Count;
			var maxValence = _nodesSorted[0].SuccessorCount();
            var pathT = (List<IState>)null;
            var nodesInPath = (HashSet<IState>)null;

            // Find an initial path
			foreach (var node in nodes.Where(n => n.SuccessorCount() == maxValence))
			{
				var (candidatePath, nodeSet) = CandidatePath(node);
				if (pathT == null || candidatePath.Count > pathT.Count)
				{
					pathT = candidatePath;
                    nodesInPath = nodeSet;
                }
            }

            return null;
		}

        private (List<IState> path, HashSet<IState> nodesInPath) CandidatePath(IState start)
        {
            var nodesInPath = new HashSet<IState>() {start};
            var path = new List<IState>(_cNodes) {start};

            // Attempt to extend the path
            return !ExtendPath(path, nodesInPath) ? (null, null) : (path, nodesInPath);
        }

        private bool ExtendPath(List<IState> path, HashSet<IState> nodesInPath)
        {
            var fEverExtended = false;
            while (true)
            {
                var fExtended = false;

                // Find an admissable extention
                var candidate = path[^1]
                    .Successors()
                    .OrderBy(n => n.SuccessorCount())
                    .FirstOrDefault(n => IsAdmissable(n, nodesInPath));

                // If we found one...
                if (candidate != null)
                {
                    //...add it to our path
                    path.Add(candidate);
                    nodesInPath.Add(candidate);
                    fExtended = true;
                    fEverExtended = true;
                }

                // If we're unable to extend then we've reached a dead end
                if (!fExtended)
                {
                    break;
                }
            }
            return fEverExtended;
        }

        private static bool IsAdmissable(IState candidate, HashSet<IState> nodesInPath)
        {
            foreach (var neighbor in candidate.Successors())
            {
                var reachableCount = neighbor.Successors().Count(s => !nodesInPath.Contains(s));
                if (reachableCount <= 1)
                {
                    return false;
                }
            }

            return true;
        }

        public class RotationTree
        {
            private List<int> _pivotPositions;
            private List<int> _parent;
            private List<IState> _originalPath;
            private HashSet<IState> _pathMembers;

            int FindPosition(int level, int positionInDerivedPath)
            {
                int positionFinal = 0;
                if (_pivotPositions[level] >= 0)
                {
                    positionFinal = FindPosition(_parent[level], positionInDerivedPath);
                    if (_pivotPositions[level] < positionFinal)
                    {
                        positionFinal = _originalPath.Count - positionFinal + _pivotPositions[level] + 1;
                    }
                }

                return positionFinal;
            }
        }
    }
}
