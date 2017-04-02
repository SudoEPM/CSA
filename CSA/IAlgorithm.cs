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
        public static string DomTree => "DomTree";
        public static string PostDomTree => "PostDomTree";
        public static string ControlDepedencies => "ControlDepedencies";
        public static string DataDepedencies => "DataDepedencies";
        public static string ProgramDepedencies => "ProgramDepedencies";
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