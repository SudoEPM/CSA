using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Visitors;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class UmlClassGeneratorAlgorithm : IProxyAlgorithm
    {
        private readonly IProxyIterator _iterator;

        public UmlClassGeneratorAlgorithm([Named("PostOrder")] IProxyIterator iterator)
        {
            _iterator = iterator;
            _iterator.SkipStatements = true;
        }

        public string Name => GetType().Name;

        public void Execute()
        {
            var visitor = Program.Kernel.Get<UmlClassGeneratorVisitor>();
            foreach (var node in _iterator.Enumerable)
            {
                node.Accept(visitor);
            }
        }
    }
}