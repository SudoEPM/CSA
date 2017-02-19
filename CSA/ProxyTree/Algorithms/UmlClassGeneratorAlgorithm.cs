using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSA.GraphVizExtension;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes;
using DotBuilder;
using DotBuilder.Attributes;
using DotBuilder.Statements;
using Microsoft.CodeAnalysis.CSharp;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class UmlClassGeneratorAlgorithm : IProxyAlgorithm
    {
        private readonly FileStream _output;
        private readonly Dictionary<string, ClassNode> _classMapping;
        private readonly string _graphVizPath;
        private readonly GraphBase _umlGraph;

        private List<string> _currentConstructors;
        private List<string> _currentMethods;
        private List<string> _currentProperties;
        private List<string> _currentFields;
        private HashSet<string> _currentDepedencies;

        public UmlClassGeneratorAlgorithm(
            [Named("PostOrder")] IProxyIterator iterator, 
            [Named("UML-CLASS")] FileStream output, 
            ProgramOptions options, 
            [Named("ClassMapping")] Dictionary<string, ClassNode> classMapping)
        {
            Iterator = iterator;

            _output = output;
            _classMapping = classMapping;
            _graphVizPath = options.GraphVizPath;
            _umlGraph = Graph.Directed("UML").Of(Font.Name("Bitstream Vera Sans"), Font.Size(8))
                .With(AttributesFor.Node.Of(new Shape("record")));
        }

        public IProxyIterator Iterator { get; }

        public string Name => GetType().Name;

        public void Apply(IProxyNode node)
        {
            // Nothing to do
        }

        public void Apply(ClassNode node)
        {
            var content = new List<string>();

            var methods = new List<string>();
            if (_currentConstructors != null)
            {
                methods.AddRange(_currentConstructors);
                _currentConstructors = null;
            }
            if (_currentMethods != null)
            {
                methods.AddRange(_currentMethods);
                _currentMethods = null;
            }
            if(methods.Any())
                content.Add(string.Join(@"\l", methods) + @"\l");

            if (_currentProperties != null)
            {
                content.Add(string.Join(@"\l", _currentProperties) + @"\l");
                _currentProperties = null;
            }

            if (_currentFields != null)
            {
                content.Add(string.Join(@"\l", _currentFields) + @"\l");
                _currentFields = null;
            }

            // Gen the class
            _umlGraph.With(GNode.Name(node.Signature, string.Join("|", content)));

            foreach (var baseType in node.BaseTypes)
            {
                _umlGraph.With(Edge.Between(node.Signature, baseType).Of(new ArrowHead("empty")));
            }

            if (_currentDepedencies != null)
            {
                foreach (var depedency in _currentDepedencies)
                {
                    _umlGraph.With(Edge.Between(node.Signature, depedency));
                }
                _currentDepedencies = null;
            }
        }

        public void Apply(ForestNode node)
        {
            // Do nothing on root for now
            var graphviz = new DotBuilder.GraphViz(_graphVizPath, OutputFormat.Png);

            // For debug purpose
            /*Console.WriteLine(dotFile);
            var dotFile = _umlGraph.Render();
            using (TextWriter fs = new StreamWriter("uml.dot"))
            {
                fs.WriteLine(dotFile);
            }*/

            graphviz.RenderGraph(_umlGraph, _output);
            _output.Flush();
            _output.Close();
            System.Diagnostics.Process.Start(_output.Name);
        }

        public void Apply(MethodNode node)
        {
            List<string> current;
            switch (node.Kind)
            {
                case SyntaxKind.ConstructorDeclaration:
                    if (_currentConstructors == null)
                        _currentConstructors = new List<string>();
                    current = _currentConstructors;
                    break;
                case SyntaxKind.MethodDeclaration:
                    if (_currentMethods == null)
                        _currentMethods = new List<string>();
                    current = _currentMethods;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            

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
            line += $"{node.Name}({string.Join(", ", node.Parameters.Select(x => x.Item1))})";
            if (node.ReturnType != "")
            {
                line += $" : {node.ReturnType}";
            }
            current.Add(line);
        }

        public void Apply(PropertyNode node)
        {
            Apply(node as IProxyNode);
        }

        public void Apply(PropertyAccessorNode node)
        {
            if(_currentProperties == null)
                _currentProperties = new List<string>();

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

            var ret = node.Accessor == "get" ? $" : {node.Type}" : "";
            line += $"{node.Signature}({(node.Accessor == "set" ? node.Type : "")}){ret}";

            _currentProperties.Add(line);
            if (node.IsAutomatic && _classMapping.ContainsKey(node.Type))
            {
                if (_currentDepedencies == null)
                    _currentDepedencies = new HashSet<string>();

                _currentDepedencies.Add(node.Type);
            }
        }

        public void Apply(FieldNode node)
        {
            if (_currentFields == null)
                _currentFields = new List<string>();

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

            foreach (var variable in node.Variables)
            {
                var res = line + $"{variable} : {node.Type}";
                _currentFields.Add(res);
            }

            if (_classMapping.ContainsKey(node.Type))
            {
                if(_currentDepedencies == null)
                    _currentDepedencies = new HashSet<string>();

                _currentDepedencies.Add(node.Type);
            }
        }
    }
}