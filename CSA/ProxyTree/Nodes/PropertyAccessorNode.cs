using System.Diagnostics;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes
{
    public class PropertyAccessorNode : BasicProxyNode, ICallableNode
    {
        public PropertyAccessorNode(SyntaxNode origin) : base(origin)
        {
            var node = Origin as AccessorDeclarationSyntax;
            Debug.Assert(node != null, "node != null");
            Protection = FindProtection(node.Modifiers, "");
            IsAutomatic = node.Body == null;
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);

        public string Namespace { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Accessor => ((AccessorDeclarationSyntax) Origin).Keyword.ToString();

        public bool IsAutomatic { get; }

        public string Protection { get; set; }

        public string Signature => $"{Namespace}.{ClassSignature}.{Name}.{Accessor}";
    }
}