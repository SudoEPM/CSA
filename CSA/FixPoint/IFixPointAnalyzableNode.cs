using System.Collections.Generic;

namespace CSA.FixPoint
{
    interface IFixPointAnalyzableNode
    {
        IEnumerable<IFixPointAnalyzableNode> Prec();
        IEnumerable<IFixPointAnalyzableNode> Succ();
    }
}