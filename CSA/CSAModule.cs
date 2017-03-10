using System;
using System.Collections.Generic;
using System.IO;
using CSA.CFG.Algorithms;
using CSA.CFG.Nodes;
using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes;
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
            Bind<IProxyAlgorithm>().To<InitTreeAlgorithm>();
            Bind<IProxyAlgorithm>().To<GenerateCfgAlgorithm>();
            if (_options.ComputeMetrics) Bind<IProxyAlgorithm>().To<MetricCalculatorAlgorithm>();
            if (_options.GeneratePackageUml) Bind<IProxyAlgorithm>().To<UmlPackageGeneratorAlgorithm>();
            if (_options.GenerateClassUml) Bind<IProxyAlgorithm>().To<UmlClassGeneratorAlgorithm>();
            Bind<ICfgAlgorithm>().To<PrintCfgAlgorithm>();

            //Bind<TextWriter>().ToConstant(Console.Out).Named("Generic");
            Bind<TextWriter>().ToConstant(new StreamWriter("generic.txt")).Named("Generic");

            //Bind<TextWriter>().ToConstant(Console.Out).Named("Metric");
            if (_options.ComputeMetrics) Bind<TextWriter>().ToConstant(new StreamWriter("metric.json")).Named("Metric");
            if (_options.GenerateClassUml) Bind<FileStream>().ToConstant(new FileStream("uml-class.png", FileMode.Create)).Named("UML-CLASS");
            if (_options.GenerateClassUml) Bind<FileStream>().ToConstant(new FileStream("uml-package.png", FileMode.Create)).Named("UML-PACKAGE");

            Bind<IDictionary<string, ClassNode>>().To<Dictionary<string, ClassNode>>().InSingletonScope().Named("ClassMapping");
            Bind<CfgGraph>().To<CfgGraph>().InSingletonScope().Named("CFG");
        }
    }
}
