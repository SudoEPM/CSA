using System;
using System.Collections.Generic;
using CSA.CFG.Iterators;
using CSA.ProxyTree.Nodes;

namespace CSA.CFG.Nodes
{
    public class CfgNode
    {
        private static int _instance = 0;

        public CfgNode(StatementNode origin)
        {
            Origin = origin;
            Prec = new HashSet<CfgNode>();
            Next = new HashSet<CfgNode>();
            UniqueId = _instance++.ToString();
        }

        public StatementNode Origin {get;}
        public HashSet<CfgNode> Prec { get; }

        public HashSet<CfgNode> Next { get; }

        public string UniqueId { get; }

        public string ToDotString()
        {
            return Origin.ToString().Replace(System.Environment.NewLine, @"\n").Replace("\"", " \\\"");
        }

        public IEnumerable<CfgNode> NodeEnumerator => (new PreOrderDepthFirstCfgIterator(this)).GetNodeEnumerable();
        public IEnumerable<CfgLink> LinkEnumerator => (new PreOrderDepthFirstCfgIterator(this)).GetLinkEnumerable();
    }
}