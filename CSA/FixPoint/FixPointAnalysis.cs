using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CSA.FixPoint
{
    abstract class FixPointAnalysis
    {
        private Dictionary<IFixPointAnalyzableNode, FixPointResult> _fixPointValues;

        protected enum MeetOperatorType
        {
            Union,
            Intersection
        };

        protected abstract MeetOperatorType MeetOperator { get; }
        protected abstract bool IsForward { get; }
        protected abstract bool RecomputeGenKill { get; }

        protected virtual void Init(IEnumerable<IFixPointAnalyzableNode> nodes) {}
        protected abstract FixPointResult Init(IFixPointAnalyzableNode node);
        protected abstract ImmutableHashSet<string> TransferFunction(IFixPointAnalyzableNode node, FixPointResult data);

        protected virtual ImmutableHashSet<string> ComputeGen(IFixPointAnalyzableNode node, FixPointResult data) => ImmutableHashSet<string>.Empty;
        protected virtual ImmutableHashSet<string> ComputeKill(IFixPointAnalyzableNode node, FixPointResult data) => ImmutableHashSet<string>.Empty;

        private ImmutableHashSet<string> GetBeforeSet(IFixPointAnalyzableNode node)
        {
            return IsForward ? _fixPointValues[node].Out : _fixPointValues[node].In;
        }

        public Dictionary<IFixPointAnalyzableNode, FixPointResult> Execute(IEnumerable<IFixPointAnalyzableNode> nodesToAnalyze)
        {
            var nodesSet = nodesToAnalyze.ToImmutableHashSet();
            Init(nodesSet);

            _fixPointValues = new Dictionary<IFixPointAnalyzableNode, FixPointResult>();
            foreach (var node in nodesSet)
            {
                _fixPointValues[node] = Init(node);
            }

            // Initialize the data structures used by the fix-point
            var workList = new Queue<IFixPointAnalyzableNode>(nodesSet);
            var workSet = new HashSet<IFixPointAnalyzableNode>(nodesSet);

            while (workList.Any())
            {
                var current = workList.Dequeue();
                workSet.Remove(current);

                var data = _fixPointValues[current];

                if (RecomputeGenKill)
                {
                    data.Gen = ComputeGen(current, data);
                    data.Kill = ComputeKill(current, data);
                }

                var beforeNodes = IsForward ? current.Prec().ToImmutableHashSet() : current.Succ().ToImmutableHashSet();
                var beforeSets = beforeNodes.Select(GetBeforeSet).ToImmutableHashSet();

                ImmutableHashSet<string> before;
                if (MeetOperator == MeetOperatorType.Union)
                {
                    before = beforeSets.SelectMany(x => x).ToImmutableHashSet();
                }
                else
                {
                    before = beforeSets
                        .Skip(1)
                        .Aggregate(
                            new HashSet<string>(beforeSets.First()),
                            (h, e) => { h.IntersectWith(e); return h; }
                        ).ToImmutableHashSet();
                }

                ImmutableHashSet<string> oldAfter;
                ImmutableHashSet<string> newAfter;
                if (IsForward)
                {
                    data.In = before;
                    oldAfter = data.Out;
                    data.Out = newAfter = TransferFunction(current, data);
                }
                else
                {
                    data.Out = before;
                    oldAfter = data.In;
                    data.In = newAfter = TransferFunction(current, data);
                }

                if (!oldAfter.SetEquals(newAfter))
                {
                    var afterNodes = IsForward ? current.Succ().ToImmutableHashSet() : current.Prec().ToImmutableHashSet();
                    foreach (var next in afterNodes)
                    {
                        if (!workSet.Contains(next))
                        {
                            workList.Enqueue(next);
                            workSet.Add(next);
                        }
                    }
                }
            }

            return _fixPointValues;
        }
    }
}