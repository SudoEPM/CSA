using System.Collections.Generic;
using System.Linq;
using CSA.CFG.Nodes;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class GenerateCfgAlgorithm : IProxyAlgorithm
    {
        private readonly CfgGraph _cfgGraph;
        private readonly Dictionary<StatementNode, CfgNode> _proxyToCfg; 

        public GenerateCfgAlgorithm(
            [Named("PreOrder")] IProxyIterator iterator,
            [Named("CFG")] CfgGraph cfgGraph)
        {
            _cfgGraph = cfgGraph;

            Iterator = iterator;
            Iterator.NodesToSkip.Add(typeof (ExpressionNode));
            _proxyToCfg = new Dictionary<StatementNode, CfgNode>();
        }

        public IProxyIterator Iterator { get; }
        public string Name => GetType().Name;

        public void Apply(MethodNode node)
        {
            var root = node.Childs.OfType<StatementNode>().FirstOrDefault();
            var cfgRoot = root != null ? GetCfgNode(root) : null;
            _cfgGraph.CfgMethods[node.Signature] = new CfgMethod(node, cfgRoot);
        }

        public void Apply(PropertyAccessorNode node)
        {
            var root = node.Childs.OfType<StatementNode>().FirstOrDefault();
            var cfgRoot = root != null ? GetCfgNode(root) : null;
            _cfgGraph.CfgMethods[node.Signature] = new CfgMethod(node, cfgRoot);
        }

        CfgNode GetCfgNode(StatementNode node)
        {
            if (_proxyToCfg.ContainsKey(node))
                return _proxyToCfg[node];

            var cfgNode = new CfgNode(node);
            _proxyToCfg[node] = cfgNode;
            return cfgNode;
        }

        public void Apply(StatementNode node)
        {
            if (node != null)
            {
                int test = 0;
            }
            else if (node.Equals("wow"))
            {
                int w = 0;
            }
            else
            {
                int meme = 0;
            }

            var cfgNode = GetCfgNode(node);
            switch (node.Kind)
            {
                case SyntaxKind.Block:
                {
                    var currNext = cfgNode.Next;
                    foreach (var child in Enumerable.Reverse(node.Childs).OfType<StatementNode>())
                    {
                        var cfgChild = GetCfgNode(child);
                        cfgChild.Next.UnionWith(currNext);
                        currNext = new HashSet<CfgNode> {cfgChild};
                    }

                    var currPrec = cfgNode.Prec;
                    foreach (var child in node.Childs.OfType<StatementNode>())
                    {
                        var cfgChild = GetCfgNode(child);
                        cfgChild.Prec.UnionWith(currPrec);
                        currPrec = new HashSet<CfgNode> {cfgChild};
                    }

                    var first = node.Childs.OfType<StatementNode>().FirstOrDefault();
                    if (first != null)
                    {
                        var cfgFirst = GetCfgNode(first);
                        cfgNode.Next.Add(cfgFirst);
                        cfgFirst.Prec.Add(cfgNode);
                    }
                    break;
                }
                case SyntaxKind.IfStatement:
                {

                    var nexts = new HashSet<CfgNode>(cfgNode.Next);

                    var cfgInsideIf = GetCfgNode(node.Childs.OfType<StatementNode>().First());
                    cfgNode.Next.Add(cfgInsideIf);
                    cfgInsideIf.Next.UnionWith(nexts);

                    // Else is present
                    if (node.Childs.OfType<StatementNode>().Count() > 1)
                    {
                        cfgNode.Next.Clear();
                        foreach (var next in nexts)
                        {
                            next.Prec.Remove(cfgNode);
                        }

                        var cfgInsideElse = GetCfgNode(node.Childs.OfType<StatementNode>().Last());
                        cfgNode.Next.Add(cfgInsideElse);
                        cfgInsideElse.Next.UnionWith(nexts);
                    }
                }
                break;
            }
    }

        public void Apply(ForestNode node)
        {
        }

        #region Unused visits
        public void Apply(PropertyNode node)
        {
        }

        public void Apply(IProxyNode node)
        {
        }

        public void Apply(ClassNode node)
        {
        }

        public void Apply(FieldNode node)
        {
        }

        public void Apply(ExpressionNode node)
        {
        }
#endregion
    }
}