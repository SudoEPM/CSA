using System.Collections.Generic;
using System.Linq;
using CSA.CFG.Nodes;

namespace CSA.CFG.Iterators
{
    class PreOrderDepthFirstCfgIterator : ICfgIterator
    {
        private readonly Stack<CfgNode> _stack;

        public PreOrderDepthFirstCfgIterator(CfgNode root)
        {
            _stack = new Stack<CfgNode>();
            _stack.Push(root);
        }

        private bool Accept(CfgNode node) => true;

        public IEnumerable<CfgNode> GetEnumerable()
        {
            while (_stack.Any())
            {
                // Find the current element
                var current = _stack.Pop();
                // Find the next elements
                current.Next.Where(Accept).ToList().ForEach(x => _stack.Push(x));
                // Return the current
                yield return current;
            }
        }
    }
}