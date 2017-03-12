using System.Diagnostics;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class GotoCaseStatementNode : StatementNode
    {
        public GotoCaseStatementNode(SyntaxNode origin) : base(origin)
        {
            var stmt = Origin as GotoStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            Case = stmt.CaseOrDefaultKeyword.ToString() == "case"
                ? $"case {stmt.Expression}:"
                : "default:";
        }

        public string Case { get; }

        public SwitchSectionStatementNode Target { get; set; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}