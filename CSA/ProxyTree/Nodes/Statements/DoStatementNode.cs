using System.Diagnostics;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class DoStatementNode : StatementNode
    {
        public DoStatementNode(SyntaxNode origin) : base(origin)
        {
        }

        public override string ToString()
        {
            var stmt = Origin as DoStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            return $"do while({stmt.Condition.ToString()})";
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}