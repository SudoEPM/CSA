using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Nodes.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes
{
    public class MethodNode : BasicProxyNode, ICallableNode
    {
        public MethodNode(SyntaxNode origin) : base(origin)
        {
            var baseNode = origin as BaseMethodDeclarationSyntax;
            Debug.Assert(baseNode != null, "baseNode != null");
            Protection = FindProtection(baseNode.Modifiers, "private");

            switch (Kind)
            {
                case SyntaxKind.ConstructorDeclaration:
                    {
                        var node = Origin as ConstructorDeclarationSyntax;
                        Debug.Assert(node != null, "node != null");
                        Name = node.Identifier.ToString();
                        Parameters = node.ParameterList.Parameters.Select(x => new Tuple<string, string>(x.Type.ToString(), x.Identifier.ToString())).ToList();
                        Type = "";
                    }
                    break;
                case SyntaxKind.MethodDeclaration:
                    {
                        var node = Origin as MethodDeclarationSyntax;
                        Debug.Assert(node != null, "node != null");
                        Name = node.Identifier.ToString();
                        Parameters = node.ParameterList.Parameters.Select(x => new Tuple<string, string>(x.Type.ToString(), x.Identifier.ToString())).ToList();
                        Type = node.ReturnType.ToString();
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public override void Accept(IProxyAlgorithm algorithm) => algorithm.Apply(this);

        public string Name { get; }

        public List<Tuple<string, string>> Parameters { get; }

        public string Protection { get; }

        public string Signature => Name + "(" + string.Join(", ", Parameters) + ")";
        public string Type { get; }
    }
}