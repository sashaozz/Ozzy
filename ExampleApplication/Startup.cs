using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ozzy.Server.Configuration;
using Microsoft.EntityFrameworkCore;
using ExampleApplication.Sagas.ContactForm;
using Ozzy;

namespace ExampleApplication
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
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddSingleton<ISerializer, ContractlessMessagePackSerializer>();

            services
            .AddOzzyDomain<SampleDbContext>(options =>
            {
                options.UseInMemoryFastChannel();
                options.AddSagaProcessor<ContactFormSaga>();
            })
            .UseEntityFramework((options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SampleDbContext"));
            }));

            services.ConfigureOzzyNode<SampleDbContext>()
                .UseEFDistributedLockService<SampleDbContext>()
                .UseEFBackgroundTaskService<SampleDbContext>();
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

            app.UseOzzy().Start();
        }
    }
}
