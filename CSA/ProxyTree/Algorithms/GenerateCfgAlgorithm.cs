using System.Collections.Generic;
using System.Linq;
using CSA.CFG.Nodes;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.ProxyTree.Visitors;
using Microsoft.CodeAnalysis.CSharp;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class GenerateCfgAlgorithm : IProxyAlgorithm
    {
        private readonly IProxyNode _forest;
        private readonly CfgGraph _cfgGraph;

        public GenerateCfgAlgorithm(
            [Named("Root")] IProxyNode forest,
            [Named("CFG")] CfgGraph cfgGraph)
        {
            _forest = forest;
            _cfgGraph = cfgGraph;
        }

        public string Name => GetType().Name;

        public void Execute()
        {
            var methodIterator = new PreOrderDepthFirstProxyIterator(_forest, true);

            // On each method
            foreach (var node in methodIterator.Enumerable.OfType<ICallableNode>())
            {
                // Set root
                var method = SetRoot(node);

                // Compute next
                var it = new PreOrderDepthFirstProxyIterator(node, false);
                var cfgVisitor = new GenerateCfgVisitor(_cfgGraph, method);
                foreach (var statementNode in it.Enumerable.OfType<StatementNode>())
                {
                    statementNode.Accept(cfgVisitor);
                }

                if (method.Root != null)
                {
                    // Compute prec
                    foreach (var link in method.Root.LinkEnumerator)
                    {
                        link.To.Prec.Add(link.From);
                    }

                    // Clean not needed nodes
                    RemoveBlocks(method.Root);
                }
            }
        }

        private CfgMethod SetRoot(ICallableNode node)
        {
            var root = node.Childs.OfType<StatementNode>().FirstOrDefault();
            var cfgRoot = root != null ? _cfgGraph.GetCfgNode(root) : null;
            CfgNode cfgExit = null;
            if (cfgRoot != null)
            {
                var begin = new CfgNode("Entry");
                begin.Next.Add(cfgRoot);
                cfgExit = new CfgNode("Exit");
                cfgRoot.Next.Add(cfgExit);

                cfgRoot = begin;
            }

            var method = new CfgMethod(node, cfgRoot, cfgExit);
            _cfgGraph.CfgMethods[node.Signature] = method;
            return method;
        }

        private void RemoveBlocks(CfgNode root)
        {
            // Now we will remove the block nodes
            var blocks = root.NodeEnumerator.Where(x => x.Kind == SyntaxKind.Block).ToList();
            foreach (var node in blocks)
            {
                var nexts = node.Next;
                var precs = node.Prec;
                foreach (var prec in precs)
                {
                    prec.Next.UnionWith(nexts);
                    prec.Next.Remove(node);
                }
                foreach (var next in nexts)
                {
                    next.Prec.UnionWith(precs);
                    next.Prec.Remove(node);
                }
            }
        }
    }
}