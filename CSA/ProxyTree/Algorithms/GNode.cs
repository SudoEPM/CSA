using DotBuilder.Attributes;
using DotBuilder.Statements;

namespace CSA.ProxyTree.Algorithms
{
    class GNode : Statement<Node, INodeAttribute>
    {
        private readonly string _name;

        private GNode(string name, string content)
        {
            _name = name;
            this.Of(Label.With("{" + _name + "|" + content + "}"));
        }

        public static GNode Name(string name, string content) => new GNode(name, content);

        public override string Render()
        {
            return $"{_name} {base.Render()}";
        }
    }
}