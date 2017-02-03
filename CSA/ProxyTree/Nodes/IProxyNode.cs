using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

namespace CSA.ProxyTree.Nodes
{
    public interface IProxyNode
    {
        List<IProxyNode> Childs { get; }
        SyntaxKind Kind { get; }
        IProxyNode Parent { get; set; }
    }
}