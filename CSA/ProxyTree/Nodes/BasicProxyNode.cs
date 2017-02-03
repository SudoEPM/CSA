using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSA.ProxyTree.Nodes
{
    public class BasicProxyNode : IProxyNode
    {
        private readonly SyntaxNode _origin;

        public IProxyNode Parent { get; set; }
        public List<IProxyNode> Childs { get; }

        public SyntaxKind Kind => _origin.Kind();

        public BasicProxyNode(SyntaxNode origin)
        {
            _origin = origin;
            Childs = new List<IProxyNode>();
        }
    }
}
