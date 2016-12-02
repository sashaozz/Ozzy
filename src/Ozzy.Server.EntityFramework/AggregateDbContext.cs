using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ozzy.Core;
using Ozzy.DomainModel;

namespace Ozzy.Server.EntityFramework
{

    /// <summary>
    /// Дата-контекст для агрегатов определенной доменной модели. Данный класс получает и сохраняет агрегаты, определенные в доменной модели.
    /// При сохранении агрегата автоматиески сохраняются и публикуются все его доменные события.
    /// Возможна дополнительная пуликацию соытий через быстрый канал публикации.
    /// </summary>
    public class AggregateDbContext : DbContext
    {
        protected readonly IFastEventPublisher FastEventPublisher;        
        private readonly List<DomainEventRecord> _eventsToSave = new List<DomainEventRecord>();
        private DbContextOptions<AggregateDbContext> _options;

        /// <summary>
        /// Создает новый дата-контекст для агрегатов без быстрого канала публикации
        /// </summary>
        /// <param name="options">Параметры подключения к БД</param>
        public AggregateDbContext(DbContextOptions<AggregateDbContext> options) : this(options, NullEventsPublisher.Instance)
        {
        }
        /// <summary>
        /// Создает новый дата-контекст для агрегатов с быстрым каналом публикации
        /// </summary>
        /// <param name="options">Параметры подключения к БД</param>
        /// <param name="fastEventPublisher">Быстрый канал публикации событий</param>
        public AggregateDbContext(DbContextOptions<AggregateDbContext> options, IFastEventPublisher fastEventPublisher) : base(options)
        {
            _options = options;
            if (fastEventPublisher == null) throw new ArgumentNullException(nameof(fastEventPublisher));
            FastEventPublisher = fastEventPublisher;            
        }

        /// <summary>
        /// Доменные события доменной модели
        /// </summary>
        public DbSet<DomainEventRecord> DomainEvents { get; set; }

        public DbSet<EntityDistributedLockRecord> DistributedLocks { get; set; }

        /// <summary>
        /// Слушатели событий в данном контексте и номера их последних обработанных сообщений
        /// </summary>
        public virtual DbSet<Sequence> Sequences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<EmptyEventRecord>();
            modelBuilder.Entity<DomainEventRecord>();
            modelBuilder.Entity<DomainEventRecord>().HasKey(r => r.Sequence);

            modelBuilder.Entity<Sequence>().HasKey(c => c.Name);
            modelBuilder.Entity<Sequence>().Property(c => c.Name).IsRequired(); ;

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

            Logger<IDomainModelTracing>.TraceVerboseMessageIfEnabled(() => $"Saving {domainEvents.Count} domain events");
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

            Logger<IDomainModelTracing>.TraceVerboseMessageIfEnabled(() => $"Saving {removedEntitiesDomainEvents.Count} entity removed events");
            DomainEvents.AddRange(removedEntitiesDomainEvents);
            _eventsToSave.AddRange(removedEntitiesDomainEvents);
        }

        /// <summary>
        /// Сохраняет изменения в доменной модели
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            Logger<IDomainModelTracing>.Log.TraceVerboseEvent("Start saving changes to context");
            SaveDomainEvents();
            var result = base.SaveChanges();

            foreach (var record in _eventsToSave)
            {
                try
                {
                    Logger<IDomainModelTracing>.Log.TracePublishToFastChannel(record);
                    FastEventPublisher.Publish(record);
                }
                catch (Exception e)
                {
                    Logger<IDomainModelTracing>.Log.PublishToFastChannelException(e);
                }
            }
            _eventsToSave.Clear();

            return result;
        }

        /// <summary>
        /// Асинхронно сохраняет изменения в доменной модели
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            Logger<IDomainModelTracing>.Log.TraceVerboseEvent("Start saving changes to context");
            SaveDomainEvents();
            var result = await base.SaveChangesAsync(cancellationToken);
            foreach (var record in _eventsToSave)
            {
                try
                {
                    Logger<IDomainModelTracing>.Log.TracePublishToFastChannel(record);
                    FastEventPublisher.Publish(record);
                }
                catch (Exception e)
                {
                    Logger<IDomainModelTracing>.Log.PublishToFastChannelException(e);
                }
            }
            _eventsToSave.Clear();

            return result;
        }

        public AggregateDbContext Clone()
        {
            return new AggregateDbContext(_options, FastEventPublisher);
        }
    }
}
