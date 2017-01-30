using System.Collections.Generic;
using CSA.ProxyTree.Nodes;

namespace CSA.ProxyTree.Visitors
{
    interface IProxyVisitor
    {
        void Visit(List<SyntaxProxyNode> forest);
        void Visit(SyntaxProxyNode node);
    }
}