using CSA.ProxyTree;
using CSA.ProxyTree.Nodes;
using Microsoft.CodeAnalysis;

namespace CSA.RoslynWalkers
{
    public static class SyntaxTreeExtensions
    {
        public static void Dump(this SyntaxTree tree)
        {
            var writer = new ConsoleDumpWalker();
            writer.Visit(tree.GetRoot());
        }

        public static SyntaxProxyNode GenerateProxy(this SyntaxTree tree)
        {
            var writer = new ProxyTreeBuildWalker();
            writer.Visit(tree.GetRoot());
            return writer.Root;
        }
    }
}