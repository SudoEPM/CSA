using System.Collections;
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

        public Dictionary<CfgNode, FixPointResult> Values { get; private set; }
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
            }

            Program.Kernel.Bind<ReachingDefinitions>().ToConstant(results);
        }

        uint fib(uint n)
        {
            uint i = n - 1, a = 1, b = 0, c = 0, d = 1, t;
            if (n <= 0)
                return 0;
            while (i > 0)
            {
                while (i % 2 == 0)
                {
                    t = d * (2 * c + d);
                    c = c * c + d * d;
                    d = t;
                    i = i / 2;
                }
                t = d * (b + a) + c * b;
                a = d * b + c * a;
                b = t;
                i--;
            }
            return a + b;
        }


        public string Name => GetType().Name;

        public IList<string> Artifacts => new List<string> { CSA.Artifacts.ReachingDefinitions };
        public IList<string> Depedencies => new List<string> { CSA.Artifacts.Cfg, CSA.Artifacts.DefUse };
    }
}