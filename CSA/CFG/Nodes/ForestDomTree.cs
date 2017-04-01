using System.Collections.Generic;

namespace CSA.CFG.Nodes
{
    class ForestDomTree
    {
        public ForestDomTree()
        {
            Forest = new Dictionary<CfgMethod, IDomTree>();
        }

        public Dictionary<CfgMethod, IDomTree> Forest { get; }  

        public IDomTree this[CfgMethod method]
        {
            get { return Forest[method]; }
            set { Forest[method] = value; }
        }
    }
}