using System.Collections.Generic;
using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Iterators;
using Microsoft.CodeAnalysis.CSharp;

namespace CSA.ProxyTree.Nodes
{
    public interface IProxyNode
    {
        List<IProxyNode> Childs { get; }
        SyntaxKind Kind { get; }

        IProxyNode Parent { get; set; }
        IProxyNode Left { get; set; }
        IProxyNode Right { get; set; }
        string ClassSignature { get; }
        string FileName { get; }

        IEnumerable<IProxyNode> Ancestors();
        void Accept(IProxyAlgorithm algorithm);
    }
}