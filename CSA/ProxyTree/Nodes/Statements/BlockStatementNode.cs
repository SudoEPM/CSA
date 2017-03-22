using System.Collections.Immutable;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class BlockStatementNode : StatementNode
    {
        public BlockStatementNode(SyntaxNode origin) : base(origin, false)
        {
            VariablesDefined = ImmutableHashSet<string>.Empty;
            VariablesUsed = ImmutableHashSet<string>.Empty;
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}