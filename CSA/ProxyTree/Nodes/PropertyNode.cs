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
        }

        public override void Accept(IProxyAlgorithm algorithm) => algorithm.Apply(this);

        public string Signature
        {
            get
            {
                switch (Kind)
                {
                    case SyntaxKind.PropertyDeclaration:
                    {
                        var origin = Origin as PropertyDeclarationSyntax;
                        Debug.Assert(origin != null, "origin != null");
                        return origin.Identifier.ToString();
                    }
                    case SyntaxKind.IndexerDeclaration:
                    {
                        var origin = Origin as IndexerDeclarationSyntax;
                        Debug.Assert(origin != null, "origin != null");
                        return origin.ThisKeyword.ToString() + origin.ParameterList;
                    }
                }

                throw new InvalidOperationException();
            }
        }

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