using CommandLine;
using CommandLine.Text;

namespace CSA
{
    class ProgramOptions
    {
        [Option('s', "sol", Required = true,
            HelpText = "Solution file to be processed.")]
        public string Solution { get; set; }

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