using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SampleApplication;
using Ozzy.DomainModel.Queues;

namespace SampleApplication.Migrations
{
    [DbContext(typeof(SampleDbContext))]
    partial class SampleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ozzy.DomainModel.DomainEventRecord", b =>
                {
                    b.Property<long>("Sequence")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EventData");

                    b.Property<string>("EventType");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Sequence");

                    b.ToTable("DomainEvents");
                });

            modelBuilder.Entity("Ozzy.DomainModel.Queues.QueueRecord", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("ItemType");

                    b.Property<string>("QueueName");

                    b.Property<int>("Status");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Queues");
                });

            modelBuilder.Entity("Ozzy.Server.EntityFramework.EntityDistributedLockRecord", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Id");

                    b.Property<DateTime>("LockDateTime");

                    b.Property<Guid>("LockId")
                        .IsConcurrencyToken();

                    b.HasKey("Name");

                    b.ToTable("DistributedLocks");
                });

            modelBuilder.Entity("Ozzy.Server.EntityFramework.Saga.SagaRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SagaState");

                    b.Property<int>("SagaVersion")
                        .IsConcurrencyToken();

                    b.Property<string>("StateType");

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

            modelBuilder.Entity("Ozzy.Server.FeatureFlags.FeatureFlagRecord", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SerializedConfiguration");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("FeatureFlags");
                });

            modelBuilder.Entity("SampleApplication.Entities.ContactFormMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("From");

                    b.Property<string>("Message");

                    b.Property<bool>("MessageSent");

                    b.HasKey("Id");

                    b.ToTable("ContactFormMessages");
                });
        }
    }
}
