using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ExampleApplication;
using ExampleApplication.Models;
using Ozzy.Server;

namespace ExampleApplication.Migrations
{
    [DbContext(typeof(SampleDbContext))]
    [Migration("20170611102002_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ExampleApplication.Models.LoanApplication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Amount");

                    b.Property<string>("Description");

                    b.Property<string>("From");

                    b.Property<string>("Name");

                    b.Property<int>("Status");

                    b.Property<int>("Version");

                    b.Property<bool>("WelcomeMessageSent");

                    b.HasKey("Id");

                    b.ToTable("LoanApplications");
                });

            modelBuilder.Entity("Ozzy.Server.EntityFramework.DomainEventRecord", b =>
                {
                    b.Property<long>("Sequence")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("EventData");

                    b.Property<string>("EventType");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Sequence");

                    b.ToTable("DomainEvents");
                });

            modelBuilder.Entity("Ozzy.Server.EntityFramework.EfDistributedLockRecord", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Id");

                    b.Property<DateTime>("LockDateTime");

                    b.Property<Guid>("LockId")
                        .IsConcurrencyToken();

                    b.Property<int>("Version");

                    b.HasKey("Name");

                    b.ToTable("DistributedLocks");
                });

            modelBuilder.Entity("Ozzy.Server.EntityFramework.EfSagaCorrelationId", b =>
                {
                    b.Property<string>("Name");

                    b.Property<string>("SagaType");

                    b.Property<Guid>("SagaId");

                    b.Property<string>("Value");

                    b.HasKey("Name", "SagaType", "SagaId");

                    b.HasIndex("SagaId");

                    b.ToTable("SagaCorrelationIds");
                });

            modelBuilder.Entity("Ozzy.Server.EntityFramework.EfSagaRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("SagaState");

                    b.Property<int>("SagaVersion")
                        .IsConcurrencyToken();

                    b.Property<string>("StateType");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Sagas");
                });

            modelBuilder.Entity("Ozzy.Server.EntityFramework.Sequence", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Number");

                    b.HasKey("Name");

                    b.ToTable("Sequences");
                });

            modelBuilder.Entity("Ozzy.Server.FeatureFlag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("SerializedConfiguration");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("FeatureFlags");
                });

            modelBuilder.Entity("Ozzy.Server.QueueRecord", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<byte[]>("Payload");

                    b.Property<string>("QueueName");

                    b.Property<int>("Status");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Queues");
                });

            modelBuilder.Entity("Ozzy.Server.EntityFramework.EfSagaCorrelationId", b =>
                {
                    b.HasOne("Ozzy.Server.EntityFramework.EfSagaRecord", "Saga")
                        .WithMany("CorrelationIds")
                        .HasForeignKey("SagaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
