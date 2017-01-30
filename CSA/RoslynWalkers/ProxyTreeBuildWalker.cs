using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree;
using CSA.ProxyTree.Nodes;
using Microsoft.CodeAnalysis;

namespace CSA.RoslynWalkers
{
    internal class ProxyTreeBuildWalker : SyntaxWalker
    {
        public SyntaxProxyNode Root { get; private set; }

        private readonly Dictionary<SyntaxNode, SyntaxProxyNode> _mapNodesConversion = new Dictionary<SyntaxNode, SyntaxProxyNode>(); 

        public override void Visit(SyntaxNode node)
        {
            var test = node.Ancestors();
            var curr = new SyntaxProxyNode(node);
            if (!node.Ancestors().Any())
            {
                Root = curr;
            }
            else
            {
                var parent = _mapNodesConversion[node.Parent];
                curr.Parent = parent;
                parent.Childs.Add(curr);
            }

            _mapNodesConversion[node] = curr; 
            base.Visit(node);
        }

    }
}