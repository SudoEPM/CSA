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
        public override void Load()
        {
            // Binding for calculating metrics
            Bind<IProxyVisitor>().To<DFSProxyVisitor>();
            Bind<IProxyAlgorithm>().To<MetricCalculatorAlgorithm>();
        }
    }
}
