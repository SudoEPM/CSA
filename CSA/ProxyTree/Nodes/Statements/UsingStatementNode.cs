using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
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
            Expression = stmt.Expression?.ToString() ?? stmt.Declaration.ToString();
        }

        public override void ComputeDefUse()
        {
            var stmt = Origin as UsingStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");

            var defined = new HashSet<string>();
            var used = new HashSet<string>();

            DataFlowAnalysis results;

            if (stmt.Declaration != null)
            {
                foreach (var declared in stmt.Declaration.Variables)
                {
                    defined.Add(declared.Identifier.ToString());
                    results = Model.AnalyzeDataFlow(declared.Initializer.Value);
                    used.UnionWith(results.ReadInside.Select(x => x.Name));
                }
            }
            else
            {
                results = Model.AnalyzeDataFlow(stmt.Expression);
                defined.UnionWith(results.WrittenInside.Select(x => x.Name));
                used.UnionWith(results.ReadInside.Select(x => x.Name));
            }

            VariablesDefined = defined.ToImmutableHashSet();
            VariablesUsed = used.ToImmutableHashSet();
        }

        public override string ToString()
        {
            return $"using ({Expression})";
        }

        public string Expression { get; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}