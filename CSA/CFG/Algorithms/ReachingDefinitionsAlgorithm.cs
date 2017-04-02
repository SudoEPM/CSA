using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CSA.CFG.Nodes;
using CSA.FixPoint;
using Ninject;

namespace CSA.CFG.Algorithms
{
    class ReachingDefinitions
    {
        public ReachingDefinitions()
        {
            Values = new Dictionary<CfgNode, FixPointResult>();
            DefinedVariables = new Dictionary<CfgMethod, HashSet<string>>();
            VariablesReferences = new Dictionary<CfgMethod, Dictionary<string, HashSet<CfgNode>>>();
        }

        public FixPointResult this[CfgNode key]
        {
            get
            {
                return Values[key];
            }
            set
            {
                Values[key] = value;
            }
        }

        public Dictionary<CfgNode, FixPointResult> Values { get; }
        public Dictionary<CfgMethod, HashSet<string>> DefinedVariables { get; }
        public Dictionary<CfgMethod, Dictionary<string, HashSet<CfgNode>>> VariablesReferences { get; }
    }

    class ReachingDefinitionsFixPoint : FixPointAnalysis
    {
        protected override bool IsForward => true;
        protected override bool RecomputeGenKill => false;
        protected override MeetOperatorType MeetOperator => MeetOperatorType.Union;

        private readonly Dictionary<string, HashSet<CfgNode>> _definitions;

        public ReachingDefinitionsFixPoint()
        {
            _definitions = new Dictionary<string, HashSet<CfgNode>>();
        }

        protected override void Init(IEnumerable<IFixPointAnalyzableNode> nodes)
        {
            foreach (var fixPointAnalyzableNode in nodes)
            {
                var node = (CfgNode) fixPointAnalyzableNode;
                if (node.Origin == null)
                    continue;

                foreach (var variable in node.Origin.VariablesDefined)
                {
                    if (!_definitions.ContainsKey(variable))
                    {
                        _definitions[variable] = new HashSet<CfgNode>();
                    }

                    _definitions[variable].Add(node);
                }
            }
        }

        protected override FixPointResult Init(IFixPointAnalyzableNode node)
        {
            var data = new FixPointResult();
            var cfgNode = (CfgNode) node;
            if (cfgNode.Origin != null)
            {
                data.Gen = cfgNode.Origin.VariablesDefined.Select(x => $"d{cfgNode.UniqueId}-{x}").ToImmutableHashSet();
                data.Kill = cfgNode.Origin.VariablesDefined.SelectMany(variable => _definitions[variable].Select(def => $"d{def.UniqueId}-{variable}")).ToImmutableHashSet();
            }
            
            return data;
        }

        protected override ImmutableHashSet<string> TransferFunction(IFixPointAnalyzableNode node, FixPointResult data)
        {
            return data.Gen.Union(data.In.Except(data.Kill)); ;
        }
    }

    class ReachingDefinitionsAlgorithm : IProduceArtefactsAlgorithm
    {
        public void Execute()
        {
            var cfg = Program.Kernel.Get<CfgGraph>("CFG");
            
            var results = new ReachingDefinitions();
            var fixPoint = new ReachingDefinitionsFixPoint();
            foreach (var method in cfg.CfgMethods.Where(x => x.Value.Root != null))
            {
                var methodResults = fixPoint.Execute(method.Value.Root.NodeEnumerator);
                foreach (var pair in methodResults)
                {
                    results[pair.Key as CfgNode] = pair.Value;
                }

                results.DefinedVariables[method.Value] = new HashSet<string>();
                results.VariablesReferences[method.Value] = new Dictionary<string, HashSet<CfgNode>>();
                foreach (var node in method.Value.Root.NodeEnumerator.Where(x => x.Origin != null))
                {
                    results.DefinedVariables[method.Value].UnionWith(node.Origin.VariablesDefined);
                    foreach (var variable in node.Origin.VariablesDefined)
                    {
                        if (!results.VariablesReferences[method.Value].ContainsKey(variable))
                        {
                            results.VariablesReferences[method.Value][variable] = new HashSet<CfgNode>();
                        }

                        results.VariablesReferences[method.Value][variable].Add(node);
                    }

                    foreach (var variable in node.Origin.VariablesUsed)
                    {
                        if (!results.VariablesReferences[method.Value].ContainsKey(variable))
                        {
                            results.VariablesReferences[method.Value][variable] = new HashSet<CfgNode>();
                        }

                        results.VariablesReferences[method.Value][variable].Add(node);
                    }
                }
            }

            Program.Kernel.Bind<ReachingDefinitions>().ToConstant(results);
        }


        public string Name => GetType().Name;

        public IList<string> Artifacts => new List<string> { CSA.Artifacts.ReachingDefinitions };
        public IList<string> Depedencies => new List<string> { CSA.Artifacts.Cfg, CSA.Artifacts.DefUse };
    }
}