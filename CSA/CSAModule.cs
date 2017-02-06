using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSA.ProxyTree;
using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Iterators;
using Ninject;
using Ninject.Modules;

namespace CSA
{
    class CsaModule : NinjectModule
    {
        private readonly ProgramOptions _options;

        public CsaModule(ProgramOptions options)
        {
            _options = options;
        }

        public override void Load()
        {
            // Binding for calculating metrics
            Bind<IProxyIterator>().To<PostOrderDepthFirstProxyIterator>().Named("PostOrder");
            Bind<IProxyIterator>().To<PreOrderDepthFirstProxyIterator>().Named("PreOrder");

            //Bind<IProxyAlgorithm>().To<PrintTreeAlgorithm>();
            if (_options.ComputeMetrics) Bind<IProxyAlgorithm>().To<InitTreeAlgorithm>();
            if (_options.ComputeMetrics) Bind<IProxyAlgorithm>().To<MetricCalculatorAlgorithm>();
            if (_options.GenerateUml) Bind<IProxyAlgorithm>().To<UmlGeneratorAlgorithm>();

            //Bind<TextWriter>().ToConstant(Console.Out).Named("Generic");
            Bind<TextWriter>().ToConstant(new StreamWriter("generic.txt")).Named("Generic");

            //Bind<TextWriter>().ToConstant(Console.Out).Named("Metric");
            if (_options.ComputeMetrics) Bind<TextWriter>().ToConstant(new StreamWriter("metric.json")).Named("Metric");
            if (_options.GenerateUml) Bind<FileStream>().ToConstant(new FileStream("uml.png", FileMode.Create)).Named("UML");

        }
    }
}
