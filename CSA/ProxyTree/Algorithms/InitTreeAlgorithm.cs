using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Visitors;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class InitTreeAlgorithm : IProxyAlgorithm
    {
        private readonly IProxyIterator _iterator;

        public InitTreeAlgorithm([Named("PreOrder")] IProxyIterator iterator)
        {
            _iterator = iterator;
        }

        public string Name => GetType().Name;

        public void Execute()
        {
            var visitor = Program.Kernel.Get<InitTreeVisitor>();
            foreach (var node in _iterator.Enumerable)
            {
                node.Accept(visitor);
            }
        }
    }
}