using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ergate;
using Ergate.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TestSample
{
    public class Startup
    {
        private ServiceInfo info = null;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            info = new ServiceInfo
            {
                ServiceName = "test",
                Title = "测试",
                Version = new ApiVersion(0, 1),
                XmlName = "TestSample.xml"
            };
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddWebApiConventions().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddService(info);

            services.Configure<RsaKeyOption>(x =>
            {
                x.PublicKey = Configuration.GetSection("Rsa:PublicKey").Value;
                x.PrivateKey = Configuration.GetSection("Rsa:PrivateKey").Value;
                x.Issuer = Configuration.GetSection("Rsa:Issuer").Value;
            });

            return services.BuildInterceptableServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseService(env, info);

            app.UseMvc();
        }
    }
}
