using ExampleApplication.Models;
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

        public DbSet<ContactFormMessage> ContactFormMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactFormMessage>().HasKey(c => c.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
