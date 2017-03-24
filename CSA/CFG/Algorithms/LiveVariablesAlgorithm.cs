using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CSA.CFG.Nodes;
using CSA.FixPoint;
using Ninject;

namespace CSA.CFG.Algorithms
{
    class LiveVariables
    {
        public LiveVariables()
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

    class LiveVariablesFixPoint : FixPointAnalysis
    {
        protected override bool IsForward => false;
        protected override bool RecomputeGenKill => false;
        protected override MeetOperatorType MeetOperator => MeetOperatorType.Union;

        protected override FixPointResult Init(IFixPointAnalyzableNode node)
        {
            var data = new FixPointResult();
            var cfgNode = (CfgNode) node;
            if (cfgNode.Origin != null)
            {
                data.Gen = cfgNode.Origin.VariablesUsed.ToImmutableHashSet();
                data.Kill = cfgNode.Origin.VariablesDefined.ToImmutableHashSet();
            }
            
            return data;
        }

        protected override ImmutableHashSet<string> TransferFunction(IFixPointAnalyzableNode node, FixPointResult data)
        {
            return data.Gen.Union(data.In.Except(data.Kill)); ;
        }
    }

    class LiveVariablesAlgorithm : IProduceArtefactsAlgorithm
    {
        public void Execute()
        {
            var cfg = Program.Kernel.Get<CfgGraph>("CFG");
            
            var results = new LiveVariables();
            var fixPoint = new LiveVariablesFixPoint();
            foreach (var method in cfg.CfgMethods.Where(x => x.Value.Root != null))
            {
                var methodResults = fixPoint.Execute(method.Value.Root.NodeEnumerator);
                foreach (var pair in methodResults)
                {
                    results[pair.Key as CfgNode] = pair.Value;
                }
            }

            Program.Kernel.Bind<LiveVariables>().ToConstant(results);
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

        public IList<string> Artifacts => new List<string> { CSA.Artifacts.LiveVariables };
        public IList<string> Depedencies => new List<string> { CSA.Artifacts.Cfg, CSA.Artifacts.DefUse };
    }
}