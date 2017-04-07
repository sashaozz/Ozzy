using System;
using System.Diagnostics.Tracing;
using Ozzy.Core.Events;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.Server;
using Ozzy.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Ozzy.Server.EntityFramework;
using Ozzy.DomainModel;

namespace Ozzy.Core.Tests
{
    public class DependencyCompositionTests
    {
        //[Fact]
        //public void EfDOmainTest()
        //{
        //    var services = new ServiceCollection();
        //    services.AddDbContext<TestDomainContext>(ops =>
        //    {
        //        ops.UseInMemoryDatabase(databaseName: "Add_writes_to_database");//UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True;");
        //    });
        //    services.AddEntityFrameworkOzzyDomain<TestDomainContext>();
        //    var serviceProvider = services.BuildServiceProvider();

        //    var options = serviceProvider.GetService<IExtensibleOptions<TestDomainContext>>();
        //    Assert.NotNull(options);
        //    var eventsReader = options.GetService<IPeristedEventsReader>();
        //    Assert.NotNull(eventsReader);
        //    Assert.IsType<DbEventsReader>(eventsReader);
        //}

        //[Fact]
        //public void DomainEventLoopTest()
        //{
        //    var services = new ServiceCollection();
        //    services.AddDbContext<TestDomainContext>(ops =>
        //    {
        //        ops.UseInMemoryDatabase(databaseName: "Add_writes_to_database");//UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True;");
        //    });

        //    services.AddEntityFrameworkOzzyDomain<TestDomainContext>()
        //        .AddEventLoop<TestEventLoop>();
        //    var serviceProvider = services.BuildServiceProvider();

        //    var options = serviceProvider.GetService<IExtensibleOptions<TestDomainContext>>();
        //    Assert.NotNull(options);
        //    var eventsReader = options.GetService<IPeristedEventsReader>();
        //    Assert.NotNull(eventsReader);
        //    Assert.IsType<DbEventsReader>(eventsReader);

        //    var loop = serviceProvider.GetService<TestEventLoop>();
        //    Assert.NotNull(eventsReader);
        //}
    }
}
