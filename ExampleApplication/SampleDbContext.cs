using ExampleApplication.Models;
using Microsoft.EntityFrameworkCore;
using Ozzy.Server;
using Ozzy.Server.EntityFramework;

namespace ExampleApplication
{
    public class SampleDbContext : AggregateDbContext
    {
        public SampleDbContext(IExtensibleOptions<SampleDbContext> options) : base(options)
        {
        }

        public DbSet<LoanApplication> LoanApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoanApplication>().HasKey(c => c.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
