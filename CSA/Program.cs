using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSA.Options;
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

            // Don't do this at home, this is kind of a stupid idea....
            foreach (var type in Assembly.GetAssembly(typeof(Program))
                    .GetTypes()
                    .Where(x => !x.IsAbstract && typeof(IProduceArtefactsAlgorithm).IsAssignableFrom(x)))
            {
                Kernel.Bind<IProduceArtefactsAlgorithm>().To(type);
            }

            // Execute the different algorithms and visits required

            // The roots are the algorithms that the user asked for
            var roots = Kernel.GetAll<IAlgorithm>().ToList();

            // The producers are not required, but computes things required by the roots
            var producers = Kernel.GetAll<IProduceArtefactsAlgorithm>().ToList();

            // We do a depedency sort to only compute the good algorithms in a correct order
            var algorithms = TopologicalSort(roots, producers);

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

        private static IEnumerable<IAlgorithm> TopologicalSort(List<IAlgorithm> roots, List<IProduceArtefactsAlgorithm> producers)
        {
            var mapProducers = new Dictionary<string, IAlgorithm>();
            foreach (var producer in producers)
            {
                foreach (var artifact in producer.Artifacts)
                {
                    mapProducers[artifact] = producer;
                }
            }

            var visited = new HashSet<IAlgorithm>();
            var stack = new Stack<IAlgorithm>();

            roots.ForEach(x => stack.Push(x));
            while (stack.Any())
            {
                // Find the current element
                var current = stack.Peek();
                // Find the next elements
                var notVisitedChilds = current.Depedencies.Select(x => mapProducers[x]).Where(x => !visited.Contains(x)).ToList();
                if (notVisitedChilds.Any())
                {
                    notVisitedChilds.ForEach(x => stack.Push(x));
                }
                else
                {
                    if (visited.Contains(current))
                        continue;

                    // Return the current if all childrens have been visited
                    yield return current;
                    visited.Add(current);
                    stack.Pop();
                }
            }
        }
    }
}
