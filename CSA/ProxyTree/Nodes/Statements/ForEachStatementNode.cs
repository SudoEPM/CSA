using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class ForEachStatementNode : StatementNode
    {
        public ForEachStatementNode(SyntaxNode origin) : base(origin)
        {
            var stmt = Origin as ForEachStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            Type = stmt.Type.ToString();
            Identifier = stmt.Identifier.ToString();
            Expression = stmt.Expression.ToString();
        }

        public override void ComputeDefUse()
        {
            var stmt = Origin as ForEachStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");

            // Analyze data flow
            var results = Model.AnalyzeDataFlow(stmt.Expression);
            VariablesDefined = new HashSet<string> { Identifier }.ToImmutableHashSet();
            VariablesUsed = results.ReadInside.Select(x => x.Name).ToImmutableHashSet();
        }

        public override string ToString()
        {
            return $"foreach({Type} {Identifier} in {Expression})";
        }

        public string Type { get; }

        public string Identifier { get; }

        public string Expression { get; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}