using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSA.CFG.Algorithms
{
    interface ICfgAlgorithm
    {
        void Execute();
        string Name { get; }
    }
}
