using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Ninject;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class DoStatementNode : StatementNode
    {
        public DoStatementNode(SyntaxNode origin) : base(origin)
        {
            var stmt = Origin as DoStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            Condition = stmt.Condition.ToString();
        }

        public override void ComputeDefUse()
        {
            var stmt = Origin as DoStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");

            // Analyze data flow
            var results = Model.AnalyzeDataFlow(stmt.Condition);
            VariablesDefined = ImmutableHashSet<string>.Empty;
            VariablesUsed = results.ReadInside.Select(x => x.Name).ToImmutableHashSet();
        }

        public override string ToString()
        {
            return $"do while({Condition})";
        }

        public string Condition { get; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}