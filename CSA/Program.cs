using System;
using System.Collections.Generic;
using System.Linq;
using CSA.Options;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.RoslynWalkers;
using Microsoft.CodeAnalysis.MSBuild;
using Ninject;

namespace CSA
{
    internal class Program
    {
        public static IKernel Kernel { get; private set; }

        private static void Main(string[] args)
        {
            // Parse the command line
            var options = new ProgramOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.Error.WriteLine("Command line options are invalids!");
                return;
            }
            // Prepare the injection container
            Kernel = new StandardKernel(new CsaModule(options));
            Kernel.Bind<ProgramOptions>().ToMethod(x => options);

            // Parse the solution
            var forest = ParseForest(options.TestMode ? options.DebugSolution : options.Solution);
            Kernel.Bind<IProxyNode>().ToMethod(x => forest).Named("Root");
            Console.WriteLine("Parsing Completed");

            // Execute the different algorithms and visits required
            var algorithms = Kernel.GetAll<IAlgorithm>().ToList();
            foreach (var algorithm in algorithms)
            {
                Console.WriteLine($"{algorithm.Name} : Started!");
                var start = Environment.TickCount;
                algorithm.Execute();
                var timeElapsed = (Environment.TickCount - start)/1000.0;
                Console.WriteLine($"{algorithm.Name} : Completed in {timeElapsed:0.###} seconds!");
            }

            Console.WriteLine("Mission Completed");
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
                        doc => doc.GetSyntaxTreeAsync().Result.GenerateProxy()));
            }
            return new ForestNode(forest);
        }
    }
}
