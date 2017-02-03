using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace CSA.RoslynWalkers
{
    internal class ConsoleDumpWalker : SyntaxWalker
    {
        public override void Visit(SyntaxNode node)
        {
            int padding = node.Ancestors().Count();
            //To identify leaf nodes vs nodes with children
            string prepend = node.ChildNodes().Any() ? "[-]" : "[.]";
            //Get the type of the node
            string line = new String(' ', padding) + prepend + " " + node.GetType();

            //Write the line
            Console.WriteLine(line);
            base.Visit(node);
        }

    }
}