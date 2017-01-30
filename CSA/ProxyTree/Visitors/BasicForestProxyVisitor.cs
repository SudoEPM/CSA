using System.Collections.Generic;
using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Nodes;

namespace CSA.ProxyTree.Visitors
{
    abstract class BasicForestProxyVisitor : IProxyVisitor
    {
        protected BasicForestProxyVisitor(IProxyAlgorithm algorithm)
        {
            Algorithm = algorithm;
        }

        protected IProxyAlgorithm Algorithm { get; }

        public void Visit(List<SyntaxProxyNode> forest)
        {
            Algorithm.Begin(forest);
            foreach (var root in forest)
            {
                Visit(root);
            }
            Algorithm.End();
        }

        public abstract void Visit(SyntaxProxyNode node);
    }
}