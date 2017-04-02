using System.Collections.Generic;
using System.Linq;
using CSA.Options;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.RoslynWalkers;
using Microsoft.CodeAnalysis.MSBuild;

namespace CSA.ProxyTree.Algorithms
{
    class ParseProjectAlgorithm : IProduceArtefactsAlgorithm
    {
        private readonly ProgramOptions _programOptions;

        public ParseProjectAlgorithm(ProgramOptions programOptions)
        {
            _programOptions = programOptions;
        }

        public string Name => GetType().Name;

        public void Execute()
        {
            var forest = ParseForest(_programOptions.TestMode ? _programOptions.DebugSolution : _programOptions.Solution);
            Program.Kernel.Bind<IProxyNode>().ToMethod(x => forest).Named("Root");
        }

        private static IProxyNode ParseForest(string filepath)
        {
            var workspace = MSBuildWorkspace.Create();
            var sol = workspace.OpenSolutionAsync(filepath).Result;
            var forest = new List<IProxyNode>();
            var projects = sol.Projects.ToList();
            foreach (var proj in projects)
            {
                forest.AddRange(
                    proj.Documents/*.Where(x => x.Name.Contains("MetricCalc"))*/.Select(
                        doc => doc.GetSyntaxTreeAsync().Result.GenerateProxy(doc.GetSemanticModelAsync().Result, doc)));
            }
            return new ForestNode(forest);
        }

        public IList<string> Depedencies => new List<string>();
        public IList<string> Artifacts => new List<string> {CSA.Artifacts.ParseTree};
    }
}