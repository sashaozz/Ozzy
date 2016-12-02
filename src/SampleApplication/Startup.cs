using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ozzy.Server.BackgroundProcesses;
using Ozzy.Server.Configuration;
using Microsoft.EntityFrameworkCore;
using Ozzy.Server.EntityFramework;

namespace SampleApplication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddDbContext<TransientAggregateDbContext>(options => {
                options.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True;");
            }, ServiceLifetime.Transient);
            services.AddDbContext<AggregateDbContext>(options => {
                options.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True;");
            }, ServiceLifetime.Transient);
            services.AddOzzy()
                .AddBackgroundProcess<NodeConsoleHeartBeatProcess>()
                .AddBackgroundProcess<NodeConsoleHeartBeatProcess2>()
                .UseRedis(Configuration.GetSection("OzzyOptions"))
                .UseEFDistributedLockService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseOzzy();
        }
    }
}
