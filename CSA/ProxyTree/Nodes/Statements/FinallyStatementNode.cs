using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class FinallyStatementNode : StatementNode
    {
        public FinallyStatementNode(SyntaxNode origin) : base(origin)
        {
        }

        public override string ToString()
        {
            return "finally";
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}