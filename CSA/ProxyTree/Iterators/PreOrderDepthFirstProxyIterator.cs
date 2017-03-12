using System;
using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Nodes.Interfaces;
using Ninject;

namespace CSA.ProxyTree.Iterators
{
    class PreOrderDepthFirstProxyIterator : IProxyIterator
    {
        public IProxyNode Root { get; }
        private readonly Stack<IProxyNode> _stack;
        private readonly HashSet<IProxyNode> _visited;
        public bool SkipStatements { get; set; }

        public PreOrderDepthFirstProxyIterator([Named("Root")] IProxyNode forest, bool skipStatements = false)
        {
            _stack = new Stack<IProxyNode>();
            _visited = new HashSet<IProxyNode>();
            Root = forest;
            SkipStatements = skipStatements;
        }

        private bool Accept(IProxyNode node)
        {
            if (SkipStatements && node.Parent is ICallableNode)
            {
                return false;
            }

            return !_visited.Contains(node);
        }

        public IEnumerable<IProxyNode> Enumerable
        {
            get
            {
                _stack.Clear();
                _visited.Clear();
                _stack.Push(Root);

                while (_stack.Any())
                {
                    // Find the current element
                    var current = _stack.Pop();

                    // Find the next elements
                    foreach (var x in current.Childs.Where(Accept))
                    {
                        _stack.Push(x);
                        _visited.Add(x);
                    }
                    // Return the current
                    yield return current;
                }
            }
        }
    }
}