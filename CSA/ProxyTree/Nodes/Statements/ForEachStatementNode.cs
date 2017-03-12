using System.Diagnostics;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class ForEachStatementNode : StatementNode
    {
        public ForEachStatementNode(SyntaxNode origin) : base(origin)
        {
        }

        public override string ToString()
        {
            var stmt = Origin as ForEachStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            return $"foreach({stmt.Type} {stmt.Identifier} in {stmt.Expression})";
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}