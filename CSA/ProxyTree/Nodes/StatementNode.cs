using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes
{
    public class StatementNode : BasicProxyNode
    {
        public StatementNode(SyntaxNode origin) : base(origin)
        {
            LineNumber = origin.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
        }

        public override string ToString()
        {
            return Origin.ToString();
        }

        public int LineNumber { get; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}