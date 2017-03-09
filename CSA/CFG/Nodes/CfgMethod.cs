using CSA.ProxyTree.Nodes.Interfaces;

namespace CSA.CFG.Nodes
{
    class CfgMethod
    {
        private ICallableNode _origin;

        public CfgMethod(ICallableNode origin, CfgNode root)
        {
            _origin = origin;
            Root = root;
        }

        public CfgNode Root { get; }
    }
}
