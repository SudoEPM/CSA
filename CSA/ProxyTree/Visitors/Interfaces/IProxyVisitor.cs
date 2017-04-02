using CSA.ProxyTree.Nodes;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.ProxyTree.Nodes.Statements;

namespace CSA.ProxyTree.Visitors.Interfaces
{
    public interface IProxyVisitor
    {
        void Apply(IProxyNode node);
        void Apply(ForestNode node);
        void Apply(FileNode node);
        void Apply(ClassNode node);
        void Apply(MethodNode node);
        void Apply(PropertyNode node);
        void Apply(PropertyAccessorNode node);
        void Apply(FieldNode node);
        void Apply(StatementNode node);
        void Apply(IfStatementNode node);
        void Apply(IfElseStatementNode node);
        void Apply(BlockStatementNode node);
        void Apply(SwitchStatementNode node);
        void Apply(SwitchSectionStatementNode node);
        void Apply(BreakStatementNode node);
        void Apply(ContinueStatementNode node);
        void Apply(YieldBreakStatementNode node);
        void Apply(YieldReturnStatementNode node);
        void Apply(ReturnStatementNode node);
        void Apply(WhileStatementNode node);
        void Apply(DoStatementNode node);
        void Apply(ForStatementNode node);
        void Apply(ForEachStatementNode node);
        void Apply(GotoStatementNode node);
        void Apply(GotoCaseStatementNode node);
        void Apply(LabelStatementNode node);
        void Apply(TryStatementNode node);
        void Apply(CatchStatementNode node);
        void Apply(ThrowStatementNode node);
        void Apply(FinallyStatementNode node);
        void Apply(UsingStatementNode node);
        void Apply(ExpressionNode node);
    }
}
