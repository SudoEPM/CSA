using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Visitors;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class PrintTreeAlgorithm : IProxyAlgorithm
    {
        public string Name => GetType().Name;

        public void Execute()
        {
            var iterator = Program.Kernel.Get<IProxyIterator>("PreOrder");
            var visitor = Program.Kernel.Get<PrintTreeVisitor>();
            foreach (var node in iterator.Enumerable)
            {
                node.Accept(visitor);
            }
        }
    }
}