using System.Diagnostics;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class LabelStatementNode : StatementNode
    {
        public LabelStatementNode(SyntaxNode origin) : base(origin)
        {
            var stmt = Origin as LabeledStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            Label = stmt.Identifier.Text;
        }

        public override string ToString()
        {
            return $"{Label}:";
        }

        public string Label { get; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}