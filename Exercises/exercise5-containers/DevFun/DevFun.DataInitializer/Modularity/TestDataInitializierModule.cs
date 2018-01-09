using _4tecture.DependencyInjection.Common;
using _4tecture.Modularity.Common;
using DevFun.Common.Storages;
using DevFun.DataInitializer.TestData;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevFun.DataInitializer.Modularity
{
    public class TestDataInitializierModule : ModuleBase
    {
        public override IServiceDefinitionCollection RegisterServices(IServiceDefinitionCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDataInitializer, TestDataInitializer>();
            return serviceCollection;
        }
    }

    public static class ModuleCatalogExtensions
    {
        public static IModuleCatalog AddTestDataInitializierModule(this IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule("TestDataInitializierModule", new TestDataInitializierModule());
            return moduleCatalog;
        }
    }
}
