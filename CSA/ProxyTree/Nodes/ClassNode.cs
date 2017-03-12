using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes
{
    public class ClassNode : BasicProxyNode
    {
        public ClassNode(SyntaxNode origin) : base(origin)
        {
            var namespaceDeclarationSyntax = (NamespaceDeclarationSyntax) origin.Ancestors().FirstOrDefault(x => x is NamespaceDeclarationSyntax);
            if (namespaceDeclarationSyntax != null)
                Namespace = namespaceDeclarationSyntax.Name.ToString();

            var root = ((CompilationUnitSyntax) origin.Ancestors().First(x => x is CompilationUnitSyntax));
            NamespaceDepedencies = root.ChildNodes().Where(x => x is UsingDirectiveSyntax).Select(x => ((UsingDirectiveSyntax)x).Name.ToString()).ToList();
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);

        public string Namespace { get; }

        public List<string> NamespaceDepedencies { get; }

        public bool IsInterface => Origin is InterfaceDeclarationSyntax;

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