using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Visitors;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class PrintTreeAlgorithm : IProxyAlgorithm
    {
        private readonly IProxyIterator _iterator;

        public PrintTreeAlgorithm([Named("PreOrder")] IProxyIterator iterator)
        {
            _iterator = iterator;
        }

        public string Name => GetType().Name;

        public void Execute()
        {
            var visitor = Program.Kernel.Get<PrintTreeVisitor>();
            foreach (var node in _iterator.Enumerable)
            {
                node.Accept(visitor);
            }
        }
    }
}