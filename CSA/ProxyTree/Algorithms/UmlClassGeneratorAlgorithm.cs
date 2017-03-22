using System.Collections.Generic;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Visitors;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class UmlClassGeneratorAlgorithm : IProxyAlgorithm
    {
        public string Name => GetType().Name;

        public void Execute()
        {
            var iterator = Program.Kernel.Get<IProxyIterator>("PostOrder");
            iterator.SkipStatements = true;
            var visitor = Program.Kernel.Get<UmlClassGeneratorVisitor>();
            foreach (var node in iterator.Enumerable)
            {
                node.Accept(visitor);
            }
        }

        public IList<string> Depedencies => new List<string> { CSA.Artifacts.Ast };
    }
}