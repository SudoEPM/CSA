using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes
{
    public class ExpressionNode : BasicProxyNode
    {
        public ExpressionNode(SyntaxNode origin) : base(origin)
        {
        }
    }
}