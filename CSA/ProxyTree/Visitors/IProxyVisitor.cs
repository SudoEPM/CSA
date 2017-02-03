using System.Collections.Generic;
using CSA.ProxyTree.Nodes;

namespace CSA.ProxyTree.Visitors
{
    interface IProxyVisitor
    {
        IEnumerable<IProxyNode> GetEnumerable();
    }
}