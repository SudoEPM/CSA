using System;
using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Nodes.Interfaces;
using Microsoft.CodeAnalysis.CSharp;

namespace CSA.ProxyTree.Nodes
{
    public class ForestNode : IProxyNode
    {
        public ForestNode(List<IProxyNode> childs)
        {
            Childs = childs;
            // We will ensure that the childs are correctly linked...
            foreach (var child in childs)
            {
                child.Parent = this;
            }
        }

        public List<IProxyNode> Childs { get; }

        public SyntaxKind Kind => SyntaxKind.None;
        public IProxyNode Parent
        {
            get { return null; }
            set { throw new System.InvalidOperationException("Impossible to set the parent of a root"); }
        }

        public IProxyNode Left
        {
            get { return null; }
            set { throw new InvalidOperationException("Impossible to set the brother of a root"); }
        }
        public IProxyNode Right
        {
            get { return null; }
            set { throw new InvalidOperationException("Impossible to set the brother of a root"); }
        }

        public string ClassSignature
        {
            get
            {
                throw new InvalidOperationException("The root is in no class.");
            }
            set
            {  
                throw new InvalidOperationException("The root is in no class.");
            }
        }

        public string FileName
        {
            get
            {
                throw new InvalidOperationException("The root has no file attached.");
            }
        }

        public IEnumerable<IProxyNode> Ancestors() => Enumerable.Empty<IProxyNode>();

        public void Accept(IProxyAlgorithm algorithm) => algorithm.Apply(this);
    }
}