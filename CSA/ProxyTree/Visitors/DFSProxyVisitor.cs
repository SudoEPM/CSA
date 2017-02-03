using System.Collections.Generic;
using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Nodes;

namespace CSA.ProxyTree.Visitors
{
    class DFSProxyVisitor : IProxyVisitor
    {
        private readonly Stack<IProxyNode> _stack;

        public DFSProxyVisitor(Stack<IProxyNode> stack, List<IProxyNode> forest)
        {
            _stack = stack;
            forest.ForEach(x => _stack.Push(x));
        }

        public IEnumerable<IProxyNode> GetEnumerable()
        {
            while (_stack.Count > 0)
            {
                // Find the current element
                var current = _stack.Pop();
                // Find the next elements
                current.Childs.ForEach(x => _stack.Push(x));
                // Return the current
                yield return current;
            }
        }
    }
}