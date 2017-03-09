using System.Collections.Generic;
using System.Linq;
using CSA.CFG;
using CSA.CFG.Nodes;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Ninject;

namespace CSA.ProxyTree.Algorithms
{
    class GenerateCfgAlgorithm : IProxyAlgorithm
    {
        private readonly CfgGraph _cfgGraph;

        public GenerateCfgAlgorithm(
            [Named("PreOrder")] IProxyIterator iterator,
            [Named("CFG")] CfgGraph cfgGraph)
        {
            _cfgGraph = cfgGraph;

            Iterator = iterator;
            Iterator.NodesToSkip.Add(typeof (StatementNode));
            Iterator.NodesToSkip.Add(typeof (ExpressionNode));
        }

        public IProxyIterator Iterator { get; }
        public string Name => GetType().Name;

        public void Apply(MethodNode node)
        {
            var root = node.Childs.FirstOrDefault(x => x is StatementNode) as StatementNode;
            var cfgRoot = root != null ? Visit(root) : null;
            _cfgGraph.CfgMethods[node.Signature] = new CfgMethod(node, cfgRoot);
        }

        public void Apply(PropertyAccessorNode node)
        {
            _cfgGraph.CfgMethods[node.Signature] = new CfgMethod(node, null);
            var root = node.Childs.FirstOrDefault(x => x is StatementNode) as StatementNode;
            var cfgRoot = root != null ? Visit(root) : null;
            _cfgGraph.CfgMethods[node.Signature] = new CfgMethod(node, cfgRoot);
        }

        CfgNode Visit(StatementNode node)
        {
            var res = new CfgNode(node);
            if (node.Right != null)
            {
                res.Next.Add(Visit((StatementNode) node.Right));
            }

            return res;
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
        
        public void Apply(StatementNode node)
        {
            switch (node.Kind)
            {
            case SyntaxKind.IfStatement:
                
                break;
            }
        }

        public void Apply(ExpressionNode node)
        {
        }
#endregion
    }
}