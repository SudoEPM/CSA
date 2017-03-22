using CSA.ProxyTree.Nodes.Interfaces;
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

        public static IProxyNode GenerateProxy(this SyntaxTree tree, SemanticModel model)
        {
            var writer = Program.Kernel.Get<ProxyTreeBuildWalker>();
            Program.Kernel.Bind<SemanticModel>().ToConstant(model);
            writer.Visit(tree.GetRoot());
            Program.Kernel.Unbind<SemanticModel>();
            return writer.Root;
        }
    }
}