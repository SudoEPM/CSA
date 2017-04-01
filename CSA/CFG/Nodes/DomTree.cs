using System.Collections.Generic;

namespace CSA.CFG.Nodes
{
    class DomTree : IDomTree
    {
        public DomTree(CfgMethod method)
        {
            Method = method;
            DomParent = new Dictionary<CfgNode, CfgNode>();
        }

        public CfgMethod Method
        {
            get;
        }

        public bool Dominate(CfgNode dominator, CfgNode dominated)
        {
            if (dominator == dominated)
                return true;

            dominated = this[dominated];
            while (dominated != null)
            {
                if (dominator == dominated)
                    return true;

                dominated = this[dominated];
            }

            return false;
        }

        public Dictionary<CfgNode, CfgNode> DomParent { get; }

        public CfgNode this[CfgNode node]
        {
            get
            {
                CfgNode res;
                return DomParent.TryGetValue(node, out res) ? res : null;
            }
            set { DomParent[node] = value; }
        }
    }
}
