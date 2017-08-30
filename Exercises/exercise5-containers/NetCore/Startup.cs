using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace NetCore
{
    public class Program
    {
        public static void Main(string[] args) => BuildWebHost(args).Run();
        public static IWebHost BuildWebHost(string[] args) => 
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) => services.AddMvc();
        public void Configure(IApplicationBuilder app) => app.UseMvc();
    }

    [Route("api/[controller]")]
    public class HelloController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get() => new [] { "Hello", "World" };
    }
}
