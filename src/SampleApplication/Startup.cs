﻿using System;
using System.Diagnostics.Tracing;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ozzy;
using Ozzy.Core;
using Ozzy.Core.Events;
using Ozzy.DomainModel;
using Ozzy.Server;
using Ozzy.Server.Configuration;
using SampleApplication.Sagas;
using SampleApplication.Tasks;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace SampleApplication
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            Environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (Environment.IsDevelopment())
            {
                builder.AddJsonFile($"appsettings.{System.Environment.MachineName}.json", optional: true);
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddCors();

            services.AddSingleton<ISerializer, ContractlessMessagePackSerializer>();
            services.AddSingleton<TestBackgoundTask>();
            var ozzyOptions = Configuration.GetSection("OzzyOptions");
            //services.ConfigureEntityFrameworkForOzzy(ozzyOptions);
            //services.ConfigureRedisForOzzy(ozzyOptions);

            services
            .AddOzzyDomain<SampleDbContext>(options =>
            {
                options.UseInMemoryFastChannel();
                options.AddSagaProcessor<ContactFormMessageSaga>();
            })
            .UseEntityFramework((options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SampleDbContext"));
            }));

            services.AddOzzyNode<SampleDbContext>(options =>
            {
                options.UseSqlServerQueues();
                options.UseEFFeatureFlagService();
                options.UseInMemoryMonitoring();
                options.AddBackgroundProcess<NodeConsoleHeartBeatProcess>();
                options.AddFeatureFlag<ConsoleLogFeature>();
            });
            //TODO : fix Api
            //.AddApi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory
            //    .AddConsole(Configuration.GetSection("Logging"))
            //    .AddDebug();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole(LogEventLevel.Verbose, "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{OzzyEvent}")
                .CreateLogger();

            var listener = new ObservableEventListener();
            listener.EnableEvents(OzzyLogger<ICommonEvents>.LogEventSource, EventLevel.LogAlways);
            listener.EnableEvents(OzzyLogger<IDomainModelTracing>.LogEventSource, EventLevel.Informational);
            listener.EnableEvents(OzzyLogger<IDistibutedLockEvents>.LogEventSource, EventLevel.LogAlways);


            var log = loggerFactory.CreateLogger<OzzyLoggerValue>();
            var parser = new MessageTemplateParser();
            listener.Subscribe(new SimpleEventObserver(e =>
            {
                var logEntry = new LogEvent(DateTime.UtcNow,
                    GetSerilogLevel(e.Level),
                    null,
                    new MessageTemplate(new MessageTemplateToken[] { new TextToken(string.Format(e.Message, e.Payload.ToArray()), 0) }),
                    e.Payload.Select((p, i) => new LogEventProperty(e.PayloadNames[i], new ScalarValue(p))).Append(new LogEventProperty("OzzyEvent", new OzzyDictionaryValue(e)))
                    );
                Log.Logger.Write(logEntry);
                //log.Log(LogLevel.Information, new EventId(e.EventId), e, null, (s, ex) => string.Format(s.Message, s.Payload.ToArray()));
            }));


            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseBrowserLink();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            //app.UseStaticFiles();

            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseOzzy().Start();
        }
        private LogEventLevel GetSerilogLevel(EventLevel level)
        {
            switch (level)
            {
                case EventLevel.Verbose:
                    return LogEventLevel.Verbose;
                case EventLevel.Informational:
                    return LogEventLevel.Information;
                case EventLevel.Warning:
                    return LogEventLevel.Warning;
                case EventLevel.Error:
                    return LogEventLevel.Error;
                case EventLevel.Critical:
                    return LogEventLevel.Fatal;
                default:
                    return LogEventLevel.Information;
            }
        }
    }

}
