using System.Collections.Immutable;
using System.Linq;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Ninject;

namespace CSA.ProxyTree.Nodes
{
    public class StatementNode : BasicProxyNode
    {
        public StatementNode(SyntaxNode origin) : base(origin)
        {
            LineNumber = origin.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
            Model = Program.Kernel.Get<SemanticModel>();
        }

        public virtual void ComputeDefUse()
        {
            // Analyze data flow
            var results = Model.AnalyzeDataFlow(Origin);
            VariablesDefined = results.WrittenInside.Select(x => x.Name).ToImmutableHashSet();
            VariablesUsed = results.ReadInside.Select(x => x.Name).ToImmutableHashSet();
        }

        public override string ToString()
        {
            return Origin.ToString();
        }

        protected SemanticModel Model { get; }
        public int LineNumber { get; }

        public ImmutableHashSet<string> VariablesDefined { get; protected set; }
        public ImmutableHashSet<string> VariablesUsed { get; protected set; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}