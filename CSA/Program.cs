using System;
using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree;
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
            Kernel = new StandardKernel(new CSAModule());

            string filepath = @"C:\Dev\Radarr\src\NzbDrone.sln";
            //string filepath = @"C:\Dev\CSA\CSA.sln";
            var forest = ParseForest(filepath);
            Console.WriteLine("Parsing Completed");
            var visitor = Kernel.Get<IProxyVisitor>();
            visitor.Visit(forest);
            Console.WriteLine("Mission Completed");
            Console.ReadKey();
        }

        private static List<SyntaxProxyNode> ParseForest(string filepath)
        {
            var workspace = MSBuildWorkspace.Create();
            var sol = workspace.OpenSolutionAsync(filepath).Result;
            return (from proj in sol.Projects from doc in proj.Documents select doc.GetSyntaxTreeAsync().Result.GenerateProxy()).ToList();
        }
    }

}
