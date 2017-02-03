using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Nodes;
using Ninject;

namespace CSA.ProxyTree.Iterators
{
    class PostOrderDepthFirstProxyIterator : IProxyIterator
    {
        private readonly Stack<IProxyNode> _stack;
        private readonly HashSet<IProxyNode> _visited; 

        public PostOrderDepthFirstProxyIterator(Stack<IProxyNode> stack, [Named("Root")] IProxyNode forest, HashSet<IProxyNode> visited)
        {
            _stack = stack;
            _visited = visited;
            _stack.Clear(); // Just to be sure that ninject didn't do something fishy
            _visited.Clear();
            _stack.Push(forest);
        }

        public IEnumerable<IProxyNode> GetEnumerable()
        {
            while (_stack.Any())
            {
                // Find the current element
                var current = _stack.Peek();
                // Find the next elements
                var notVisitedChilds = current.Childs.Where(x => !_visited.Contains(x)).ToList();
                if (notVisitedChilds.Any())
                {
                    notVisitedChilds.ForEach(x => _stack.Push(x));
                }
                else
                {
                    // Return the current if all childrens have been visited
                    yield return current;
                    _visited.Add(current);
                    _stack.Pop();
                }
            }
        }
    }
}