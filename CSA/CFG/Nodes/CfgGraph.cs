using System.Collections.Generic;
using CSA.ProxyTree.Nodes;

namespace CSA.CFG.Nodes
{
    class CfgGraph
    {
        private readonly Dictionary<StatementNode, CfgNode> _proxyToCfg;

        public CfgGraph(Dictionary<string, CfgMethod> cfgMethods)
        {
            CfgMethods = cfgMethods;
            _proxyToCfg = new Dictionary<StatementNode, CfgNode>();
        }

        internal CfgNode GetCfgNode(StatementNode node)
        {
            if (_proxyToCfg.ContainsKey(node))
                return _proxyToCfg[node];

            var cfgNode = new CfgNode(node);
            _proxyToCfg[node] = cfgNode;
            return cfgNode;
        }

        public Dictionary<string, CfgMethod> CfgMethods { get; }
    }
}
