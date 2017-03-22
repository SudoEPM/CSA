using System.IO;
using System.Linq;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class PrintDefUseAlgorithm : IProxyAlgorithm
    {
        public string Name => GetType().Name;

        public void Execute()
        {
            var iterator = Program.Kernel.Get<IProxyIterator>("PostOrder");
            using (TextWriter fs = new StreamWriter("def-use.txt"))
            {
                foreach (var node in iterator.Enumerable.OfType<StatementNode>())
                {
                    var method = node.Ancestors().OfType<ICallableNode>().FirstOrDefault();
                    if(method == null)
                        continue;

                    fs.WriteLine(method.Signature + "#" + node.LineNumber);
                    fs.Write("Def: ");
                    fs.WriteLine(string.Join(", ", node.VariablesDefined));
                    fs.Write("Use: ");
                    fs.WriteLine(string.Join(", ", node.VariablesUsed));
                    fs.WriteLine();
                }
            }
        }
    }
}