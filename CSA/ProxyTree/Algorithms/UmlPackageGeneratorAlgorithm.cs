using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Visitors;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class UmlPackageGeneratorAlgorithm : IProxyAlgorithm
    {
        private readonly IProxyIterator _iterator;

        public UmlPackageGeneratorAlgorithm([Named("PostOrder")] IProxyIterator iterator)
        {
            _iterator = iterator;
            _iterator.SkipStatements = true;
        }

        public string Name => GetType().Name;

        public void Execute()
        {
            var visitor = Program.Kernel.Get<UmlPackageGeneratorVisitor>();
            foreach (var node in _iterator.Enumerable)
            {
                node.Accept(visitor);
            }
        }
    }
}