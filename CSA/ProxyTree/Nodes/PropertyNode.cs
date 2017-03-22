using System;
using System.Diagnostics;
using CSA.ProxyTree.Visitors.Interfaces;
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
            Protection = FindProtection(baseOrigin.Modifiers, "");

            switch (Kind)
            {
                case SyntaxKind.EventDeclaration:
                    {
                        var org = Origin as EventDeclarationSyntax;
                        Debug.Assert(org != null, "org != null");
                        Signature = org.Identifier.ToString();
                    }
                    break;
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

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);

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

        public string Protection { get; set; }
    }
}