using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinaryFormatter;
using Eventure.Domain.Aggregate;
using Eventure.Domain.DomainEvents;
using Eventure.Domain.Extensions;
using Eventure.EventStore.Ef.Npgsql.Infrastructure;
using Eventure.EventStore.Ef.Npgsql.Model;
using Eventure.EventStore.Ef.Npgsql.Model.Factory;
using Eventure.EventStore.Ef.Npgsql.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Eventure.EventStore.Ef.Npgsql.EventStore
{
    public class BaseEventStore<TEventData, TEventId, TAggregateId> : IEventStore<TEventData, TEventId, TAggregateId>
        where TEventData : class, IEventData<TEventId, TAggregateId>
        where TEventId : IComparable, IComparable<TEventId>, IEquatable<TEventId>
        where TAggregateId : IComparable, IComparable<TAggregateId>, IEquatable<TAggregateId>
    {
        protected readonly EventDbContext<TEventData, TEventId, TAggregateId> DbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventDataFactory<TEventData, TEventId, TAggregateId> _eventDataFactory;

        public BaseEventStore(
            EventDbContext<TEventData, TEventId, TAggregateId> dbContext, 
            IServiceProvider serviceProvider, IEventDataFactory<TEventData, TEventId, TAggregateId> eventDataFactory)
        {
            DbContext = dbContext;
            _serviceProvider = serviceProvider;
            _eventDataFactory = eventDataFactory;
        }

        public async Task<TAggregate> GetAsync<TAggregate>(TAggregateId id) 
            where TAggregate : IAggregateRoot<TAggregateId, TEventId>
        {
            var creater = _serviceProvider.GetAggregateFactory<TAggregate, TAggregateId, TEventId>();
            var aggregate = creater.Create(id);
            var events = GetEventsByAggregateId(id);
            
            aggregate.AddEvents(events);
            aggregate.CommitEvents();
            return await Task.FromResult(aggregate);
        }

        public async Task AddEventAsync<TEvent>(TEvent @event) 
            where TEvent : IEvent<TEventId, TAggregateId>
        {
            var persistedEvent = _eventDataFactory.Create(@event);
            await DbContext.Events.AddAsync(persistedEvent);
            await DbContext.SaveChangesAsync();
        }

        public IEnumerable<IEvent<TEventId, TAggregateId>> GetAllEvents()
        {
            var converter = new BinaryConverter();
            return DbContext.Events
                .Select(@event => EventureSerializer.Deserialize<IEvent<TEventId, TAggregateId>>(@event.Data));
        }

        public IEnumerable<IEvent<TEventId, TAggregateId>> GetEventsByAggregateId(TAggregateId id)
        {
            var converter = new BinaryConverter();
            var events = DbContext.Events
                .Where(@event => @event.AggregateId.Equals(id))
                .Select(@event => EventureSerializer.Deserialize<IEvent<TEventId, TAggregateId>>(@event.Data));
            return events;
        }

        public Task<int> GetAggregateVersionAsync(TAggregateId id) => 
            DbContext.Events.CountAsync(@event => @event.AggregateId.Equals(id));
    }
}