using Microsoft.CodeAnalysis;

namespace CSA.ProxyTree.Nodes
{
    class StatementNode : BasicProxyNode
    {
        public StatementNode(SyntaxNode origin) : base(origin)
        {
        }
    }
}