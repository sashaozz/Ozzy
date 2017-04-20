﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ozzy.Server.Configuration;
using Microsoft.EntityFrameworkCore;
using Ozzy.Server.EntityFramework;
using Ozzy.Server.Api.Configuration;
using Ozzy.Core.Events;
using Ozzy.Core;
using System.Diagnostics.Tracing;
using System;
using System.Linq;
using Ozzy.DomainModel;
using Ozzy.Server;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;
using Ozzy.Server.Events;
using Ozzy.Server.BackgroundProcesses;
using SampleApplication.Tasks;
using Ozzy.Server.Queues;
using SampleApplication.Queues;
using Ozzy.Server.Saga;
using SampleApplication.Sagas;

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

            services.AddTransient<TestBackgoundTask>();
            services.AddTransient<IQueueService<SampleQueueItem>, QueueService<SampleQueueItem>>();
            var ozzyOptions = Configuration.GetSection("OzzyOptions");
            //services.ConfigureEntityFrameworkForOzzy(ozzyOptions);
            //services.ConfigureRedisForOzzy(ozzyOptions);

            OzzyDomainBuilder<SampleDbContext> domain = null;
            if (Environment.IsDevelopment())
            {
                domain = services
                .AddEntityFrameworkOzzyDomain<SampleDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("SampleDbContext"));
                })
                .UseInMemoryFastChannel()
                .AddEventLoop<SampleEventLoop>(options =>
                {
                    options.AddProcessor<LoggerEventsProcessor>();                    
                    //options.AddHandler<LoggerEventHandler, SimpleChekpointManager>();
                    options.AddSagaProcessor<ContactFormMessageSaga>();
                });
            }
            else
            {
                domain = services
                .AddEntityFrameworkOzzyDomain<SampleDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("SampleDbContext"));
                })
                //.UseRedisFastChannel()
                .AddEventLoop<SampleEventLoop>(options =>
                {
                    //options.AddHandler<SampleEventProcessor>();
                    options.AddSagaProcessor<ContactFormMessageSaga>();
                });
            }

            var node = services
                .AddOzzy()
                .UseSimpleAuthService()
                .AddBackgroundProcess<NodeConsoleHeartBeatProcess>()
                .AddBackgroundProcess<NodeConsoleHeartBeatProcess2>()
                .AddBackgroundMessageLoopProcess<SampleEventLoop>()
                .AddBackgroundMessageLoopProcess<OzzyNodeEventLoop<SampleDbContext>>()
                .UseEFDomainEvent<SampleDbContext>()
                .UseEFDistributedLockService<SampleDbContext>()
                .UseEFFeatureFlagService<SampleDbContext>()
                .UseEFBackgroundTaskService<SampleDbContext>()
                .AddBackgroundProcess<TaskQueueProcess>()
                .UseRedis(ozzyOptions)
                .UseRedisMonitoring()
                //.UseInMemoryMonitoring()
                .AddFeatureFlag<ConsoleLogFeature>()
                .AddApi();
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

            //app.UseMessageLoop<SampleEventsLoop>(handlers =>
            //{
            //    handlers.Addhandler<SampleEventProcessor>();
            //});                    

            app.UseCors(builder => builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseCookieAuthentication();
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
