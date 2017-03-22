using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class ForStatementNode : StatementNode
    {
        public ForStatementNode(SyntaxNode origin) : base(origin)
        {
            var stmt = Origin as ForStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");

            Declaration = stmt.Declaration?.ToString() ?? stmt.Initializers.ToString();
            Condition = stmt.Condition.ToString();
            Incrementors = stmt.Incrementors.ToString();
        }

        public override void ComputeDefUse()
        {
            var stmt = Origin as ForStatementSyntax;
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
                foreach (var initialized in stmt.Initializers)
                {
                    results = Model.AnalyzeDataFlow(initialized);
                    defined.UnionWith(results.WrittenInside.Select(x => x.Name));
                    used.UnionWith(results.ReadInside.Select(x => x.Name));
                }
            }

            results = Model.AnalyzeDataFlow(stmt.Condition);
            defined.UnionWith(results.WrittenInside.Select(x => x.Name));
            used.UnionWith(results.ReadInside.Select(x => x.Name));

            foreach (var incrementor in stmt.Incrementors)
            {
                results = Model.AnalyzeDataFlow(incrementor);
                defined.UnionWith(results.WrittenInside.Select(x => x.Name));
                used.UnionWith(results.ReadInside.Select(x => x.Name));
            }

            VariablesDefined = defined.ToImmutableHashSet();
            VariablesUsed = used.ToImmutableHashSet();
        }

        public override string ToString()
        {
            var stmt = Origin as ForStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            return $"for({stmt.Declaration}; {stmt.Condition}; {stmt.Incrementors})";
        }

        public string Declaration { get; }

        public string Condition { get; }

        public string Incrementors { get; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}