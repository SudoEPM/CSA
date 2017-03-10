using System.Diagnostics;
using CSA.ProxyTree.Algorithms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes
{
    public class StatementNode : BasicProxyNode
    {
        public StatementNode(SyntaxNode origin) : base(origin)
        {
        }

        public override string ToString()
        {
            switch (Kind)
            {
                case SyntaxKind.IfStatement:
                {
                    var stmt = Origin as IfStatementSyntax;
                    Debug.Assert(stmt != null, "stmt != null");
                    return $"if({stmt.Condition.ToString()})";
                }
            }
            return Origin.ToString();
        }

        public override void Accept(IProxyAlgorithm algorithm) => algorithm.Apply(this);
    }
}