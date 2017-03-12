using CSA.ProxyTree.Nodes.Interfaces;

namespace CSA.CFG.Nodes
{
    class CfgMethod
    {
        private readonly ICallableNode _origin;

        public CfgMethod(ICallableNode origin, CfgNode root, CfgNode exit)
        {
            _origin = origin;
            Root = root;
            Exit = exit;
        }

        public CfgNode Root { get; private set; }
        public CfgNode Exit { get; private set; }

        public string ClassSignature => _origin.Namespace + "." + _origin.ClassSignature;
    }
}
