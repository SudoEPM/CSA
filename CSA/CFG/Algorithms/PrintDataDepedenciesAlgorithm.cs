using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSA.CFG.Nodes;
using CSA.Options;
using DotBuilder;
using DotBuilder.Attributes;
using DotBuilder.Statements;
using Ninject;

namespace CSA.CFG.Algorithms
{
    class PrintDataDepedenciesAlgorithm : IAlgorithm
    {
        private readonly string _outputFolder;
        private readonly string _graphVizPath;
        private DataDepedencies _dataDepecencies;

        public PrintDataDepedenciesAlgorithm(ProgramOptions options)
        {
            _outputFolder = "data-depedencies";
            Directory.CreateDirectory(_outputFolder);

            _graphVizPath = options.GraphVizPath;
        }

        public void Execute()
        {
            var cfg = Program.Kernel.Get<CfgGraph>("CFG");
            _dataDepecencies = Program.Kernel.Get<DataDepedencies>();
            var classGraphs = new Dictionary<string, GraphBase>();

            foreach (var method in cfg.CfgMethods.Where(x => x.Value.Root != null))
            {
                GraphBase graph;
                if (!classGraphs.ContainsKey(method.Value.ClassSignature))
                {
                    graph = Graph.Directed("UML").Of(Font.Name("Bitstream Vera Sans"), Font.Size(8))
                        .With(AttributesFor.Node.Of(Shape.Box));
                    classGraphs[method.Value.ClassSignature] = graph;
                }
                graph = classGraphs[method.Value.ClassSignature];

                var subGraph = Subgraph.Cluster;
                subGraph.Of(Label.With(method.Key));
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
        }

        private void Execute(CfgMethod method, Subgraph subGraph)
        {
            foreach (var link in _dataDepecencies[method])
            {
                var from = Node.Name(link.From.UniqueId).Of(Label.With(link.From.ToDotString()));
                if (link.From.Origin == null)
                {
                    @from = @from.Of(Shape.Diamond);
                }

                var to = Node.Name(link.To.UniqueId).Of(Label.With(link.To.ToDotString()));
                if (link.To.Origin == null)
                {
                    to = to.Of(Shape.Diamond);
                }

                subGraph.With(@from);
                subGraph.With(to);
                subGraph.With(Edge.Between(link.From.UniqueId, link.To.UniqueId));
            }
        }

        public string Name => GetType().Name;

        public IList<string> Depedencies => new List<string> { CSA.Artifacts.DataDepedencies };
    }
}