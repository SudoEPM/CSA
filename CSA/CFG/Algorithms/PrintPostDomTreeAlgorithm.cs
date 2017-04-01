using System.Collections.Generic;
using System.IO;
using CSA.CFG.Nodes;
using CSA.Options;
using DotBuilder;
using DotBuilder.Attributes;
using DotBuilder.Statements;
using Ninject;

namespace CSA.CFG.Algorithms
{
    class PrintPostDomTreeAlgorithm : IAlgorithm
    {
        private readonly string _outputFolder;
        private readonly string _graphVizPath;

        public PrintPostDomTreeAlgorithm(ProgramOptions options)
        {
            _outputFolder = "pdom-graph";
            Directory.CreateDirectory(_outputFolder);

            _graphVizPath = options.GraphVizPath;
        }

        public void Execute()
        {
            var domTrees = Program.Kernel.Get<ForestDomTree>("PDomTree");
            var classGraphs = new Dictionary<string, GraphBase>();

            foreach (var method in domTrees.Forest)
            {
                GraphBase graph;
                if (!classGraphs.ContainsKey(method.Key.ClassSignature))
                {
                    graph = Graph.Directed("UML").Of(Font.Name("Bitstream Vera Sans"), Font.Size(8))
                        .With(AttributesFor.Node.Of(Shape.Box));
                    classGraphs[method.Key.ClassSignature] = graph;
                }
                graph = classGraphs[method.Key.ClassSignature];

                var subGraph = Subgraph.Cluster;
                subGraph.Of(Label.With(method.Key.ClassSignature));
                graph.With(subGraph);

                Execute(method.Value, subGraph);
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

        private void Execute(IDomTree tree, Subgraph subGraph)
        {
            foreach (var domLink in tree.DomParent)
            {
                var child = domLink.Key;
                var parent = domLink.Value;

                var from = Node.Name(parent.UniqueId).Of(Label.With(parent.ToDotString(true)));
                if (parent.Origin == null)
                {
                    @from = @from.Of(Shape.Diamond);
                }

                var to = Node.Name(child.UniqueId).Of(Label.With(child.ToDotString(true)));
                if (child.Origin == null)
                {
                    to = to.Of(Shape.Diamond);
                }

                subGraph.With(@from);
                subGraph.With(to);
                subGraph.With(Edge.Between(parent.UniqueId, child.UniqueId));
            }
        }

        public string Name => GetType().Name;

        public IList<string> Depedencies => new List<string> { CSA.Artifacts.PostDomTree };
    }
}