using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventure.Domain.Aggregate;
using Eventure.Domain.DomainEvents;
using Eventure.EventStore.Ef.Npgsql.Model;

namespace Eventure.EventStore.Ef.Npgsql.EventStore
{
    public interface IEventStore<TEventData> : IEventStore<TEventData, Guid, Guid> 
        where TEventData : IEventData<Guid, Guid>
    {
        
    }
    
    
    public interface IEventStore<TEventData, TEventId, TAggregateId> 
        where TEventData : IEventData<TEventId, TAggregateId>
        where TEventId : IComparable, IComparable<TEventId>, IEquatable<TEventId>
        where TAggregateId : IComparable, IComparable<TAggregateId>, IEquatable<TAggregateId>
    {
        Task<TAggregate> GetAggregateAsync<TAggregate>(TAggregateId id) where TAggregate : IAggregateRoot<TAggregateId, TEventId>;
        Task AddEventAsync<TEvent>(TEvent @event) where TEvent : IEvent<TEventId, TAggregateId>;
        IEnumerable<IEvent<TEventId, TAggregateId>> GetAllEvents();
        IEnumerable<IEvent<TEventId, TAggregateId>> GetEventsByAggregateId(TAggregateId id);
        IQueryable<TEventData> GetEventsByEventType<TEvent>() where TEvent : IEvent<TEventId, TAggregateId>;
        Task<int> GetAggregateVersionAsync(TAggregateId id);
    }
}