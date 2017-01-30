using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSA.ProxyTree.Nodes
{
    public class SyntaxProxyNode
    {
        private readonly SyntaxNode _origin;

        public SyntaxProxyNode Parent { get; set; }
        public List<SyntaxProxyNode> Childs { get; }

        public SyntaxKind Kind => _origin.Kind();

        public SyntaxProxyNode(SyntaxNode origin)
        {
            _origin = origin;
            Childs = new List<SyntaxProxyNode>();
        }
    }
}
