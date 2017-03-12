using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class ContinueStatementNode : StatementNode
    {
        public ContinueStatementNode(SyntaxNode origin) : base(origin)
        {
        }

        public StatementNode Link { get; set; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}