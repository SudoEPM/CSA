using CommandLine;
using CommandLine.Text;

namespace CSA.Options
{
    class ProgramOptions
    {
        [Option('s', "solution", Required = true,
            HelpText = "Solution file to be processed.")]
        public string Solution { get; set; }

        [Option("debug-solution", DefaultValue = "",
            HelpText = "Solution file to be processed in test mode.")]
        public string DebugSolution { get; set; }

        [Option('t', "test", DefaultValue = false,
            HelpText = "Is the project in test mode (use debug solution).")]
        public bool TestMode { get; set; }

        [Option('m', "metrics", DefaultValue = false,
            HelpText = "Compute the metrics of the solution.")]
        public bool ComputeMetrics { get; set; }

        [Option('u', "uml-class", DefaultValue = false,
            HelpText = "Generate the class UML of the solution.")]
        public bool GenerateClassUml { get; set; }

        [Option("uml-package", DefaultValue = false,
            HelpText = "Generate the package UML of the solution.")]
        public bool GeneratePackageUml { get; set; }

        [Option("print-cfg", DefaultValue = false,
            HelpText = "Print the control flow graph of the solution.")]
        public bool PrintCfg { get; set; }

        [Option("doms", DefaultValue = false,
            HelpText = "Compute and print the dominators of the solution.")]
        public bool ComputeDominators { get; set; }

        [Option("post-doms", DefaultValue = false,
            HelpText = "Compute and print the post-dominators of the solution.")]
        public bool ComputePostDominators { get; set; }

        [Option("reach-defs", DefaultValue = false,
            HelpText = "Compute and print the reaching-definitions of the solution.")]
        public bool ComputeReachingDefinitions { get; set; }

        [Option("lives", DefaultValue = false,
            HelpText = "Compute and print the lives variables of the solution.")]
        public bool ComputeLiveVariables { get; set; }
        
        [Option('p', "prec", DefaultValue = -1,
            HelpText = "Choose the precision of the package generation. (-1 for everything)")]
        public int PackageNesting { get; set; }

        [Option("package-include-external", DefaultValue = false,
            HelpText = "Choose to include package external depencies in the UML")]
        public bool PackagesIncludeExternal { get; set; }

        [Option('g', "graphviz-path", DefaultValue = "C:/Program Files (x86)/Graphviz2.38/bin",
            HelpText = "Path of Graphviz, you need it if you want to generate UML.")]
        public string GraphVizPath { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}