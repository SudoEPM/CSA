using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CSA.CFG.Nodes;
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
    
    class ReachingDefinitionsAlgorithm : IProduceArtefactsAlgorithm
    {
        public void Execute()
        {
            var cfg = Program.Kernel.Get<CfgGraph>("CFG");
            var results = new ReachingDefinitions();

            foreach (var method in cfg.CfgMethods.Where(x => x.Value.Root != null))
            {
                Execute(method.Value, results.Values);
            }

            Program.Kernel.Bind<ReachingDefinitions>().ToConstant(results);
        }

        private void Execute(CfgMethod method, Dictionary<CfgNode, FixPointResult> results)
        {
            var nodes = method.Root.NodeEnumerator.Where(x => x.Origin != null).ToList();

            // Find all the definitions in the method   
            var definitions = new Dictionary<string, HashSet<CfgNode>>();
            foreach (var node in nodes)
            {
                foreach (var variable in node.Origin.VariablesDefined)
                {
                    if (!definitions.ContainsKey(variable))
                    {
                        definitions[variable] = new HashSet<CfgNode>();
                    }

                    definitions[variable].Add(node);
                }
            }

            // Initialize the data structures used by the fix-point
            nodes.ForEach(x => results[x] = new FixPointResult());
            var workList = new Queue<CfgNode>(nodes);
            var workSet = new HashSet<CfgNode>(nodes);

            while (workList.Any())
            {
                var current = workList.Dequeue();
                workSet.Remove(current);

                var data = results[current];

                data.Gen = current.Origin.VariablesDefined.Select(x => $"d{current.UniqueId}-{x}").ToImmutableHashSet();
                data.Kill = current.Origin.VariablesDefined.SelectMany(variable => definitions[variable].Select(def => $"d{def.UniqueId}-{variable}")).ToImmutableHashSet();
                data.In = current.Prec.Where(x => x.Origin != null).SelectMany(x => results[x].Out).ToImmutableHashSet();

                var newOut = data.Gen.Union(data.In.Except(data.Kill));
                if (!data.Out.SetEquals(newOut))
                {
                    data.Out = newOut;
                    foreach (var next in current.Next.Where(x => x.Origin != null))
                    {
                        if (!workSet.Contains(next))
                        {
                            workList.Enqueue(next);
                            workSet.Add(next);
                        }
                    }
                }

            }
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