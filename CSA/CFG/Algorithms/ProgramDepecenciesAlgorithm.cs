using System.Collections.Generic;
using CSA.CFG.Iterators;
using CSA.CFG.Nodes;
using Ninject;

namespace CSA.CFG.Algorithms
{
    class ProgramDepedencies
    {
        public ProgramDepedencies()
        {
            Graphs = new Dictionary<CfgMethod, HashSet<CfgLink>>();
        }

        public Dictionary<CfgMethod, HashSet<CfgLink>> Graphs { get; }

        public HashSet<CfgLink> this[CfgMethod method]
        {
            get
            {
                if (!Graphs.ContainsKey(method))
                {
                    Graphs[method] = new HashSet<CfgLink>();
                }

                return Graphs[method];
            }
        } 
    }

    class ProgramDepecenciesAlgorithm : IProduceArtefactsAlgorithm
    {
        private CfgGraph _cfgGraph;

        public void Execute()
        {
            _cfgGraph = Program.Kernel.Get<CfgGraph>("CFG");
            var depedencies = Program.Kernel.Get<ProgramDepedencies>();

            var dataDepedencies = Program.Kernel.Get<DataDepedencies>();
            var controlDepedencies = Program.Kernel.Get<ControlDepedencies>();

            foreach (var method in _cfgGraph.CfgMethods)
            {
                if(method.Value.Root == null)
                    continue;

                depedencies[method.Value].UnionWith(dataDepedencies[method.Value]);
                depedencies[method.Value].UnionWith(controlDepedencies[method.Value]);
            }
        }

        public string Name => GetType().Name;

        public IList<string> Artifacts => new List<string> { CSA.Artifacts.ProgramDepedencies };
        public IList<string> Depedencies => new List<string>{ CSA.Artifacts.DataDepedencies, CSA.Artifacts.ControlDepedencies };
    }
}
