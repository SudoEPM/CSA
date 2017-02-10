using System;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Algorithms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes
{
    public class PropertyNode : BasicProxyNode
    {
        public PropertyNode(SyntaxNode origin) : base(origin)
        {
            var baseOrigin = origin as BasePropertyDeclarationSyntax;
            Debug.Assert(baseOrigin != null, "baseOrigin != null");
            Protection = FindProtection(baseOrigin.Modifiers);

            switch (Kind)
            {
                case SyntaxKind.PropertyDeclaration:
                    {
                        var org = Origin as PropertyDeclarationSyntax;
                        Debug.Assert(org != null, "org != null");
                        Signature = org.Identifier.ToString();
                    }
                    break;
                case SyntaxKind.IndexerDeclaration:
                    {
                        var org = Origin as IndexerDeclarationSyntax;
                        Debug.Assert(org != null, "org != null");
                        Signature = org.ThisKeyword.ToString() + org.ParameterList;
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public override void Accept(IProxyAlgorithm algorithm) => algorithm.Apply(this);

        public string Signature { get; }

        public string Type
        {
            get
            {
                var origin = Origin as BasePropertyDeclarationSyntax;
                Debug.Assert(origin != null, "origin != null");
                return origin.Type.ToString();
            }
        }

        public string Protection { get; }

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