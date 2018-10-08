using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RawRabbit.vNext;

using RESTQueue.lib.Common;
using RESTQueue.lib.DAL;

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
                    = Configuration.GetSection("MongoConnection:HostName").Value ?? Constants.DEFAULT_MONGO_HOSTNAME;
                options.MongoPortNumber
                    = Convert.ToInt32(Configuration.GetSection("MongoConnection:PortNumber").Value ?? Constants.DEFAULT_MONGO_HOSTPORT);
            });

            services.AddTransient<IStorageDatabase, MongoDatabase>();

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