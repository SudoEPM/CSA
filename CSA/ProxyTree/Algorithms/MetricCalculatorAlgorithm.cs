using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Visitors;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class MetricCalculatorAlgorithm : IProxyAlgorithm
    {
        private readonly IProxyIterator _iterator;

        public MetricCalculatorAlgorithm([Named("PostOrder")] IProxyIterator iterator)
        {
            _iterator = iterator;
        }

        public string Name => GetType().Name;

        public void Execute()
        {
            var visitor = Program.Kernel.Get<MetricCalculatorVisitor>();
            foreach (var node in _iterator.Enumerable)
            {
                node.Accept(visitor);
            }
        }
    }
}