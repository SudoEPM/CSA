using CSA.ProxyTree.Nodes.Interfaces;

namespace CSA.CFG.Nodes
{
    class CfgMethod
    {
        public ICallableNode Origin { get; }

        public CfgMethod(ICallableNode origin, CfgNode root, CfgNode exit)
        {
            Origin = origin;
            Root = root;
            Exit = exit;
        }

        public CfgNode Root { get; private set; }
        public CfgNode Exit { get; private set; }

        public string ClassSignature => Origin.Namespace + "." + Origin.ClassSignature;

        public override bool Equals(object obj)
        {
            var other = obj as CfgMethod;
            if (other != null)
            {
                return Origin.Signature == other.Origin.Signature;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Origin.GetHashCode();
        }
    }
}
