using System;
using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Nodes.Interfaces;
using Ninject;

namespace CSA.ProxyTree.Iterators
{
    class PostOrderDepthFirstProxyIterator : IProxyIterator
    {
        private readonly Stack<IProxyNode> _stack;
        private readonly HashSet<IProxyNode> _visited;

        public PostOrderDepthFirstProxyIterator([Named("Root")] IProxyNode root, bool skipStatements = false)
        {
            _stack = new Stack<IProxyNode>();
            _visited = new HashSet<IProxyNode>();
            Root = root;
            SkipStatements = skipStatements;
        }

        public IProxyNode Root { get; }

        public bool SkipStatements { get; set; }

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
                    var current = _stack.Peek();
                    // Find the next elements
                    var notVisitedChilds = current.Childs.Where(Accept).ToList();
                    if (notVisitedChilds.Any())
                    {
                        notVisitedChilds.ForEach(x => _stack.Push(x));
                    }
                    else
                    {
                        if (_visited.Contains(current))
                        {
                            _stack.Pop();
                            continue;
                        }

                        // Return the current if all childrens have been visited
                        yield return current;
                        _visited.Add(current);
                        _stack.Pop();
                    }
                }
            }
        }

    }
}