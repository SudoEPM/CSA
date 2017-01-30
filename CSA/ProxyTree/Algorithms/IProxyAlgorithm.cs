using System.Collections.Generic;
using CSA.ProxyTree.Nodes;

namespace CSA.ProxyTree.Algorithms
{
    interface IProxyAlgorithm
    {
        void Begin(List<SyntaxProxyNode> forest);
        void End();

        void Accept(SyntaxProxyNode node);
    }
}
