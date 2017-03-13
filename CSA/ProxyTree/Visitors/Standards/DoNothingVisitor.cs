using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.ProxyTree.Nodes.Statements;
using CSA.ProxyTree.Visitors.Interfaces;

namespace CSA.ProxyTree.Visitors.Standards
{
    public class DoNothingVisitor : IProxyVisitor
    {
        public virtual void Apply(IProxyNode node)
        {
        }

        public virtual void Apply(ForestNode node)
        {
        }

        public virtual void Apply(ClassNode node)
        {
        }

        public virtual void Apply(MethodNode node)
        {
        }

        public virtual void Apply(PropertyNode node)
        {
        }

        public virtual void Apply(PropertyAccessorNode node)
        {
        }

        public virtual void Apply(FieldNode node)
        {
        }

        public virtual void Apply(StatementNode node)
        {
        }

        public virtual void Apply(IfStatementNode node)
        {
        }

        public virtual void Apply(IfElseStatementNode node)
        {
        }

        public virtual void Apply(BlockStatementNode node)
        {
        }

        public virtual void Apply(SwitchStatementNode node)
        {
        }

        public virtual void Apply(SwitchSectionStatementNode node)
        {
        }

        public virtual void Apply(BreakStatementNode node)
        {
        }

        public virtual void Apply(ContinueStatementNode node)
        {
        }

        public virtual void Apply(YieldBreakStatementNode node)
        {
        }

        public virtual void Apply(YieldReturnStatementNode node)
        {
        }

        public virtual void Apply(ReturnStatementNode node)
        {
        }

        public virtual void Apply(WhileStatementNode node)
        {
        }

        public virtual void Apply(DoStatementNode node)
        {
        }

        public virtual void Apply(ForStatementNode node)
        {
        }

        public virtual void Apply(ForEachStatementNode node)
        {
        }

        public virtual void Apply(GotoStatementNode node)
        {
        }

        public virtual void Apply(GotoCaseStatementNode node)
        {
        }

        public virtual void Apply(LabelStatementNode node)
        {
        }

        public virtual void Apply(TryStatementNode node)
        {
        }

        public virtual void Apply(CatchStatementNode node)
        {
        }

        public virtual void Apply(ThrowStatementNode node)
        {
        }

        public virtual void Apply(FinallyStatementNode node)
        {
        }

        public virtual void Apply(UsingStatementNode node)
        {
        }

        public virtual void Apply(ExpressionNode node)
        {
        }
    }
}