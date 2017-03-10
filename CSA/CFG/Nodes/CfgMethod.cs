using System;
using System.Linq;
using CSA.CFG.Iterators;
using CSA.ProxyTree.Nodes.Interfaces;
using Microsoft.CodeAnalysis.CSharp;

namespace CSA.CFG.Nodes
{
    class CfgMethod
    {
        private readonly ICallableNode _origin;

        public CfgMethod(ICallableNode origin, CfgNode root)
        {
            _origin = origin;
            Root = root;           
        }

        public CfgNode Root { get; private set; }

        public string ClassSignature => _origin.ClassSignature;

        public void RemoveBlocks()
        {
            if (Root == null)
                return;

            var newRoot = Root;
            while (newRoot != null && newRoot.Origin.Kind == SyntaxKind.Block)
            {
                if (newRoot.Next.Count > 1)
                {
                    throw new Exception($"Could not find the root of the method :{_origin.Signature}");
                }
                newRoot = newRoot.Next.FirstOrDefault();
            }

            // Now we will remove the block nodes
            var blocks = Root.NodeEnumerator.Where(x => x.Origin.Kind == SyntaxKind.Block).ToList();
            foreach (var node in blocks)
            {
                var nexts = node.Next;
                var precs = node.Prec;
                foreach (var prec in precs)
                {
                    prec.Next.UnionWith(nexts);
                    prec.Next.Remove(node);
                }
                foreach (var next in nexts)
                {
                    next.Prec.UnionWith(precs);
                    next.Prec.Remove(node);
                }
            }

            Root = newRoot;
        }
    }
}
