using _4tecture.DataAccess.Common.Storages;
using _4tecture.DataAccess.EntityFramework.Modularity;
using _4tecture.DataAccess.EntityFramework.Storages;
using _4tecture.DependencyInjection.Common;
using _4tecture.Modularity.Common;
using DevFun.Common.Storages;
using DevFun.Storage.EntityConfigurations;
using DevFun.Storage.Storages;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevFun.Storage.Modularity
{
    public class DevFunStorageModule : ModuleDataAccessEntityFramework
    {
        public override IServiceDefinitionCollection RegisterServices(IServiceDefinitionCollection serviceCollection)
        {
            base.RegisterServices(serviceCollection);
            serviceCollection.AddTransient<IRelationalEntityConfiguration, DevJokeConfiguration>();
            serviceCollection.AddTransient<DevFunStorage>();
            serviceCollection.AddTransient<IStorage, DevFunStorage>();
            serviceCollection.AddTransient<StorageSession<DevFunStorage>>();
            serviceCollection.AddTransient<IStorageFactory<IStorage>, StorageFactory<DevFunStorage>>();
            serviceCollection.AddTransient<IStorageFactory<IDevFunStorage>, StorageFactory<DevFunStorage>>();
            serviceCollection.AddTransient<IStorageFactory<DevFunStorage>, StorageFactory<DevFunStorage>>();

            

            return serviceCollection;
        }
    }

    public static class ModuleCatalogExtensions
    {
        public static IModuleCatalog AddDevFunStorageModule(this IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule("DevFunStorageModule", new DevFunStorageModule());
            return moduleCatalog;
        }
    }
}
