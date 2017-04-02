using System.Collections.Generic;
using System.IO;
using CSA.CFG.Algorithms;
using CSA.CFG.Nodes;
using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Iterators;
using CSA.ProxyTree.Nodes;
using Ninject.Modules;

namespace CSA.Options
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

            //Bind<IAlgorithm>().To<PrintTreeAlgorithm>();
            //Bind<IAlgorithm>().To<PrintDefUseAlgorithm>();
            if (_options.ComputeEverything || _options.ComputeMetrics) Bind<IAlgorithm>().To<MetricCalculatorAlgorithm>();
            if (_options.ComputeEverything || _options.GeneratePackageUml) Bind<IAlgorithm>().To<UmlPackageGeneratorAlgorithm>();
            if (_options.ComputeEverything || _options.GenerateClassUml) Bind<IAlgorithm>().To<UmlClassGeneratorAlgorithm>();
            if (_options.ComputeEverything || _options.PrintCfg) Bind<IAlgorithm>().To<PrintCfgAlgorithm>();
            if (_options.ComputeEverything || _options.ComputeDominators) Bind<IAlgorithm>().To<PrintDomTreeAlgorithm>();
            if (_options.ComputeEverything || _options.ComputePostDominators) Bind<IAlgorithm>().To<PrintPostDomTreeAlgorithm>();
            if (_options.ComputeEverything || _options.ComputeReachingDefinitions) Bind<IAlgorithm>().To<PrintCfgWithReachingDefsAlgorithm>();
            if (_options.ComputeEverything || _options.ComputeLiveVariables) Bind<IAlgorithm>().To<PrintCfgWithLiveVariablesAlgorithm>();
            if (_options.ComputeEverything || _options.ComputeControlDepedencies) Bind<IAlgorithm>().To<PrintControlDepedenciesAlgorithm>();
            if (_options.ComputeEverything || _options.ComputeDataDepedencies) Bind<IAlgorithm>().To<PrintDataDepedenciesAlgorithm>();
            if (_options.ComputeEverything || _options.ComputeProgramDepedencies) Bind<IAlgorithm>().To<PrintProgramDepedenciesAlgorithm>();
            if (_options.ComputeEverything || _options.ComputeSlicing) Bind<IAlgorithm>().To<SlicingAlgorithm>();

            //Bind<TextWriter>().ToConstant(Console.Out).Named("Generic");
            Bind<TextWriter>().ToConstant(new StreamWriter("generic.txt")).Named("Generic");

            //Bind<TextWriter>().ToConstant(Console.Out).Named("Metric");
            if (_options.ComputeEverything || _options.ComputeMetrics) Bind<TextWriter>().ToConstant(new StreamWriter("metric.json")).Named("Metric");
            if (_options.ComputeEverything || _options.GenerateClassUml) Bind<FileStream>().ToConstant(new FileStream("uml-class.png", FileMode.Create)).Named("UML-CLASS");
            if (_options.ComputeEverything || _options.GenerateClassUml) Bind<FileStream>().ToConstant(new FileStream("uml-package.png", FileMode.Create)).Named("UML-PACKAGE");

            Bind<IDictionary<string, ClassNode>>().To<Dictionary<string, ClassNode>>().InSingletonScope().Named("ClassMapping");
            Bind<CfgGraph>().To<CfgGraph>().InSingletonScope().Named("CFG");
            Bind<ForestDomTree>().To<ForestDomTree>().InSingletonScope().Named("DomTree");
            Bind<ForestDomTree>().To<ForestDomTree>().InSingletonScope().Named("PDomTree");
            Bind<ControlDepedencies>().To<ControlDepedencies>().InSingletonScope();
            Bind<DataDepedencies>().To<DataDepedencies>().InSingletonScope();
            Bind<ProgramDepedencies>().To<ProgramDepedencies>().InSingletonScope();
        }
    }
}
