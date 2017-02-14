using System.Diagnostics;
using CSA.ProxyTree.Algorithms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes
{
    public class PropertyAccessorNode : BasicProxyNode
    {
        public PropertyAccessorNode(SyntaxNode origin) : base(origin)
        {
            var node = Origin as AccessorDeclarationSyntax;
            Debug.Assert(node != null, "node != null");
            Protection = FindProtection(node.Modifiers, "");
        }

        public override void Accept(IProxyAlgorithm algorithm) => algorithm.Apply(this);

        public string Name { get; set; }

        public string Type { get; set; }

        public string Accessor => ((AccessorDeclarationSyntax) Origin).Keyword.ToString();

        public string Protection { get; set; }

        public string Signature => Name + "." + Accessor;
    }
}