using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes
{
    class ExpressionNode : BasicProxyNode
    {
        public ExpressionNode(SyntaxNode origin) : base(origin)
        {
        }
    }
}