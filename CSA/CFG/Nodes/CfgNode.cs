using System.Collections.Generic;
using CSA.ProxyTree.Nodes;

namespace CSA.CFG.Nodes
{
    public class CfgNode
    {
        public CfgNode(StatementNode origin)
        {
            Origin = origin;
            Next = new List<CfgNode>();
        }

        public StatementNode Origin {get;}

        public List<CfgNode> Next { get; }
    }
}