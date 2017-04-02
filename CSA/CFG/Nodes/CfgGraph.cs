using System.Collections.Generic;
using CSA.ProxyTree.Nodes;

namespace CSA.CFG.Nodes
{
    class CfgGraph
    {
        private readonly Dictionary<StatementNode, CfgNode> _proxyToCfg;

        public CfgGraph()
        {
            CfgMethods = new Dictionary<string, CfgMethod>();
            CfgNodes = new Dictionary<string, CfgNode>();
            _proxyToCfg = new Dictionary<StatementNode, CfgNode>();
        }

        internal CfgNode GetCfgNode(StatementNode node)
        {
            if (_proxyToCfg.ContainsKey(node))
                return _proxyToCfg[node];

            var cfgNode = new CfgNode(node);
            CfgNodes[cfgNode.UniqueId] = cfgNode;
            _proxyToCfg[node] = cfgNode;
            return cfgNode;
        }

        public Dictionary<string, CfgMethod> CfgMethods { get; }
        public Dictionary<string, CfgNode> CfgNodes { get; }
    }
}
