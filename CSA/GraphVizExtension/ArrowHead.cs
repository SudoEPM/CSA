using DotBuilder.Attributes;

namespace CSA.GraphVizExtension
{
    class ArrowHead : IEdgeAttribute
    {
        private readonly string _value;

        public ArrowHead(string value)
        {
            _value = value;
        }

        public string Render()
        {
            return $"arrowhead=\"{_value}\"";
        }
    }
}
