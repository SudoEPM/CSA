using System.Collections.Generic;

namespace CSA.CFG.Nodes
{
    internal interface IDomTree
    {
        CfgMethod Method { get; }
        Dictionary<CfgNode, CfgNode> DomParent { get; }
        bool Dominate(CfgNode dominator, CfgNode dominated);
        CfgNode this[CfgNode node] { get; set; }
    }
}