using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes
{
    public class ExpressionNode : BasicProxyNode
    {
        public ExpressionNode(SyntaxNode origin) : base(origin)
        {
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}