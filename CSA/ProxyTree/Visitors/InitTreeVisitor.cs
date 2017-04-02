using System;
using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.ProxyTree.Nodes.Statements;
using CSA.ProxyTree.Visitors.Standards;
using Microsoft.CodeAnalysis.CSharp;
using Ninject;

namespace CSA.ProxyTree.Visitors
{
    class InitTreeVisitor : ApplyDefaultVisitor
    {
        private ClassNode _currentClass;
        private Dictionary<string, LabelStatementNode> _pendingTargets;
        private HashSet<GotoStatementNode> _pendingGotos;
        private PropertyNode _currentProperty;
        private readonly List<Func<IProxyNode, bool>> _filterRules;
        private readonly IDictionary<string, ClassNode> _classMapping;

        public InitTreeVisitor([Named("ClassMapping")] IDictionary<string, ClassNode> classMapping)
        {
            _classMapping = classMapping;

            _filterRules = new List<Func<IProxyNode, bool>>
            {
                node => !node.GetType().IsSubclassOf(typeof (BasicProxyNode)),
                node => (node is ExpressionNode) && _currentClass == null
            };
        }

        public override void Apply(IProxyNode node)
        {
            if (TrimTree && _filterRules.Any(x => x(node)))
            {
                // Thoses nodes are not concrete and should be filtered out

                // Break the relation
                var ndx = node.Parent.Childs.FindIndex(x => x == node);
                node.Parent.Childs.RemoveAt(ndx);
                node.Parent.Childs.InsertRange(ndx, node.Childs);
                node.Childs.ForEach(x => x.Parent = node.Parent);
            }
            // We keep theses nodes
            else
            {
                node.ClassSignature = _currentClass.Signature;
            }
        }

        public bool TrimTree => true;

        public override void Apply(ForestNode node)
        {
            // Nothing to do
        }

        public override void Apply(FileNode node)
        {
            // Nothing to do
        }

        public override void Apply(ClassNode node)
        {
            _currentClass = node;

            Apply((IProxyNode)node);

            _classMapping[node.Signature] = node;
        }

        public override void Apply(MethodNode node)
        {
            node.Namespace = _currentClass.Namespace;
            _pendingTargets = new Dictionary<string, LabelStatementNode>();
            _pendingGotos = new HashSet<GotoStatementNode>();

            Apply((IProxyNode)node);
        }

        public override void Apply(PropertyNode node)
        {
            _currentProperty = node;
            if (node.Protection == "")
            {
                node.Protection = _currentClass.IsInterface ? "public" : "private";
            }

            Apply((IProxyNode)node);
        }

        public override void Apply(PropertyAccessorNode node)
        {
            node.Namespace = _currentClass.Namespace;

            node.Type = _currentProperty.Type;
            if (node.Protection == "")
            {
                node.Protection = _currentProperty.Protection;
            }

            node.Name = _currentProperty.Signature;

            Apply((IProxyNode)node);
        }

        public override void Apply(BreakStatementNode node)
        {
            var link = node.Ancestors().First(parent => parent.Kind == SyntaxKind.SwitchStatement
                                        || parent.Kind == SyntaxKind.WhileStatement
                                        || parent.Kind == SyntaxKind.DoStatement
                                        || parent.Kind == SyntaxKind.ForStatement
                                        || parent.Kind == SyntaxKind.ForEachStatement);
            node.Link = (StatementNode)link;

            Apply((IProxyNode)node);
        }

        public override void Apply(ContinueStatementNode node)
        {
            var link = node.Ancestors().First(parent => parent.Kind == SyntaxKind.SwitchStatement
                                        || parent.Kind == SyntaxKind.WhileStatement
                                        || parent.Kind == SyntaxKind.DoStatement
                                        || parent.Kind == SyntaxKind.ForStatement
                                        || parent.Kind == SyntaxKind.ForEachStatement);
            node.Link = (StatementNode)link;

            Apply((IProxyNode)node);
        }

        public override void Apply(GotoStatementNode node)
        {
            _pendingGotos.Add(node);
            MatchGotos();

            Apply((IProxyNode)node);
        }

        public override void Apply(GotoCaseStatementNode node)
        {
            var switchStatement = node.Ancestors().First(parent => parent.Kind == SyntaxKind.SwitchStatement);
            var switchSectionStatementNode = switchStatement.Childs.OfType<SwitchSectionStatementNode>().First(x => x.Labels.Contains(node.Case));
            node.Target = switchSectionStatementNode;

            Apply((IProxyNode)node);
        }

        public override void Apply(LabelStatementNode node)
        {
            _pendingTargets[node.Label] = node;
            MatchGotos();

            Apply((IProxyNode)node);
        }

        public override void Apply(TryStatementNode node)
        {
            node.Catchs.UnionWith(node.Childs.OfType<CatchStatementNode>());
            node.Finally = (FinallyStatementNode)node.Childs.FirstOrDefault(x => x is FinallyStatementNode);
            node.Content = node.Childs.OfType<StatementNode>().First();

            Apply((IProxyNode)node);
        }

        private void MatchGotos()
        {
            foreach (var target in _pendingTargets)
            {
                var gotos = _pendingGotos.Where(x => x.Label == target.Key).ToList();
                foreach (var node in gotos)
                {
                    node.Target = target.Value;
                }
                _pendingGotos.ExceptWith(gotos);
            }
        }
    }
}