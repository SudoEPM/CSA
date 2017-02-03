using System;
using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Visitors;
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
            Kernel = new StandardKernel(new CSAModule(options));

            // Parse the solution
            var forest = ParseForest(options.Solution);
            Kernel.Bind<List<IProxyNode>>().ToMethod(x => forest);
            Console.WriteLine("Parsing Completed");

            // Execute the different algorithms and visits required
            var visitors = Kernel.GetAll<IProxyVisitor>();
            var algorithms = Kernel.GetAll<IProxyAlgorithm>().ToList();

            foreach (var visitor in visitors)
            {
                foreach (var node in visitor.GetEnumerable())
                {
                    algorithms.ForEach(x => x.Accept(node));
                }
            }
            algorithms.ForEach(x => x.End());

            Console.WriteLine("Mission Completed");
            Console.ReadKey();
        }

        private static List<IProxyNode> ParseForest(string filepath)
        {
            var workspace = MSBuildWorkspace.Create();
            var sol = workspace.OpenSolutionAsync(filepath).Result;
            return (from proj in sol.Projects from doc in proj.Documents select doc.GetSyntaxTreeAsync().Result.GenerateProxy()).ToList();
        }
    }

}
