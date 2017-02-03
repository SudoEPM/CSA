using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Algorithms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSA.ProxyTree.Nodes
{
    public class BasicProxyNode : IProxyNode
    {
        protected SyntaxNode Origin { get; }

        public IProxyNode Parent { get; set; }
        public IProxyNode Left { get; set; }
        public IProxyNode Right { get; set; }

        public List<IProxyNode> Childs { get; }

        public SyntaxKind Kind => Origin.Kind();

        public BasicProxyNode(SyntaxNode origin)
        {
            Origin = origin;
            Childs = new List<IProxyNode>();
        }

        public string FileName => Origin.SyntaxTree.FilePath;

        public IEnumerable<IProxyNode> Ancestors()
        {
            var curr = Parent;
            while (curr != null)
            {
                yield return curr;
                curr = curr.Parent;
            }
        }

        public virtual void Accept(IProxyAlgorithm algorithm) => algorithm.Apply(this);

        public string ClassSignature
        {
            get
            {
                var classDeclaration = Ancestors().First(x => x is ClassNode) as ClassNode;

                Debug.Assert(classDeclaration != null, "classDeclaration != null");
                return classDeclaration.Signature;
            }
        }
    }
}
