using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Visitors;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class CatchStatementNode : StatementNode
    {
        public CatchStatementNode(SyntaxNode origin) : base(origin)
        {
            var stmt = Origin as CatchClauseSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            Declaration = stmt.Declaration?.ToString();
            Filter = stmt.Filter?.ToString();

            
        }

        public override void ComputeDefUse()
        {
            var stmt = Origin as CatchClauseSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            VariablesDefined = ImmutableHashSet<string>.Empty;
            var identifier = stmt.Declaration?.Identifier.ToString();
            VariablesUsed = !string.IsNullOrWhiteSpace(identifier) ? new HashSet<string> { identifier }.ToImmutableHashSet() : ImmutableHashSet<string>.Empty;
        }

        public override string ToString()
        {
            return $"catch {Declaration} {Filter}";
        }

        public string Declaration { get; }
        public string Filter { get; }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}