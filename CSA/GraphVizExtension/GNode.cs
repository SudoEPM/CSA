using DotBuilder.Attributes;
using DotBuilder.Statements;

namespace CSA.GraphVizExtension
{
    class GNode : Statement<Node, INodeAttribute>
    {
        private readonly string _name;

        private GNode(string name, string content)
        {
            _name = name;
            // Escape some characters...
            content = content.Replace("<", @"\<").Replace(">", @"\>");
            this.Of(Label.With("{" + _name + "|" + content + "}"));
        }

        public static GNode Name(string name, string content) => new GNode(name, content);

        public override string Render()
        {
            return $"{_name} {base.Render()}";
        }
    }
}