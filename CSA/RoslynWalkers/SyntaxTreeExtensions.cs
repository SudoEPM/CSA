using CSA.ProxyTree.Nodes;
using Microsoft.CodeAnalysis;
using Ninject;

namespace CSA.RoslynWalkers
{
    public static class SyntaxTreeExtensions
    {
        public static void Dump(this SyntaxTree tree)
        {
            var writer = Program.Kernel.Get<ConsoleDumpWalker>();
            writer.Visit(tree.GetRoot());
        }

        public static IProxyNode GenerateProxy(this SyntaxTree tree)
        {
            var writer = Program.Kernel.Get<ProxyTreeBuildWalker>();
            writer.Visit(tree.GetRoot());
            return writer.Root;
        }
    }
}