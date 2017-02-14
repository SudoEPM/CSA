using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                var origin = Origin as BaseTypeDeclarationSyntax;
                Debug.Assert(origin != null, "origin != null");
                return origin.Identifier.ToString();
            }
        }

        public IEnumerable<string> BaseTypes
        {
            get
            {
                var origin = Origin as BaseTypeDeclarationSyntax;
                Debug.Assert(origin != null, "origin != null");
                return origin.BaseList?.Types.Select(x => x.Type.ToString()) ?? Enumerable.Empty<string>();
            }
        }
    }
}