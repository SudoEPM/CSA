using System.Diagnostics;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class SwitchStatementNode : StatementNode
    {
        public SwitchStatementNode(SyntaxNode origin) : base(origin)
        {
        }

        public override string ToString()
        {
            var stmt = Origin as SwitchStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            return $"switch({stmt.Expression})";
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}