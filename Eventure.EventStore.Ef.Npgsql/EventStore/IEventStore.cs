using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventure.Domain.Aggregate;
using Eventure.Domain.DomainEvents;
using Eventure.EventStore.Ef.Npgsql.Model;

namespace Eventure.EventStore.Ef.Npgsql.EventStore
{
    public interface IEventStore<in TEventData> : IEventStore<TEventData, Guid, Guid> 
        where TEventData : IEventData<Guid, Guid>
    {
        
    }
    
    
    public interface IEventStore<in TEventData, TEventId, TAggregateId> 
        where TEventData : IEventData<TEventId, TAggregateId>
        where TEventId : IComparable, IComparable<TEventId>, IEquatable<TEventId>
        where TAggregateId : IComparable, IComparable<TAggregateId>, IEquatable<TAggregateId>
    {
        Task<TAggregate> GetAsync<TAggregate>(TAggregateId id) where TAggregate : IAggregateRoot<TAggregateId, TEventId>;
        Task AddEventAsync<TEvent>(TEvent @event) where TEvent : IEvent<TEventId, TAggregateId>;
        IEnumerable<IEvent<TEventId, TAggregateId>> GetAllEvents();
        IEnumerable<IEvent<TEventId, TAggregateId>> GetEventsByAggregateId(TAggregateId id);
        Task<int> GetAggregateVersionAsync(TAggregateId id);
    }
}