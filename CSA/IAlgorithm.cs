using System.Collections.Generic;

namespace CSA
{
    public abstract class Artifacts
    {
        public static string ParseTree => "ParseTree";
        public static string Ast => "AST";
        public static string Cfg => "CFG";
        public static string DefUse => "DefUse";
        public static string ReachingDefinitions => "ReachingDefinitions";
        public static string LiveVariables => "LiveVariables";
    }

    public interface IProduceArtefactsAlgorithm : IAlgorithm
    {
        IList<string> Artifacts { get; }
    }

    public interface IAlgorithm
    {
        IList<string> Depedencies { get; }
        string Name { get; }
        void Execute();
    }
}