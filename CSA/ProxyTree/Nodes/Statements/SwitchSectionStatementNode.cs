using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class SwitchSectionStatementNode : StatementNode
    {
        public SwitchSectionStatementNode(SyntaxNode origin) : base(origin)
        {
            var stmt = Origin as SwitchSectionSyntax;
            Debug.Assert(stmt != null, "stmt != null");

            Labels = stmt.Labels.Select(x => x.ToString()).ToList();
        }

        public override void ComputeDefUse()
        {
            VariablesDefined = ImmutableHashSet<string>.Empty;
            VariablesUsed = ImmutableHashSet<string>.Empty;
        }

        public List<string> Labels { get; }

        public override string ToString()
        {
            var str = "";
            foreach (var label in Labels)
            {
                str += $"{label}\n";
            }
            return str;
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}