using System.Diagnostics;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class ForStatementNode : StatementNode
    {
        public ForStatementNode(SyntaxNode origin) : base(origin)
        {
        }

        public override string ToString()
        {
            var stmt = Origin as ForStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            return $"for({stmt.Declaration}; {stmt.Condition}; {stmt.Incrementors})";
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}