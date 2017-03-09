namespace CSA.ProxyTree.Nodes.Interfaces
{
    public interface ICallableNode : IProxyNode
    {
        string Name { get; }
        string Protection { get; }
        string Signature { get; }
        string Type { get; }
    }
}