using System;
using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Iterators;
using CSA.RoslynWalkers;
using Microsoft.CodeAnalysis;
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
            Kernel = new StandardKernel(new CSAModule(options));
            Kernel.Bind<ProgramOptions>().ToMethod(x => options);

            // Parse the solution
            var forest = ParseForest(options.TestMode ? options.DebugSolution : options.Solution);
            Kernel.Bind<IProxyNode>().ToMethod(x => forest).Named("Root");
            Console.WriteLine("Parsing Completed");

            // Execute the different algorithms and visits required
            var algorithms = Kernel.GetAll<IProxyAlgorithm>().ToList();

            foreach (var algorithm in algorithms)
            {
                foreach (var node in algorithm.Iterator.GetEnumerable())
                {
                    node.Accept(algorithm);
                }
            }

            Console.WriteLine("Mission Completed");
        }

        private static IProxyNode ParseForest(string filepath)
        {
            var workspace = MSBuildWorkspace.Create();
            var sol = workspace.OpenSolutionAsync(filepath).Result;
            var forest = new List<IProxyNode>();
            var nbProjectsParsed = 0;
            var projects = sol.Projects.ToList();
            foreach (var proj in projects)
            {
                forest.AddRange(proj.Documents.Select(doc => doc.GetSyntaxTreeAsync().Result.GenerateProxy()));
                nbProjectsParsed++;
                Console.WriteLine(nbProjectsParsed + "/" + projects.Count + " projects have been analyzed!");
            }
            return new ForestNode(forest);
        }
    }

}
