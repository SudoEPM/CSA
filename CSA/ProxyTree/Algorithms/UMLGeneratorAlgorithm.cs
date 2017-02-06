using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes;
using DotBuilder;
using DotBuilder.Attributes;
using DotBuilder.Statements;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class UmlGeneratorAlgorithm : IProxyAlgorithm
    {
        private readonly FileStream _output;
        private readonly string _graphVizPath;
        private readonly GraphBase _umlGraph;

        public UmlGeneratorAlgorithm([Named("PostOrder")] IProxyIterator iterator, [Named("UML")] FileStream output, ProgramOptions options)
        {
            Iterator = iterator;

            _output = output;
            _graphVizPath = options.GraphVizPath;
            _umlGraph = Graph.Directed("UML").Of(Font.Name("Bitstream Vera Sans"), Font.Size(8))
                .With(AttributesFor.Node.Of(new Shape("record")));
        }

        public IProxyIterator Iterator { get; }

        public void Apply(IProxyNode node)
        {
            // TODO
        }

        public void Apply(ClassNode node)
        {
            var content = string.Join(@"\l", node.Childs.Where(x => x is MethodNode).Select(x => ToUml((MethodNode)x))) + @"\l";

            // Gen the class
            _umlGraph.With(GNode.Name(node.Signature, content));
        }

        public void Apply(ForestNode node)
        {
            // Do nothing on root for now
            var graphviz = new GraphViz(_graphVizPath, OutputFormat.Png);

            Console.WriteLine(_umlGraph.Render());

            graphviz.RenderGraph(_umlGraph, _output);
            _output.Flush();
            _output.Close();
            System.Diagnostics.Process.Start(_output.Name);
        }

        public void Apply(MethodNode node)
        {
            Apply(node as IProxyNode);
        }

        public void Apply(PropertyNode node)
        {
            Apply(node as IProxyNode);
        }

        public void Apply(PropertyAccessorNode node)
        {
            Apply(node as IProxyNode);
        }

        public string ToUml(MethodNode node)
        {
            var line = "";
            switch (node.Protection)
            {
                case "private":
                    line += "- ";
                    break;
                case "public":
                    line += "+ ";
                    break;
                case "protected":
                    line += "# ";
                    break;
                case "internal":
                    line += "~ ";
                    break;
            }
            line += node.Name;
            var test = node.Parameters;
            var args = node.Parameters.Substring(1, node.Parameters.Length - 2).Split(',');

            return line;
        }
    }
}