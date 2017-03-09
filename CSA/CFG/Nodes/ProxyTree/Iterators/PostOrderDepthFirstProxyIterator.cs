using System;
using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using Ninject;

namespace CSA.ProxyTree.Iterators
{
    class PostOrderDepthFirstProxyIterator : IProxyIterator
    {
        private readonly Stack<IProxyNode> _stack;
        private readonly HashSet<IProxyNode> _visited; 

        public PostOrderDepthFirstProxyIterator(Stack<IProxyNode> stack, [Named("Root")] IProxyNode forest, HashSet<IProxyNode> visited, HashSet<Type> nodesToSkip)
        {
            _stack = stack;
            _visited = visited;
            NodesToSkip = nodesToSkip;
            _stack.Clear(); // Just to be sure that ninject didn't do something fishy
            _visited.Clear();
            _stack.Push(forest);
        }

        public HashSet<Type> NodesToSkip { get; }

        private bool Accept(IProxyNode node)
        {
            return !_visited.Contains(node) && !NodesToSkip.Contains(node.GetType());
        }

        public IEnumerable<IProxyNode> GetEnumerable()
        {
            while (_stack.Any())
            {
                // Find the current element
                var current = _stack.Peek();
                // Find the next elements
                var notVisitedChilds = current.Childs.Where(Accept).ToList();
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