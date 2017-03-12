using System;
using System.Collections.Generic;
using System.Linq;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSA.ProxyTree.Nodes
{
    public class BasicProxyNode : IProxyNode
    {
        protected SyntaxNode Origin { get; }

        public IProxyNode Parent { get; set; }

        public IProxyNode Left
        {
            get
            {
                var index = Parent.Childs.FindIndex(x => x == this);
                return index == 0 ? null : Parent.Childs[index - 1];
            }
        }

        public IProxyNode Right
        {
            get
            {
                var index = Parent.Childs.FindIndex(x => x == this);
                return index == Parent.Childs.Count - 1 ? null : Parent.Childs[index + 1];
            }
        }

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

        public virtual void Accept(IProxyVisitor visitor) => visitor.Apply(this);

        public string ClassSignature { get; set; }

        protected string FindProtection(SyntaxTokenList modifiers, string def)
        {
            try
            {
                var modifier = modifiers.First(x =>
                    x.Kind() == SyntaxKind.PublicKeyword ||
                    x.Kind() == SyntaxKind.PrivateKeyword ||
                    x.Kind() == SyntaxKind.ProtectedKeyword ||
                    x.Kind() == SyntaxKind.InternalKeyword);
                return modifier.ToString();
            }
            catch (InvalidOperationException)
            {
                return def;
            }
        }
    }
}
