using System;
using System.IO;
using System.Linq;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.ProxyTree.Visitors.Standards;
using Ninject;

namespace CSA.ProxyTree.Visitors
{
    class PrintTreeVisitor : ApplyDefaultVisitor
    {
        private readonly TextWriter _output;

        public PrintTreeVisitor([Named("Generic")] TextWriter output)
        {
            _output = output;
        }


        public override void Apply(IProxyNode node)
        {
            int padding = node.Ancestors().Count();
            //To identify leaf nodes vs nodes with children
            string prepend = node.Childs.Any() ? "[-]" : "[.]";
            //Get the type of the node
            string line = new String(' ', padding) + prepend + " " + node.Kind;
            _output.WriteLine(line);
        }

        public override void Apply(ForestNode node)
        {
            _output.WriteLine("Root");
        }
    }
}