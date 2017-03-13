using System.Diagnostics;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSA.ProxyTree.Nodes.Statements
{
    public class ExpressionAssignmentStatementNode : StatementNode
    {
        public ExpressionAssignmentStatementNode(SyntaxNode origin) : base(origin)
        {
            var stmt = Origin as ExpressionStatementSyntax;
            Debug.Assert(stmt != null, "stmt != null");
            var assignment = stmt.Expression as AssignmentExpressionSyntax;
            Debug.Assert(assignment != null, "assignment != null");
            Identifier = assignment.Left.ToString();
            Expression = assignment.Right.ToString();
        }

        public string Identifier { get; }
        public string Expression { get; }

        public override string ToString()
        {
            return $"{Identifier} = {Expression};";
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}