using System;
using System.Collections.Generic;
using CSA.ProxyTree.Nodes;
using Microsoft.CodeAnalysis.CSharp;

namespace CSA.ProxyTree.Algorithms
{
    class MetricCalculatorAlgorithm : IProxyAlgorithm
    {
        private int _nbNodes = 0;
        private int _nbConditionnals = 0;
        private int _nbFiles = 0;
        private int _nbQueries = 0;
        private int _nbLambdas = 0;

        public void Begin(List<SyntaxProxyNode> forest)
        {
            _nbFiles = forest.Count;
        }

        public void End()
        {
            Console.WriteLine(" Metrics results: ");
            Console.WriteLine("Nb. files: " + _nbFiles);
            Console.WriteLine("Nb. nodes: " + _nbNodes);
            Console.WriteLine("Nb. conditionnal nodes: " + _nbConditionnals);
            Console.WriteLine("Nb. queries nodes: " + _nbQueries);
            Console.WriteLine("Nb. lambdas nodes: " + _nbLambdas);
        }

        public void Accept(SyntaxProxyNode node)
        {
            ++_nbNodes;

            switch (node.Kind)
            {
                // Conditionnals
                case SyntaxKind.IfStatement:
                case SyntaxKind.SwitchStatement:
                    _nbConditionnals++;
                    break;

                case SyntaxKind.QueryExpression:
                    _nbQueries++;
                    break;

                case SyntaxKind.SimpleLambdaExpression:
                case SyntaxKind.ParenthesizedLambdaExpression:
                    _nbLambdas++;
                    break;

                default:
                    break;
            }
        }
    }
}