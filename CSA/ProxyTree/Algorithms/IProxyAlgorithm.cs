using System.Collections.Generic;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Iterators;

namespace CSA.ProxyTree.Algorithms
{
    public interface IProxyAlgorithm
    {
        IProxyIterator Iterator { get; }

        void Apply(IProxyNode node);
        void Apply(ForestNode node);
        void Apply(ClassNode node);
        void Apply(MethodNode node);
        void Apply(PropertyNode node);
        void Apply(PropertyAccessorNode node);
    }
}
