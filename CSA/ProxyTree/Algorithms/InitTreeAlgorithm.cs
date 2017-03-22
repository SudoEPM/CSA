using System.Collections.Generic;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Visitors;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class InitTreeAlgorithm : IProduceArtefactsAlgorithm
    {
        public string Name => GetType().Name;

        public void Execute()
        {
            var iterator = Program.Kernel.Get<IProxyIterator>("PreOrder");
            var visitor = Program.Kernel.Get<InitTreeVisitor>();
            foreach (var node in iterator.Enumerable)
            {
                node.Accept(visitor);
            }
        }

        public IList<string> Depedencies => new List<string> { CSA.Artifacts.ParseTree };
        public IList<string> Artifacts => new List<string> { CSA.Artifacts.Ast };
    }
}