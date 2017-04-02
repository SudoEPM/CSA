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

        public override bool Equals(object obj)
        {
            CfgLink other = obj as CfgLink;
            if (other != null)
            {
                return From == other.From && To == other.To;
            }

            return false;
        }

        public override int GetHashCode()
        {
            // Thanks Jon Skeet : http://stackoverflow.com/questions/1646807/quick-and-simple-hash-code-combinations
            int hash = 17;
            hash = hash * 31 + From.GetHashCode();
            hash = hash * 31 + To.GetHashCode();
            return hash;
        }
    }

    interface ICfgIterator
    {
        IEnumerable<CfgLink> LinkEnumerable { get; }
        IEnumerable<CfgNode> NodeEnumerable { get; }
    }
}