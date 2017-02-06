using System;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Algorithms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes
{
    public class MethodNode : BasicProxyNode
    {
        public MethodNode(SyntaxNode origin) : base(origin)
        {
            var baseNode = origin as BaseMethodDeclarationSyntax;
            Debug.Assert(baseNode != null, "baseNode != null");
            Protection = FindProtection(baseNode.Modifiers);

            switch (Kind)
            {
                case SyntaxKind.ConstructorDeclaration:
                    {
                        var node = Origin as ConstructorDeclarationSyntax;
                        Debug.Assert(node != null, "node != null");
                        Name = node.Identifier.ToString();
                        Parameters = node.ParameterList.ToString();
                        ReturnType = "";
                    }
                    break;
                case SyntaxKind.MethodDeclaration:
                    {
                        var node = Origin as MethodDeclarationSyntax;
                        Debug.Assert(node != null, "node != null");
                        Name = node.Identifier.ToString();
                        Parameters = node.ParameterList.ToString();
                        ReturnType = node.ReturnType.ToString();
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public override void Accept(IProxyAlgorithm algorithm) => algorithm.Apply(this);

        public string Name { get; }

        public string Parameters { get; }

        public string ReturnType { get; }
        public string Protection { get; }

        public string Signature => Name + Parameters;

        private string FindProtection(SyntaxTokenList modifiers)
        {
            try
            {
                var modifier = modifiers.First(x =>
                    x.Kind() == SyntaxKind.PublicKeyword ||
                    x.Kind() == SyntaxKind.PrivateKeyword ||
                    x.Kind() == SyntaxKind.ProtectedKeyword ||
                    x.Kind() == SyntaxKind.InternalKeyword);
                return modifier.ToString();
            }
            catch (InvalidOperationException)
            {
                return "private";
            }
        }
    }
}