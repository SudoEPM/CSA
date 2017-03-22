using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class ComputeDefUseAlgorithm : IProduceArtefactsAlgorithm
    {
        public string Name => GetType().Name;

        public void Execute()
        {
            var iterator = Program.Kernel.Get<IProxyIterator>("PostOrder");
            foreach (var node in iterator.Enumerable.OfType<StatementNode>())
            {
                node.ComputeDefUse();
            }
        }

        public IList<string> Depedencies => new List<string> { CSA.Artifacts.ParseTree };
        public IList<string> Artifacts => new List<string> { CSA.Artifacts.DefUse };
    }
}