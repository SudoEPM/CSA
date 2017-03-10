using CSA.ProxyTree.Algorithms;
using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes
{
    public class ExpressionNode : BasicProxyNode
    {
        public ExpressionNode(SyntaxNode origin) : base(origin)
        {
        }

        public override void Accept(IProxyAlgorithm algorithm) => algorithm.Apply(this);
    }
}