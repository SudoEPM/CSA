using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes
{
    public class StatementNode : BasicProxyNode
    {
        public StatementNode(SyntaxNode origin) : base(origin)
        {
        }
    }
}