using System.Collections.Generic;
using CSA.ProxyTree.Nodes;

namespace CSA.ProxyTree.Iterators
{
    public interface IProxyIterator
    {
        IEnumerable<IProxyNode> GetEnumerable();
    }
}