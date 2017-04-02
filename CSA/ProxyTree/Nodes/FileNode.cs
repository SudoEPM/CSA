using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes.Interfaces;
using CSA.ProxyTree.Visitors.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Ninject;

namespace CSA.ProxyTree.Nodes
{
    public class FileNode : BasicProxyNode
    {
        private Document _document;

        public FileNode(SyntaxNode origin) : base(origin)
        {
            _document = Program.Kernel.Get<Document>();
        }

        public string ToCodeFiltered(ImmutableHashSet<IProxyNode> nodesToExclude)
        {
            var notRoots = new HashSet<IProxyNode>();
            foreach (var possibleRoot in nodesToExclude)
            {
                var it = new PreOrderDepthFirstProxyIterator(possibleRoot);
                notRoots.UnionWith(nodesToExclude.Intersect(it.Enumerable.Where(x => x != possibleRoot)));
            }
            nodesToExclude = nodesToExclude.Except(notRoots);
            
            var nodes = nodesToExclude.OfType<BasicProxyNode>().Select(x => x.Origin).Where(x => x != null).ToList();

            // Remove correctly the else
            var ifStatementNodes = nodes.OfType<IfStatementSyntax>().Where(x => x.Parent is ElseClauseSyntax).ToList();
            foreach (var toBeReplaced in ifStatementNodes)
            {
                nodes.Remove(toBeReplaced);
                nodes.Add(toBeReplaced.Parent);
            }

            var editor = DocumentEditor.CreateAsync(_document).Result;
            foreach (var node in nodes)
            {
                if (node is ElseClauseSyntax 
                    || node is SwitchSectionSyntax
                    || node is CatchClauseSyntax
                    || node is FinallyClauseSyntax
                    || node.Parent is BlockSyntax)
                {
                    editor.RemoveNode(node);
                }
                else
                {
                    var dummy = SyntaxFactory.Block();
                    editor.ReplaceNode(node, (n, syntaxNode) => dummy);
                }

                var s = editor.GetChangedDocument().GetSyntaxTreeAsync().Result.ToString();
            }

            
            return editor.GetChangedDocument().GetSyntaxTreeAsync().Result.ToString();
        }

        public override void Accept(IProxyVisitor visitor) => visitor.Apply(this);
    }
}