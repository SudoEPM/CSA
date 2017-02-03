using System;
using System.IO;
using System.Linq;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class PrintTreeAlgorithm : IProxyAlgorithm
    {
        private readonly TextWriter _output;

        public PrintTreeAlgorithm([Named("PreOrder")] IProxyIterator iterator, [Named("Generic")] TextWriter output)
        {
            _output = output;
            Iterator = iterator;
        }

        public IProxyIterator Iterator { get; }
        public void Apply(IProxyNode node)
        {
            int padding = node.Ancestors().Count();
            //To identify leaf nodes vs nodes with children
            string prepend = node.Childs.Any() ? "[-]" : "[.]";
            //Get the type of the node
            string line = new String(' ', padding) + prepend + " " + node.Kind;
            _output.WriteLine(line);
        }

        public void Apply(ForestNode node)
        {
            _output.WriteLine("Root");
        }

        public void Apply(MethodNode node)
        {
            Apply(node as IProxyNode);
        }

        public void Apply(PropertyNode node)
        {
            Apply(node as IProxyNode);
        }
    }
}