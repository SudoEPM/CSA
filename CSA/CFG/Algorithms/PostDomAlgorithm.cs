using System.Collections.Generic;
using System.Linq;
using CSA.CFG.Iterators;
using CSA.CFG.Nodes;
using Ninject;

namespace CSA.CFG.Algorithms
{
    // Georgiadis, L., Tarjan, R. E., & Werneck, R. F. F. (2006). Finding Dominators in Practice. J. Graph Algorithms Appl., 10(1), 69-94.
    // http://www.cs.princeton.edu/~rwerneck/papers/GTW06-dominators.pdf

    class PostDomAlgorithm : IProduceArtefactsAlgorithm
    {
        public void Execute()
        {
            var cfg = Program.Kernel.Get<CfgGraph>("CFG");
            var domTrees = Program.Kernel.Get<ForestDomTree>("PDomTree");

            foreach (var method in cfg.CfgMethods.Where(x => x.Value.Root != null))
            {
                var domTree = new PostDomTree(method.Value);
                domTrees[method.Value] = domTree;
                Execute(method.Value, domTree);
            }
        }

        private void Execute(CfgMethod method, PostDomTree tree)
        {
            var treeNodes = new HashSet<CfgNode>();

            var treeEnumerator = new ReversePreOrderDepthFirstTreeCfgIterator(method.Exit);
            foreach (var link in treeEnumerator.LinkEnumerable)
            {
                tree[link.From] = link.To;
            }

            var workList = new Queue<CfgNode>();
            var workSet = new HashSet<CfgNode>();
            foreach (var node in treeEnumerator.NodeEnumerable)
            {
                workList.Enqueue(node);
                workSet.Add(node);
                treeNodes.Add(node);
            }

            while (workList.Any())
            {
                var node = workList.Dequeue();
                workSet.Remove(node);

                if (!node.Next.Any())
                {
                    continue;
                }

                CfgNode parent = null;
                foreach (var p in node.Next)
                {
                    if (parent == null)
                    {
                        parent = p;
                        continue;
                    }

                    if (!treeNodes.Contains(p))
                    {
                        continue;
                    }

                    parent = FindCommonRoot(tree.DomParent, parent, p);
                }

                if (tree[node] != parent)
                {
                    tree[node] = parent;
                    foreach (var prec in node.Prec.Where(x => !workSet.Contains(x)))
                    {
                        workSet.Add(prec);
                        workList.Enqueue(prec);
                    }
                }
            }
        }

        private CfgNode FindCommonRoot(Dictionary<CfgNode, CfgNode> domParent, CfgNode node1, CfgNode node2)
        {
            var path = new HashSet<CfgNode>();
            var current = node1;
            while (current != null)
            {
                path.Add(current);
                current = domParent.ContainsKey(current) ? domParent[current] : null;
            }

            current = node2;
            while (current != null && !path.Contains(current))
            {
                current = domParent.ContainsKey(current) ? domParent[current] : null;
            }

            return current;
        }

        public string Name => GetType().Name;

        public IList<string> Artifacts => new List<string> { CSA.Artifacts.PostDomTree };
        public IList<string> Depedencies => new List<string> { CSA.Artifacts.Cfg };
    }
}