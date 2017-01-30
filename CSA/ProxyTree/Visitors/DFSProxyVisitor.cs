using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Nodes;

namespace CSA.ProxyTree.Visitors
{
    class DFSProxyVisitor : BasicForestProxyVisitor
    {
        public DFSProxyVisitor(IProxyAlgorithm algorithm) : base(algorithm)
        {
        }

        public override void Visit(SyntaxProxyNode node)
        {
            Algorithm.Accept(node);
            foreach (var child in node.Childs)
            {
                Visit(child);
            }
        }
    }
}