using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSA.Options;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Visitors.Standards;
using DotBuilder;
using DotBuilder.Attributes;
using DotBuilder.Statements;
using Ninject;

namespace CSA.ProxyTree.Visitors
{
    class UmlPackageGeneratorVisitor : DoNothingVisitor
    {
        private readonly FileStream _output;
        private readonly string _graphVizPath;
        private readonly GraphBase _umlGraph;

        private readonly HashSet<string> _packages;
        private readonly HashSet<string> _acceptedPackages;
        private readonly HashSet<Tuple<string, string>> _links;
        private readonly int _maxNestedPackage;

        public UmlPackageGeneratorVisitor(
            [Named("UML-PACKAGE")] FileStream output, 
            ProgramOptions options)
        {

            _output = output;
            _graphVizPath = options.GraphVizPath;
            _umlGraph = Graph.Directed("UML").Of(Font.Name("Bitstream Vera Sans"), Font.Size(8))
                .With(AttributesFor.Node.Of(new Shape("record")));
            _packages = new HashSet<string>();

            _acceptedPackages = options.PackagesIncludeExternal ? null : new HashSet<string>();

            _links = new HashSet<Tuple<string, string>>();
            _maxNestedPackage = options.PackageNesting;
        }

        private string Strip(string package, int max)
        {
            return max == -1 ? package : string.Join(".", package.Split('.').Take(max));
        }

        public override void Apply(ClassNode node)
        {
            if (string.IsNullOrWhiteSpace(node.Namespace))
                return;

            // Gen the class
            var currentPackage = Strip(node.Namespace, _maxNestedPackage);
            _packages.Add(currentPackage);
            _acceptedPackages?.Add(currentPackage);

            foreach (var package in node.NamespaceDepedencies)
            {
                var dep = Strip(package, _maxNestedPackage);
                _packages.Add(dep);
                _links.Add(new Tuple<string, string>(currentPackage, dep));
            }
        }

        public override void Apply(ForestNode node)
        {
            foreach (var package in _packages)
            {
                if(_acceptedPackages != null && !_acceptedPackages.Contains(package))
                    continue;

                _umlGraph.With(Node.Name(package));
            }

            foreach (var link in _links)
            {
                if (_acceptedPackages != null && (!_acceptedPackages.Contains(link.Item1) || !_acceptedPackages.Contains(link.Item2)))
                    continue;

                _umlGraph.With(Edge.Between(link.Item1, link.Item2));
            }

            // Do nothing on root for now
            var graphviz = new DotBuilder.GraphViz(_graphVizPath, OutputFormat.Png);

            // For debug purpose
            /*var dotFile = _umlGraph.Render();
            //Console.WriteLine(dotFile);
            using (TextWriter fs = new StreamWriter("uml.dot"))
            {
                fs.WriteLine(dotFile);
            }*/

            graphviz.RenderGraph(_umlGraph, _output);
            _output.Flush();
            _output.Close();
        }
    }
}