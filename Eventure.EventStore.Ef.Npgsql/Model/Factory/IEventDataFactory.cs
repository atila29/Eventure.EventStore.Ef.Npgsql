using System;
using Eventure.Domain.DomainEvents;

namespace Eventure.EventStore.Ef.Npgsql.Model.Factory
{
    public interface IEventDataFactory<out TEvent, in TEventId, in TAggregateId>
        where TEvent : class, IEventData<TEventId, TAggregateId>
        where TAggregateId : IComparable, IComparable<TAggregateId>, IEquatable<TAggregateId> 
        where TEventId : IComparable, IComparable<TEventId>, IEquatable<TEventId>
    {
        TEvent Create(IEvent<TEventId, TAggregateId> @event);
    }
}