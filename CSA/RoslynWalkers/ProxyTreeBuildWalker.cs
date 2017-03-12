using System;
using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.ProxyTree.Nodes.Statements;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Ninject;

namespace CSA.RoslynWalkers
{
    internal class ProxyTreeBuildWalker : SyntaxWalker
    {
        public IProxyNode Root { get; private set; }
        private readonly Dictionary<SyntaxNode, IProxyNode> _mapNodesConversion = new Dictionary<SyntaxNode, IProxyNode>();

        private readonly Dictionary<SyntaxKind, Type> _mapTypes;

        public ProxyTreeBuildWalker(Dictionary<SyntaxKind, Type> mapTypes)
        {
            _mapTypes = mapTypes;
            _mapTypes[SyntaxKind.MethodDeclaration] = typeof(MethodNode);
            _mapTypes[SyntaxKind.ConstructorDeclaration] = typeof(MethodNode);
            _mapTypes[SyntaxKind.GetAccessorDeclaration] = typeof(PropertyAccessorNode);
            _mapTypes[SyntaxKind.SetAccessorDeclaration] = typeof(PropertyAccessorNode);
            _mapTypes[SyntaxKind.PropertyDeclaration] = typeof(PropertyNode);
            _mapTypes[SyntaxKind.IndexerDeclaration] = typeof(PropertyNode);
            _mapTypes[SyntaxKind.ClassDeclaration] = typeof(ClassNode);
            _mapTypes[SyntaxKind.StructDeclaration] = typeof(ClassNode);
            _mapTypes[SyntaxKind.InterfaceDeclaration] = typeof(ClassNode);
            _mapTypes[SyntaxKind.FieldDeclaration] = typeof(FieldNode);
            _mapTypes[SyntaxKind.Block] = typeof(BlockStatementNode);
            _mapTypes[SyntaxKind.SwitchStatement] = typeof(SwitchStatementNode);
            _mapTypes[SyntaxKind.SwitchSection] = typeof(SwitchSectionStatementNode);
            _mapTypes[SyntaxKind.BreakStatement] = typeof(BreakStatementNode);
            _mapTypes[SyntaxKind.ContinueStatement] = typeof(ContinueStatementNode);
            _mapTypes[SyntaxKind.YieldBreakStatement] = typeof(YieldBreakStatementNode);
            _mapTypes[SyntaxKind.YieldReturnStatement] = typeof(YieldReturnStatementNode);
            _mapTypes[SyntaxKind.ReturnStatement] = typeof(ReturnStatementNode);
            _mapTypes[SyntaxKind.WhileStatement] = typeof(WhileStatementNode);
            _mapTypes[SyntaxKind.DoStatement] = typeof(DoStatementNode);
            _mapTypes[SyntaxKind.ForStatement] = typeof(ForStatementNode);
            _mapTypes[SyntaxKind.ForEachStatement] = typeof(ForEachStatementNode);
            _mapTypes[SyntaxKind.GotoStatement] = typeof(GotoStatementNode);
            _mapTypes[SyntaxKind.GotoDefaultStatement] = typeof(GotoCaseStatementNode);
            _mapTypes[SyntaxKind.GotoCaseStatement] = typeof(GotoCaseStatementNode);
            _mapTypes[SyntaxKind.LabeledStatement] = typeof(LabelStatementNode);
            _mapTypes[SyntaxKind.TryStatement] = typeof(TryStatementNode);
            _mapTypes[SyntaxKind.CatchClause] = typeof(CatchStatementNode);
            _mapTypes[SyntaxKind.ThrowStatement] = typeof(ThrowStatementNode);
            _mapTypes[SyntaxKind.FinallyClause] = typeof(FinallyStatementNode);
        }

        public override void Visit(SyntaxNode node)
        {
            Type nodeType;
            // Check if there's a direct mapping
            if (_mapTypes.ContainsKey(node.Kind()))
            {
                nodeType = _mapTypes[node.Kind()];
            }
            else
            {
                // Check if we can convert it to a generic statement or expression
                if (node is StatementSyntax)
                {
                    var ifNode = node as IfStatementSyntax;
                    if (ifNode != null)
                    {
                        nodeType = ifNode.Else != null ? typeof(IfElseStatementNode) : typeof(IfStatementNode);
                    }
                    else
                    {
                       nodeType = typeof (StatementNode);
                    }
                }
                else if (node is ExpressionSyntax)
                {
                    nodeType = typeof(ExpressionNode);
                }
                else
                {
                    nodeType = typeof (BasicProxyNode);
                }
            }

            Program.Kernel.Bind<SyntaxNode>().ToMethod(x => node);
            var curr = (IProxyNode) Program.Kernel.Get(nodeType);
            Program.Kernel.Unbind<SyntaxNode>();

            if (!node.Ancestors().Any())
            {
                Root = curr;
            }
            else
            {
                var parent = _mapNodesConversion[node.Parent];
                curr.Parent = parent;
                parent.Childs.Add(curr);
            }

            _mapNodesConversion[node] = curr; 
            base.Visit(node);
        }

    }
}