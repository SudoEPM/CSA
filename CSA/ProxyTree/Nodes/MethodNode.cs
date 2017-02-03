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
        }

        public override void Accept(IProxyAlgorithm algorithm) => algorithm.Apply(this);

        public string Signature
        {
            get
            {
                switch (Kind)
                {
                   case SyntaxKind.ConstructorDeclaration:
                    {
                        var origin = Origin as ConstructorDeclarationSyntax;
                        Debug.Assert(origin != null, "origin != null");
                        return origin.Identifier.ToString() + origin.ParameterList;
                    }
                    case SyntaxKind.MethodDeclaration:
                    {
                        var origin = Origin as MethodDeclarationSyntax;
                        Debug.Assert(origin != null, "origin != null");
                        return origin.Identifier.ToString() + origin.ParameterList;
                    }
                    case SyntaxKind.GetAccessorDeclaration:
                    case SyntaxKind.SetAccessorDeclaration:
                    {
                        var origin = Origin as AccessorDeclarationSyntax;
                        Debug.Assert(origin != null, "origin != null");
                        var property = Ancestors().First(x => x is PropertyNode) as PropertyNode;
                        Debug.Assert(property != null, "property != null");
                        return property.Signature + "." + origin.Keyword;
                    }
                }
                throw new InvalidOperationException();
            }
        }
    }
}