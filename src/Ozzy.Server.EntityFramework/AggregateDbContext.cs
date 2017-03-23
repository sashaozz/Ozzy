using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ozzy.Core;
using Ozzy.DomainModel;
using Ozzy.Server.FeatureFlags;
using Ozzy.Server.BackgroundTasks;
using Ozzy.Server.Queues;
using Ozzy.DomainModel.Queues;

namespace Ozzy.Server.EntityFramework
{

    /// <summary>
    /// Дата-контекст для агрегатов определенной доменной модели. Данный класс получает и сохраняет агрегаты, определенные в доменной модели.
    /// При сохранении агрегата автоматиески сохраняются и публикуются все его доменные события.
    /// Возможна дополнительная пуликацию соытий через быстрый канал публикации.
    /// </summary>
    public class AggregateDbContext : DbContext, IOzzyDomainModel
    {
        protected readonly IFastEventPublisher FastEventPublisher;
        private readonly List<DomainEventRecord> _eventsToSave = new List<DomainEventRecord>();

        public AggregateDbContext(IExtensibleOptions<AggregateDbContext> options)
            : this(options.GetDbContextOptions(), options.GetFastEventPublisher())
        {
        }

        /// <summary>
        /// Создает новый дата-контекст для агрегатов без быстрого канала публикации
        /// </summary>
        /// <param name="options">Параметры подключения к БД</param>
        public AggregateDbContext(DbContextOptions options)
            : this(options, NullEventsPublisher.Instance)
        {
        }
        /// <summary>
        /// Создает новый дата-контекст для агрегатов с быстрым каналом публикации
        /// </summary>
        /// <param name="options">Параметры подключения к БД</param>
        /// <param name="fastEventPublisher">Быстрый канал публикации событий</param>
        public AggregateDbContext(DbContextOptions options, IFastEventPublisher fastEventPublisher)
            : base(options)
        {
            FastEventPublisher = fastEventPublisher ?? NullEventsPublisher.Instance;
        }

        /// <summary>
        /// Доменные события доменной модели
        /// </summary>
        public DbSet<DomainEventRecord> DomainEvents { get; set; }
        public DbSet<EntityDistributedLockRecord> DistributedLocks { get; set; }
        public DbSet<FeatureFlagRecord> FeatureFlags { get; set; }
        public DbSet<QueueRecord> Queues { get; set; }
        /// <summary>
        /// Слушатели событий в данном контексте и номера их последних обработанных сообщений
        /// </summary>
        public virtual DbSet<Sequence> Sequences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<EmptyEventRecord>();

            modelBuilder.Entity<DomainEventRecord>().HasKey(r => r.Sequence);

            modelBuilder.Entity<Sequence>().HasKey(c => c.Name);
            modelBuilder.Entity<Sequence>().Property(c => c.Name).IsRequired();

            modelBuilder.Entity<FeatureFlagRecord>().Ignore(r => r.Configuration);
            modelBuilder.Entity<FeatureFlagRecord>().Ignore(r => r.Events);

            modelBuilder.Entity<QueueRecord>().Ignore(r => r.Events);

            base.OnModelCreating(modelBuilder);
        }

        private void SaveDomainEvents()
        {
            var domainEventEntities = ChangeTracker.Entries<IAggregate>()
                .Where(po => po.Entity.Events.Any())
                .Select(po => po.Entity)
                .ToList();

            var domainEvents = domainEventEntities
                .SelectMany(dee => dee.Events.Select(ev => new DomainEventRecord(ev)))
                .ToList();

            OzzyLogger<IDomainModelTracing>.TraceVerboseMessageIfEnabled(() => $"Saving {domainEvents.Count} domain events");
            DomainEvents.AddRange(domainEvents);
            _eventsToSave.AddRange(domainEvents);
            foreach (var entity in domainEventEntities)
            {
                entity.Events.Clear();
            }

            var removedEntitiesEvents = ChangeTracker.Entries<IEntity>()
                .Where(dbe => dbe.State == EntityState.Deleted)
                .Select(po => new EntityRemovedEvent(po.Entity));

            var removedEntitiesDomainEvents = removedEntitiesEvents
                .Select(domainEvent => new DomainEventRecord(domainEvent))
                .ToList();

            OzzyLogger<IDomainModelTracing>.TraceVerboseMessageIfEnabled(() => $"Saving {removedEntitiesDomainEvents.Count} entity removed events");
            DomainEvents.AddRange(removedEntitiesDomainEvents);
            _eventsToSave.AddRange(removedEntitiesDomainEvents);
        }

        /// <summary>
        /// Сохраняет изменения в доменной модели
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            OzzyLogger<IDomainModelTracing>.Log.TraceVerboseEvent("Start saving changes to context");
            SaveDomainEvents();
            var result = base.SaveChanges();

            foreach (var record in _eventsToSave)
            {
                try
                {
                    OzzyLogger<IDomainModelTracing>.Log.TracePublishToFastChannel(record);
                    FastEventPublisher.Publish(record);
                }
                catch (Exception e)
                {
                    OzzyLogger<IDomainModelTracing>.Log.PublishToFastChannelException(e);
                }
            }
            _eventsToSave.Clear();

            return result;
        }

        /// <summary>
        /// Асинхронно сохраняет изменения в доменной модели
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            OzzyLogger<IDomainModelTracing>.Log.TraceVerboseEvent("Start saving changes to context");
            SaveDomainEvents();
            var result = await base.SaveChangesAsync(cancellationToken);
            foreach (var record in _eventsToSave)
            {
                try
                {
                    OzzyLogger<IDomainModelTracing>.Log.TracePublishToFastChannel(record);
                    FastEventPublisher.Publish(record);
                }
                catch (Exception e)
                {
                    OzzyLogger<IDomainModelTracing>.Log.PublishToFastChannelException(e);
                }
            }
            _eventsToSave.Clear();

            return result;
        }

        protected List<IDomainEvent> EventsList { get; set; } = new List<IDomainEvent>();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            Guard.ArgumentNotNull(domainEvent, nameof(domainEvent));
            var record = new DomainEventRecord(domainEvent);
            DomainEvents.Add(record);
            _eventsToSave.Add(record);
        }
    }
}
