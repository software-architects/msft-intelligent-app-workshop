using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Collections.Generic;

[assembly: OwinStartup(typeof(NetFx.Startup))]

namespace NetFx
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();
            app.UseWebApi(configuration);
        }

        public class HelloController : ApiController
        {
            [HttpGet]
            [Route("api/hello")]
            public IEnumerable<string> Get() => new[] { "Hello", "World" };
        }
    }
}
