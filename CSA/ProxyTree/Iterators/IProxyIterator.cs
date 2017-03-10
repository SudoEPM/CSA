using System;
using System.Collections.Generic;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;

namespace CSA.ProxyTree.Iterators
{
    public interface IProxyIterator
    {
        HashSet<Type> NodesToSkip { get; } 
        IEnumerable<IProxyNode> GetEnumerable();
    }
}