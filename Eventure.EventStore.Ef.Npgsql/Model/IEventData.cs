using System;
using ReflectionMagic;

namespace Eventure.EventStore.Ef.Npgsql.Model
{
    public interface IEventData : IEventData<Guid, Guid>
    {
    }

    public interface IEventData<TEventId, TAggregateId>
        where TEventId : IComparable, IComparable<TEventId>,IEquatable<TEventId>
        where TAggregateId : IComparable, IComparable<TAggregateId>, IEquatable<TAggregateId>
    {
        DateTime Timestamp { get; set; }
        string EventType { get; set; }
        int Version { get; set; }
        byte[] Data { get; set; }

        TEventId Id { get; set; }
        TAggregateId AggregateId { get; set; }
        
    }
}