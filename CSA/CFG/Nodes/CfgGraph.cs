using System.Collections.Generic;

namespace CSA.CFG.Nodes
{
    class CfgGraph
    {
        public CfgGraph(Dictionary<string, CfgMethod> cfgMethods)
        {
            CfgMethods = cfgMethods;
        }

        public Dictionary<string, CfgMethod> CfgMethods { get; }
    }
}
