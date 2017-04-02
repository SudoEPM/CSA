using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using CSA.CFG.Nodes;
using CSA.Options;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using Ninject;

namespace CSA.CFG.Algorithms
{
    class SlicingAlgorithm : IAlgorithm
    {
        private readonly string _outputFolder;
        private readonly ProgramOptions _programOptions;
        private ReachingDefinitions _reachingDefinitions;
        private ProgramDepedencies _programDepedencies;

        public SlicingAlgorithm(ProgramOptions programOptions)
        {
            _programOptions = programOptions;

            _outputFolder = $"{Directory.GetCurrentDirectory()}\\slicing";
            if(Directory.Exists(_outputFolder))
                Directory.Delete(_outputFolder, true);
            Directory.CreateDirectory(_outputFolder);
        }

        public void Execute()
        {
            var forest = Program.Kernel.Get<IProxyNode>("Root");
            _reachingDefinitions = Program.Kernel.Get<ReachingDefinitions>();
            _programDepedencies = Program.Kernel.Get<ProgramDepedencies>();

            var solutionPath =
                Path.GetDirectoryName(_programOptions.TestMode
                    ? _programOptions.DebugSolution
                    : _programOptions.Solution);
            if (solutionPath == null) throw new ArgumentNullException(nameof(solutionPath));

            foreach (var file in forest.Childs.Where(x => x.FileName.Contains(solutionPath) && x.FileName.EndsWith(".cs")))
            {
                var relFilePath = file.FileName.Substring(solutionPath.Length);
                var path = $"{_outputFolder}{relFilePath}";

                if(path.Length >= 247)
                    continue; // Abort

                Directory.CreateDirectory(path);

                // Slice for each variable defined in all functions
                var it = new PreOrderDepthFirstProxyIterator(file, true);
                var methods = it.Enumerable.OfType<ICallableNode>().ToImmutableHashSet();
                foreach (var method in _reachingDefinitions.DefinedVariables)
                {
                    if (methods.Contains(method.Key.Origin) && method.Value.Any() && method.Key.Root != null)
                    {
                        var sanitized = string.Join("_", method.Key.Origin.Signature.Split(Path.GetInvalidFileNameChars()));
                        var end = sanitized.IndexOf('(');
                        if (end == -1)
                        {
                            end = sanitized.LastIndexOf('.');
                            end = sanitized.Substring(0, end - 1).LastIndexOf('.');
                            sanitized = sanitized.Substring(end + 1);
                        }
                        else
                        {
                            sanitized = sanitized.Substring(sanitized.Substring(0, end).LastIndexOf('.') + 1);
                        }

                        var methodPath = $"{path}/{sanitized}";
                        if(methodPath.Length >= 247)
                            continue; // Abort

                        bool directoryCreated = false;

                        foreach (var variable in method.Value)
                        {
                            var startPoint = _reachingDefinitions.VariablesReferences[method.Key][variable].ToImmutableHashSet();
                            if(startPoint.IsEmpty)
                                continue;

                            // Just for demo: Filter out for on output only 
                            var filtered = startPoint.Where(x => x.Origin.ToCode.Contains("Console.WriteLine(")).ToImmutableHashSet();

                            if (!filtered.IsEmpty)
                            {
                                startPoint = filtered;
                            }

                            var outputFile = methodPath + "/" + variable + ".cs";
                            if (outputFile.Length >= 259)
                                continue; // Abort

                            if (!directoryCreated)
                            {
                                directoryCreated = true;
                                Directory.CreateDirectory(methodPath);
                            }
                            
                            using (TextWriter fs = new StreamWriter(outputFile))
                            {
                                fs.WriteLine(Slice(method.Key, startPoint));
                            }
                        }
                    }
                }
            }
        }


        public string Slice(CfgMethod method, IEnumerable<CfgNode> starts)
        {
            var file = method.Origin.Ancestors().OfType<FileNode>().First();
            var allNodes = method.Root.NodeEnumerator.ToImmutableHashSet();

            var depedencies = new Dictionary<CfgNode, List<CfgNode>>();
            foreach (var cfgLink in _programDepedencies[method])
            {
                if (!depedencies.ContainsKey(cfgLink.To))
                {
                    depedencies[cfgLink.To] = new List<CfgNode>();
                }

                depedencies[cfgLink.To].Add(cfgLink.From);
            }

            var nodesToKeep = new HashSet<CfgNode>();

            var stack = new Stack<CfgNode>();
            foreach (var start in starts)
            {
                stack.Push(start);
            }

            while (stack.Any())
            {
                var current = stack.Pop();
                nodesToKeep.Add(current);

                if (depedencies.ContainsKey(current))
                {
                    foreach (var depp in depedencies[current].Where(x => !nodesToKeep.Contains(x)))
                    {
                        stack.Push(depp);
                        nodesToKeep.Add(depp);
                    }
                }
            }

            var nodesToRemove = allNodes.Except(nodesToKeep).Where(x => x.Origin != null).Select(x => x.Origin as IProxyNode).ToImmutableHashSet();

            return file.ToCodeFiltered(nodesToRemove);
        }

        public string Name => GetType().Name;

        public IList<string> Depedencies => new List<string> { CSA.Artifacts.Ast, Artifacts.ReachingDefinitions, Artifacts.ProgramDepedencies };
    }
}
