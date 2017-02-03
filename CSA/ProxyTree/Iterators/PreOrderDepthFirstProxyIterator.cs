using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Nodes;
using Ninject;

namespace CSA.ProxyTree.Iterators
{
    class PreOrderDepthFirstProxyIterator : IProxyIterator
    {
        private readonly Stack<IProxyNode> _stack;

        public PreOrderDepthFirstProxyIterator(Stack<IProxyNode> stack, [Named("Root")] IProxyNode forest)
        {
            _stack = stack;
            _stack.Clear(); // Just to be sure that ninject didn't do something fishy
            _stack.Push(forest);
        }

        public IEnumerable<IProxyNode> GetEnumerable()
        {
            while (_stack.Any())
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