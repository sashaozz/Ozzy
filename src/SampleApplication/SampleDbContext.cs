using Microsoft.EntityFrameworkCore;
using Ozzy.Core;
using Ozzy.Server;
using Ozzy.Server.EntityFramework;
using SampleApplication.Entities;

namespace SampleApplication
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