using System;
using System.Collections.Generic;
using CSA.ProxyTree.Nodes.Interfaces;

namespace CSA.ProxyTree.Iterators
{
    public interface IProxyIterator
    {
        IProxyNode Root { get; }
        IEnumerable<IProxyNode> Enumerable { get; }
        bool SkipStatements { get; set; }
    }
}