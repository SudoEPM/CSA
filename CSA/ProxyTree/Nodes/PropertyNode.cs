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
    }
}