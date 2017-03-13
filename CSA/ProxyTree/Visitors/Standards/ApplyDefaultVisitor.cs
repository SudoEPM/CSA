using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.ProxyTree.Nodes.Statements;
using CSA.ProxyTree.Visitors.Interfaces;

namespace CSA.ProxyTree.Visitors.Standards
{
    public class ApplyDefaultVisitor : IProxyVisitor
    {
        public virtual void Apply(IProxyNode node)
        {
        }

        public virtual void Apply(ForestNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(ClassNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(MethodNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(PropertyNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(PropertyAccessorNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(FieldNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(StatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(IfStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(IfElseStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(BlockStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(SwitchStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(SwitchSectionStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(BreakStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(ContinueStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(YieldBreakStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(YieldReturnStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(ReturnStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(WhileStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(DoStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(ForStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(ForEachStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(GotoStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(GotoCaseStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(LabelStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(TryStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(CatchStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(ThrowStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(FinallyStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(UsingStatementNode node)
        {
            Apply((IProxyNode) node);
        }

        public virtual void Apply(ExpressionNode node)
        {
            Apply((IProxyNode) node);
        }
    }
}