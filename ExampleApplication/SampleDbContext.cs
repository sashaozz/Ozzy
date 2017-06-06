using Microsoft.EntityFrameworkCore;
using Ozzy.Server;
using Ozzy.Server.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApplication
{
    public class SampleDbContext : AggregateDbContext
    {
        public SampleDbContext(IExtensibleOptions<SampleDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
