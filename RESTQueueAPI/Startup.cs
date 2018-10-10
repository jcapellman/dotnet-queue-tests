using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RESTQueue.lib.Common;
using RESTQueue.lib.DAL;
using RESTQueue.lib.Queue;

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

            services.AddTransient<IQueue, RabbitQueue>();
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