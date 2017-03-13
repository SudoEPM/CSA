using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class LocalDeclarationStatementNode : StatementNode
    {
        public LocalDeclarationStatementNode(SyntaxNode origin) : base(origin)
        {
            var stmt = Origin as LocalDeclarationStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            Type = stmt.Declaration.Type.ToString();

            AssignedIdentifiers = new List<string>();
            Identifiers = new List<string>();
            Initializers = new List<string>();
            foreach (var variable in stmt.Declaration.Variables)
            {
                Identifiers.Add(variable.Identifier.ToString());
                Initializers.Add(variable.Initializer?.ToString());
                if (!string.IsNullOrWhiteSpace(Initializers.Last()))
                {
                    AssignedIdentifiers.Add(Identifiers.Last());
                }
            }
        }

        public string Type { get; }
        public List<string> AssignedIdentifiers { get; }
        public List<string> Identifiers { get; }
        public List<string> Initializers { get; }

        public override string ToString()
        {
            var text = $"{Type} ";
            for (var i = 0; i < Identifiers.Count; i++)
            {
                text += Identifiers[i];
                if (!string.IsNullOrWhiteSpace(Initializers[i]))
                {
                    text += Initializers[i];
                }

                if (i != Identifiers.Count - 1)
                    text += ", ";
            }
            return $"{text};";
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}