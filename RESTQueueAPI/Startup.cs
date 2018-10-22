using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

using RESTQueue.lib.Common;
using RESTQueue.lib.DAL;
using RESTQueue.lib.Managers;
using RESTQueue.lib.Queue;

namespace RESTQueueAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddOptions();

            services.Configure<Settings>(Configuration.GetSection("Settings"));

            services.AddSingleton(Configuration);

            services.AddSingleton<IStorageDatabase, MongoDatabase>();
            services.AddSingleton<IStorageDatabase, LiteDBDatabase>();

            services.AddSingleton(typeof(StorageManager));
            
            services.AddTransient<IQueue, RabbitQueue>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}