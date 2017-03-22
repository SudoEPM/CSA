using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class SwitchStatementNode : StatementNode
    {
        public SwitchStatementNode(SyntaxNode origin) : base(origin)
        {
            var stmt = Origin as SwitchStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            Expression = stmt.Expression.ToString();
        }

        public override void ComputeDefUse()
        {
            var stmt = Origin as SwitchStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");

            var results = Model.AnalyzeDataFlow(stmt.Expression);
            // Should be empty...
            VariablesDefined = results.WrittenInside.Select(x => x.Name).ToImmutableHashSet();
            VariablesUsed = results.ReadInside.Select(x => x.Name).ToImmutableHashSet();
        }

        public override string ToString()
        {
            return $"switch({Expression})";
        }

        public string Expression { get; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}