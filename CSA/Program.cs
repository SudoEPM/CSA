using System;
using System.Linq;
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
    }
}
