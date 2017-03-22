using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Ninject;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class ForStatementNode : StatementNode
    {
        public ForStatementNode(SyntaxNode origin) : base(origin, false)
        {
            var stmt = Origin as ForStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");

            Declaration = stmt.Declaration?.ToString() ?? stmt.Initializers.ToString();
            Condition = stmt.Condition.ToString();
            Incrementors = stmt.Incrementors.ToString();

            var defined = new HashSet<string>();
            var used = new HashSet<string>();

            var model = Program.Kernel.Get<SemanticModel>();
            DataFlowAnalysis results;
            
            if (stmt.Declaration != null)
            {
                foreach (var declared in stmt.Declaration.Variables)
                {
                    defined.Add(declared.Identifier.ToString());
                    results = model.AnalyzeDataFlow(declared.Initializer.Value);
                    used.UnionWith(results.ReadInside.Select(x => x.Name));
                }
            }
            else
            {
                foreach (var initialized in stmt.Initializers)
                {
                    results = model.AnalyzeDataFlow(initialized);
                    defined.UnionWith(results.WrittenInside.Select(x => x.Name));
                    used.UnionWith(results.ReadInside.Select(x => x.Name));
                }
            }

            results = model.AnalyzeDataFlow(stmt.Condition);
            defined.UnionWith(results.WrittenInside.Select(x => x.Name));
            used.UnionWith(results.ReadInside.Select(x => x.Name));

            foreach (var incrementor in stmt.Incrementors)
            {
                results = model.AnalyzeDataFlow(incrementor);
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