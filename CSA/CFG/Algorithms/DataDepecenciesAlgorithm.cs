using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CSA.CFG.Iterators;
using CSA.CFG.Nodes;
using CSA.FixPoint;
using Ninject;

namespace CSA.CFG.Algorithms
{
    class DataDepedencies
    {
        public DataDepedencies()
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

    class DataDepecenciesAlgorithm : IProduceArtefactsAlgorithm
    {
        private CfgGraph _cfgGraph;

        public void Execute()
        {
            _cfgGraph = Program.Kernel.Get<CfgGraph>("CFG");
            var definitions = Program.Kernel.Get<ReachingDefinitions>();

            var dataDepedencies = Program.Kernel.Get<DataDepedencies>();

            foreach (var method in _cfgGraph.CfgMethods)
            {
                if(method.Value.Root == null)
                    continue;

                var nodes = method.Value.Root.NodeEnumerator.ToImmutableHashSet();
                Execute(definitions.Values.Where(x => nodes.Contains(x.Key)).ToImmutableDictionary(), dataDepedencies[method.Value]);
            }
        }

        private void Execute(ImmutableDictionary<CfgNode, FixPointResult> definitions, HashSet<CfgLink> dataDepedencyLinks)
        {
            foreach (var definition in definitions)
            {
                foreach (var def in definition.Value.In)
                {
                    if (definition.Key.Origin == null)
                        continue;

                    var uniqueId = new string(def.Skip(1).TakeWhile(Char.IsDigit).ToArray());
                    var variable = new string(def.SkipWhile(x => x != '-').Skip(1).ToArray());

                    if (definition.Key.Origin.VariablesUsed.Contains(variable))
                    {
                        dataDepedencyLinks.Add(new CfgLink(_cfgGraph.CfgNodes[uniqueId], definition.Key));
                    }

                }
            }
        }

        public string Name => GetType().Name;

        public IList<string> Artifacts => new List<string> { CSA.Artifacts.DataDepedencies };
        public IList<string> Depedencies => new List<string>{ CSA.Artifacts.ReachingDefinitions, CSA.Artifacts.DefUse };
    }
}
