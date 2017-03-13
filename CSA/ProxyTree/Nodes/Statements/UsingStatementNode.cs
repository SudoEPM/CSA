using System.Diagnostics;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class UsingStatementNode : StatementNode
    {
        public UsingStatementNode(SyntaxNode origin) : base(origin)
        {
            var stmt = Origin as UsingStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            Declaration = stmt.Declaration.ToString();
        }

        public override string ToString()
        {
            return $"using ({Declaration})";
        }

        public string Declaration { get; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}