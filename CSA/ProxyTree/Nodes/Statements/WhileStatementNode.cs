using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Ninject;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class WhileStatementNode : StatementNode
    {
        public WhileStatementNode(SyntaxNode origin) : base(origin, false)
        {
            var stmt = Origin as WhileStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            Condition = stmt.Condition.ToString();

            // Analyze data flow
            var model = Program.Kernel.Get<SemanticModel>();
            var results = model.AnalyzeDataFlow(stmt.Condition);
            VariablesDefined = results.WrittenInside.Select(x => x.Name).ToImmutableHashSet();
            VariablesUsed = results.ReadInside.Select(x => x.Name).ToImmutableHashSet();
        }

        public string Condition { get; }

        public override string ToString()
        {
            return $"while({Condition})";
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}