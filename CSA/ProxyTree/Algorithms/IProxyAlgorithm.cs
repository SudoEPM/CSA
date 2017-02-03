using System.Collections.Generic;
using CSA.ProxyTree.Nodes;

namespace CSA.ProxyTree.Algorithms
{
    interface IProxyAlgorithm
    {
        void Begin();
        void End();

        void Accept(IProxyNode node);
        void Accept(MethodProxyNode node);
    }
}
