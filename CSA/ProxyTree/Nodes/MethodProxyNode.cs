using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes
{
    class MethodProxyNode : BasicProxyNode
    {
        public MethodProxyNode(SyntaxNode origin) : base(origin)
        {
        }
    }
}