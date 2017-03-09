using System.Collections.Generic;
using CSA.ProxyTree.Algorithms;
using Microsoft.CodeAnalysis.CSharp;

namespace CSA.ProxyTree.Nodes.Interfaces
{
    public interface IProxyNode
    {
        List<IProxyNode> Childs { get; }
        SyntaxKind Kind { get; }

        IProxyNode Parent { get; set; }
        IProxyNode Left { get; }
        IProxyNode Right { get; }

        string ClassSignature { get; set; }
        string FileName { get; }

        IEnumerable<IProxyNode> Ancestors();
        void Accept(IProxyAlgorithm algorithm);
    }
}