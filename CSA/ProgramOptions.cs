using CommandLine;
using CommandLine.Text;

namespace CSA
{
    class ProgramOptions
    {
        [Option('s', "solution", Required = true,
            HelpText = "Solution file to be processed.")]
        public string Solution { get; set; }

        [Option('d', "debug-solution", DefaultValue = "",
            HelpText = "Solution file to be processed in test mode.")]
        public string DebugSolution { get; set; }

        [Option('t', "test", DefaultValue = false,
            HelpText = "Is the project in test mode (use debug solution).")]
        public bool TestMode { get; set; }

        [Option('m', "metrics", DefaultValue = true,
            HelpText = "Compute the metrics of the solution.")]
        public bool ComputeMetrics { get; set; }

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