using System;
using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
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

            var interfaceType = typeof(IProxyNode).Name;
            foreach (var type in _mapTypes)
            {
                if (type.Value.GetInterface(interfaceType) == null)
                {
                    Console.Error.WriteLine(type.Value.Name + " has been mapped has a node, but is not a child of " + interfaceType);
                }
            }
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
                    nodeType = typeof (StatementNode);
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