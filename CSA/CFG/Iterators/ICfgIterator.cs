using System.Collections.Generic;
using CSA.CFG.Nodes;

namespace CSA.CFG.Iterators
{
    public class CfgLink
    {
        public CfgLink(CfgNode from, CfgNode to)
        {
            From = from;
            To = to;
        }

        public CfgNode From { get; }
        public CfgNode To { get; }
    }

    interface ICfgIterator
    {
        IEnumerable<CfgLink> LinkEnumerable { get; }
        IEnumerable<CfgNode> NodeEnumerable { get; }
    }
}