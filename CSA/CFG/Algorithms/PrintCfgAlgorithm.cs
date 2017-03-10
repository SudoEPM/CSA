using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSA.CFG.Iterators;
using CSA.CFG.Nodes;
using DotBuilder;
using DotBuilder.Attributes;
using DotBuilder.Statements;
using Ninject;

namespace CSA.CFG.Algorithms
{
    class PrintCfgAlgorithm : ICfgAlgorithm
    {
        private readonly CfgGraph _cfg;
        private readonly string _outputFolder;
        private readonly string _graphVizPath;

        public PrintCfgAlgorithm([Named("CFG")] CfgGraph graph, ProgramOptions options)
        {
            _cfg = graph;
            // TODO:: REMOVE SOON
            foreach (var method in graph.CfgMethods.Values)
            {
                method.RemoveBlocks();
            }
            _outputFolder = "cfg-graph";
            Directory.CreateDirectory(_outputFolder);

            _graphVizPath = options.GraphVizPath;
        }

        public void Execute()
        {
            var classGraphs = new Dictionary<string, GraphBase>();
            var methodSubGraphs = new Dictionary<string, Dictionary<string, Subgraph>>();

            foreach (var method in _cfg.CfgMethods.Where(x => x.Value.Root != null))
            {
                GraphBase graph;
                if (!classGraphs.ContainsKey(method.Value.ClassSignature))
                {
                    graph = Graph.Directed("UML").Of(Font.Name("Bitstream Vera Sans"), Font.Size(8))
                        .With(AttributesFor.Node.Of(Shape.Box));
                    classGraphs[method.Value.ClassSignature] = graph;
                    methodSubGraphs[method.Value.ClassSignature] = new Dictionary<string, Subgraph>();
                }
                graph = classGraphs[method.Value.ClassSignature];
                var methodSubGraph = methodSubGraphs[method.Value.ClassSignature];

                Subgraph subGraph;
                if (!methodSubGraph.ContainsKey(method.Key))
                {
                    subGraph = Subgraph.Cluster;
                    subGraph.Of(Label.With(method.Key));
                    graph.With(subGraph);
                    methodSubGraph[method.Key] = subGraph;
                }
                subGraph = methodSubGraph[method.Key];

                subGraph.With(Node.Name(method.Key + ":Entry").Of(Label.With("Entry")).Of(Shape.Diamond));
                subGraph.With(Node.Name(method.Value.Root.UniqueId).Of(Label.With(method.Value.Root.ToDotString())));
                subGraph.With(Edge.Between(method.Key + ":Entry", method.Value.Root.UniqueId));

                foreach (var link in method.Value.Root.LinkEnumerator)
                {
                    subGraph.With(Node.Name(link.From.UniqueId).Of(Label.With(link.From.ToDotString())));
                    subGraph.With(Node.Name(link.To.UniqueId).Of(Label.With(link.To.ToDotString())));
                    subGraph.With(Edge.Between(link.From.UniqueId, link.To.UniqueId));
                }
            }

            var graphviz = new GraphViz(_graphVizPath, OutputFormat.Png);

            foreach (var classGraph in classGraphs)
            {
                var file = new FileStream(_outputFolder + "/" + classGraph.Key + ".png", FileMode.Create);
                graphviz.RenderGraph(classGraph.Value, file);

                // For debug purpose
                var dotFile = classGraph.Value.Render();
                //Console.WriteLine(dotFile);
                using (TextWriter fs = new StreamWriter(_outputFolder + "/" + classGraph.Key + ".dot"))
                {
                    fs.WriteLine(dotFile);
                }
            }

            System.Diagnostics.Process.Start(_outputFolder);
        }

        public string Name => GetType().Name;
    }
}