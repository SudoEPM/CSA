using System.Collections.Generic;
using CSA.CFG.Nodes;

namespace CSA.CFG.Iterators
{
    interface ICfgIterator
    {
        IEnumerable<CfgNode> GetEnumerable();
    }
}