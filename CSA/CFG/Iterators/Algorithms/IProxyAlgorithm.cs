using System.Collections.Generic;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes.Interfaces;

namespace CSA.ProxyTree.Algorithms
{
    public interface IProxyAlgorithm
    {
        IProxyIterator Iterator { get; }
        string Name { get; }

        void Apply(IProxyNode node);
        void Apply(ForestNode node);
        void Apply(ClassNode node);
        void Apply(MethodNode node);
        void Apply(PropertyNode node);
        void Apply(PropertyAccessorNode node);
        void Apply(FieldNode node);
        void Apply(StatementNode node);
        void Apply(ExpressionNode node);
    }
}
