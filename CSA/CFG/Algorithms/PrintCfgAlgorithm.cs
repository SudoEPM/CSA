using System.IO;
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
        private readonly FileStream _output;
        private readonly string _graphVizPath;
        private GraphBase _graph
            ;

        public PrintCfgAlgorithm([Named("CFG")] CfgGraph graph, [Named("CFG")] FileStream output, ProgramOptions options)
        {
            _cfg = graph;
            _output = output;

            _graphVizPath = options.GraphVizPath;
            _graph = Graph.Directed("UML").Of(Font.Name("Bitstream Vera Sans"), Font.Size(8))
                .With(AttributesFor.Node.Of(Shape.Box));
        }

        public void Execute()
        {
            return;
            foreach (var method in _cfg.CfgMethods)
            {
                _graph.With(Node.Name(method.Key).Of(Shape.Diamond));
                _graph.With(Node.Name(method.Value.Root.ToString()));
                _graph.With(Edge.Between(method.Key, method.Value.Root.ToString()));

                ICfgIterator iterator = new PreOrderDepthFirstCfgIterator(method.Value.Root);
                foreach (var node in iterator.GetEnumerable())
                {
                    
                }
            }

            var graphviz = new GraphViz(_graphVizPath, OutputFormat.Png);

            // For debug purpose
            /*var dotFile = _graph.Render();
            //Console.WriteLine(dotFile);
            using (TextWriter fs = new StreamWriter("uml.dot"))
            {
                fs.WriteLine(dotFile);
            }*/

            graphviz.RenderGraph(_graph, _output);
            _output.Flush();
            _output.Close();
            System.Diagnostics.Process.Start(_output.Name);
        }

        public string Name => GetType().Name;
    }
}