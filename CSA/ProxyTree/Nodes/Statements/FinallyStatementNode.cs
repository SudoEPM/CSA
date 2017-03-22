using System.Collections.Immutable;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class FinallyStatementNode : StatementNode
    {
        public FinallyStatementNode(SyntaxNode origin) : base(origin)
        {
        }

        public override void ComputeDefUse()
        {
            VariablesDefined = ImmutableHashSet<string>.Empty;
            VariablesUsed = ImmutableHashSet<string>.Empty;
        }

        public override string ToString()
        {
            return "finally";
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}