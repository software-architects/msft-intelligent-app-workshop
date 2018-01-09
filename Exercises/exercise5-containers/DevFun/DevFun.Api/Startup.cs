using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using DevFun.Storage.Storages;
using _4tecture.DependencyInjection.AspNet;
using _4tecture.DependencyInjection.AutofacAdapter;
using DevFun.Logic.Modularity;
using DevFun.Storage.Modularity;
using DevFun.Common.Storages;
using DevFun.DataInitializer.Modularity;

namespace DevFun.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddEntityFramework()
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<DevFunStorage>(opt => opt.UseInMemoryDatabase());

            // init modules
            var moduleCatalog = services.InitializeModuleCatalog();
            moduleCatalog.AddDevFunLogicModule();
            moduleCatalog.AddDevFunStorageModule();
            moduleCatalog.AddTestDataInitializierModule();
            services.AddModulesFromConfiguration(Configuration, moduleCatalog);

            return services.SetupAutofac();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();


            // do some data initialization
            var dataInitializers = app.ApplicationServices.GetRequiredService<IEnumerable<IDataInitializer>>();
            foreach (var initializer in dataInitializers)
            {
                initializer.InitializeData().Wait();
            }
        }
    }
}
