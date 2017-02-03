using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSA.ProxyTree;
using CSA.ProxyTree.Algorithms;
using CSA.ProxyTree.Visitors;
using Ninject;
using Ninject.Modules;

namespace CSA
{
    class CSAModule : NinjectModule
    {
        private readonly ProgramOptions _options;

        public CSAModule(ProgramOptions options)
        {
            _options = options;
        }

        public override void Load()
        {
            // Binding for calculating metrics
            Bind<IProxyVisitor>().To<DFSProxyVisitor>();

            if (_options.ComputeMetrics) Bind<IProxyAlgorithm>().To<MetricCalculatorAlgorithm>();
        }
    }
}
