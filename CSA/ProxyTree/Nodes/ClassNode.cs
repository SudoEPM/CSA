using System;
using System.Diagnostics;
using CSA.ProxyTree.Algorithms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes
{
    public class ClassNode : BasicProxyNode
    {
        public ClassNode(SyntaxNode origin) : base(origin)
        {
        }

        public override void Accept(IProxyAlgorithm algorithm) => algorithm.Apply(this);

        public string Signature
        {
            get
            {
                switch (Kind)
                {
                    case SyntaxKind.ClassDeclaration:
                    {
                            var origin = Origin as ClassDeclarationSyntax;
                            Debug.Assert(origin != null, "origin != null");
                            return origin.Identifier.ToString();
                    }
                    case SyntaxKind.StructDeclaration:
                    {
                        var origin = Origin as StructDeclarationSyntax;
                        Debug.Assert(origin != null, "origin != null");
                        return origin.Identifier.ToString();
                    }
                    case SyntaxKind.InterfaceDeclaration:
                    {
                        var origin = Origin as InterfaceDeclarationSyntax;
                        Debug.Assert(origin != null, "origin != null");
                        return origin.Identifier.ToString();
                    }
                }

                throw new InvalidOperationException();
            }
        }
    }
}