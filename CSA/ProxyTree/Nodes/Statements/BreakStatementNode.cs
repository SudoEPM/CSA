using System.Collections.Immutable;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class BreakStatementNode : StatementNode
    {
        public BreakStatementNode(SyntaxNode origin) : base(origin)
        {
        }

        public override void ComputeDefUse()
        {
            VariablesDefined = ImmutableHashSet<string>.Empty;
            VariablesUsed = ImmutableHashSet<string>.Empty;
        }

        public StatementNode Link { get; set; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}