using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes
{
    public class StatementNode : BasicProxyNode
    {
        public StatementNode(SyntaxNode origin) : base(origin)
        {
        }

        public override string ToString()
        {
            return Origin.ToString();
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}