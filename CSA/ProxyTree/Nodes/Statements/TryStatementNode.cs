using System.Collections.Generic;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class TryStatementNode : StatementNode
    {
        public TryStatementNode(SyntaxNode origin) : base(origin)
        {
            Catchs = new HashSet<CatchStatementNode>();
            Finally = null;
        }

        public override string ToString()
        {
            return "Try";
        }

        public HashSet<CatchStatementNode> Catchs { get; }
        public FinallyStatementNode Finally { get; set; }
        public StatementNode Content { get; set; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}