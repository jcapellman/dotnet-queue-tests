using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RawRabbit.vNext;
using RESTQueue.lib.Managers;
using RESTQueueAPI.Common;

namespace RESTQueueAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<Settings>(options =>
            {
                options.MongoHostName
                    = Configuration.GetSection("MongoConnection:HostName").Value;
                options.MongoPortNumber
                    = Convert.ToInt32(Configuration.GetSection("MongoConnection:PortNumber").Value);
            });

            services.AddTransient(typeof(MongoDBManager));

            services.AddRawRabbit(config =>
            {
                config.AddJsonFile("rawrabbit.json");
                config.AddEnvironmentVariables("RABBIT_");
            });
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}