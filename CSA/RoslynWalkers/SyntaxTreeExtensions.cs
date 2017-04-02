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

        public static IProxyNode GenerateProxy(this SyntaxTree tree, SemanticModel model, Document doc)
        {
            var writer = Program.Kernel.Get<ProxyTreeBuildWalker>();
            Program.Kernel.Bind<SemanticModel>().ToConstant(model);
            Program.Kernel.Bind<Document>().ToConstant(doc);
            writer.Visit(tree.GetRoot());
            Program.Kernel.Unbind<Document>();
            Program.Kernel.Unbind<SemanticModel>();
            return writer.Root;
        }
    }
}