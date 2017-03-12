using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes
{
    public class FieldNode : BasicProxyNode
    {
        public FieldNode(SyntaxNode origin) : base(origin)
        {
            var org = Origin as FieldDeclarationSyntax;
            Debug.Assert(org != null, "org != null");
            Protection = FindProtection(org.Modifiers, "private");
            var varDeclaration = org.ChildNodes().First(x => x.Kind() == SyntaxKind.VariableDeclaration) as VariableDeclarationSyntax;
            Debug.Assert(varDeclaration != null, "varDeclaration != null");
            Type = varDeclaration.Type.ToString();
            Variables = varDeclaration.Variables.Select(x => x.Identifier.ToString()).ToList();
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);

        public List<string> Variables { get; }

        public string Signature => string.Join(", ", Variables);

        public string Type { get; }

        public string Protection { get; }
    }
}