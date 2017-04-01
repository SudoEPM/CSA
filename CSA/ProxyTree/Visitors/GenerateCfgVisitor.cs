using System.Collections.Generic;
using System.Linq;
using CSA.CFG.Nodes;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Statements;
using CSA.ProxyTree.Visitors.Standards;
using Microsoft.CodeAnalysis.CSharp;
using Ninject;

namespace CSA.ProxyTree.Visitors
{
    public class GenerateCfgVisitor : DoNothingVisitor
    {
        private readonly CfgGraph _cfgGraph;
        private readonly CfgMethod _current;

        internal GenerateCfgVisitor([Named("CFG")] CfgGraph cfgGraph, CfgMethod current)
        {
            _cfgGraph = cfgGraph;
            _current = current;
        }

        public override void Apply(StatementNode node)
        {
            // Nothing to do
        }

        public override void Apply(IfStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);
            var nexts = new HashSet<CfgNode>(cfgNode.Next);

            var cfgInsideIf = _cfgGraph.GetCfgNode(node.Childs.OfType<StatementNode>().First());
            cfgNode.Next.Add(cfgInsideIf);
            cfgInsideIf.Next.UnionWith(nexts);
        }

        public override void Apply(IfElseStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);
            var nexts = new HashSet<CfgNode>(cfgNode.Next);

            var cfgInsideIf = _cfgGraph.GetCfgNode(node.Childs.OfType<StatementNode>().First());
            var cfgInsideElse = _cfgGraph.GetCfgNode(node.Childs.OfType<StatementNode>().Last());

            cfgNode.Next.Clear();
            cfgNode.Next.Add(cfgInsideIf);
            cfgNode.Next.Add(cfgInsideElse);

            cfgInsideIf.Next.UnionWith(nexts);
            cfgInsideElse.Next.UnionWith(nexts);
        }

        public override void Apply(BlockStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);
            var currNext = cfgNode.Next;
            foreach (var child in Enumerable.Reverse(node.Childs).OfType<StatementNode>())
            {
                var cfgChild = _cfgGraph.GetCfgNode(child);
                cfgChild.Next.UnionWith(currNext);
                currNext = new HashSet<CfgNode> { cfgChild };
            }

            var first = node.Childs.OfType<StatementNode>().FirstOrDefault();
            if (first != null)
            {
                var cfgFirst = _cfgGraph.GetCfgNode(first);
                cfgNode.Next.Clear();
                cfgNode.Next.Add(cfgFirst);
            }
        }

        public override void Apply(SwitchStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);

            var nexts = new HashSet<CfgNode>();
            var hasDefault = false;
                
            foreach (var section in node.Childs.OfType<SwitchSectionStatementNode>())
            {
                var cfgSection = _cfgGraph.GetCfgNode(section);
                nexts.Add(cfgSection);
                cfgSection.Next.UnionWith(cfgNode.Next);
                if (section.Labels.Any(x => x == "default:"))
                {
                    hasDefault = true;
                }
            }

            if (hasDefault)
            {
                cfgNode.Next.Clear();
            }
            cfgNode.Next.UnionWith(nexts);
        }

        public override void Apply(SwitchSectionStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);
            var currNext = cfgNode.Next;
            foreach (var child in Enumerable.Reverse(node.Childs).OfType<StatementNode>())
            {
                var cfgChild = _cfgGraph.GetCfgNode(child);
                cfgChild.Next.UnionWith(currNext);
                currNext = new HashSet<CfgNode> { cfgChild };
            }

            var first = node.Childs.OfType<StatementNode>().FirstOrDefault();
            if (first != null)
            {
                var cfgFirst = _cfgGraph.GetCfgNode(first);
                cfgNode.Next.Clear();
                cfgNode.Next.Add(cfgFirst);
            }
        }

        public override void Apply(BreakStatementNode node)
        {
            if (node.Link.Kind == SyntaxKind.SwitchStatement)
            {
                return;
            }

            var cfgNode = _cfgGraph.GetCfgNode(node);
            var cfgLoop = _cfgGraph.GetCfgNode(node.Link);
            var cfgInsideWhile = _cfgGraph.GetCfgNode(cfgLoop.Origin.Childs.OfType<StatementNode>().First());

            cfgNode.Next.Clear();
            cfgNode.Next.UnionWith(cfgLoop.Next);
            cfgNode.Next.Remove(cfgInsideWhile);
        }

        public override void Apply(ContinueStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);
            var cfgLoop = _cfgGraph.GetCfgNode(node.Link);

            cfgNode.Next.Clear();
            cfgNode.Next.Add(cfgLoop);
        }

        public override void Apply(YieldBreakStatementNode node)
        {
            // It's like a return
            var cfgNode = _cfgGraph.GetCfgNode(node);
            cfgNode.Next.Clear();
            cfgNode.Next.Add(_current.Exit);
        }

        public override void Apply(YieldReturnStatementNode node)
        {
            // We return the value to the caller, but we continue in the method too
            var cfgNode = _cfgGraph.GetCfgNode(node);
            cfgNode.Next.Add(_current.Exit);
        }

        public override void Apply(ReturnStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);
            cfgNode.Next.Clear();
            cfgNode.Next.Add(_current.Exit);
        }

        public override void Apply(WhileStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);

            var cfgInsideWhile = _cfgGraph.GetCfgNode(node.Childs.OfType<StatementNode>().First());

            cfgNode.Next.Add(cfgInsideWhile);

            cfgInsideWhile.Next.Clear();
            cfgInsideWhile.Next.Add(cfgNode);
        }

        public override void Apply(DoStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);

            var cfgInsideDo = _cfgGraph.GetCfgNode(node.Childs.OfType<StatementNode>().First());

            cfgNode.Next.Add(cfgInsideDo);

            cfgInsideDo.Next.Clear();
            cfgInsideDo.Next.Add(cfgNode);
        }

        public override void Apply(ForStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);

            var cfgInsideFor = _cfgGraph.GetCfgNode(node.Childs.OfType<StatementNode>().First());

            cfgNode.Next.Add(cfgInsideFor);

            cfgInsideFor.Next.Clear();
            cfgInsideFor.Next.Add(cfgNode);
        }

        public override void Apply(ForEachStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);

            var cfgInsideForEach = _cfgGraph.GetCfgNode(node.Childs.OfType<StatementNode>().First());

            cfgNode.Next.Add(cfgInsideForEach);

            cfgInsideForEach.Next.Clear();
            cfgInsideForEach.Next.Add(cfgNode);
        }

        public override void Apply(GotoStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);
            var cfgLabel = _cfgGraph.GetCfgNode(node.Target);

            cfgNode.Next.Clear();
            cfgNode.Next.Add(cfgLabel);
        }

        public override void Apply(GotoCaseStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);
            var cfgCase = _cfgGraph.GetCfgNode(node.Target);

            cfgNode.Next.Clear();
            cfgNode.Next.Add(cfgCase);
        }

        public override void Apply(LabelStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);

            var cfgInsideLabel = _cfgGraph.GetCfgNode(node.Childs.OfType<StatementNode>().First());

            cfgInsideLabel.Next.UnionWith(cfgNode.Next);

            cfgNode.Next.Clear();
            cfgNode.Next.Add(cfgInsideLabel);
        }

        public override void Apply(TryStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);

            // Content
            if (!node.Content.Childs.Any())
            {
                // The try is empty, just jump
                return;
            }

            var nexts = new HashSet<CfgNode>(cfgNode.Next);

            var cfgContent = _cfgGraph.GetCfgNode(node.Content);
            cfgNode.Next.Clear();
            cfgNode.Next.Add(cfgContent);
            cfgContent.Next.UnionWith(nexts);

            // Catch
            var cfgCatchs = node.Catchs.OfType<StatementNode>().Select(x => _cfgGraph.GetCfgNode(x)).ToList();
            cfgNode.Next.UnionWith(cfgCatchs);
            cfgCatchs.ForEach(x => x.Next.UnionWith(nexts));

            // Finally
            if (node.Finally != null)
            {
                cfgNode.Next.Add(_cfgGraph.GetCfgNode(node.Finally));
            }
        }

        public override void Apply(CatchStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);
            var cfgInside = _cfgGraph.GetCfgNode(node.Childs.OfType<StatementNode>().First());

            cfgInside.Next.UnionWith(cfgNode.Next);

            cfgNode.Next.Clear();
            cfgNode.Next.Add(cfgInside);
        }

        public override void Apply(ThrowStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);
            cfgNode.Next.Clear();

            // It's a bad management, but...
            cfgNode.Next.Add(_current.Exit);
        }

        public override void Apply(FinallyStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);
            cfgNode.Next.Add(_cfgGraph.GetCfgNode(node.Childs.OfType<StatementNode>().First()));
        }

        public override void Apply(UsingStatementNode node)
        {
            var cfgNode = _cfgGraph.GetCfgNode(node);

            // Content
            var cfgContent = _cfgGraph.GetCfgNode(node.Childs.OfType<StatementNode>().First());
            cfgContent.Next.UnionWith(cfgNode.Next);

            cfgNode.Next.Clear();
            cfgNode.Next.Add(cfgContent);
        }
    }
}