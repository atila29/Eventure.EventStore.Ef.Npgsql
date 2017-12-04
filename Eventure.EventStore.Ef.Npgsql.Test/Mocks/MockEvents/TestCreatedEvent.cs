using System;
using Eventure.Domain.DomainEvents;

namespace Eventure.EventStore.Ef.Npgsql.Test.Mocks.MockEvents
{
    [Serializable]
    public class TestCreatedEvent : IEvent
    {
        public Guid Id { get; }
        public Guid AggregateId { get; }
        public int Version { get; }
        
        public TestCreatedEvent(Guid id, Guid aggregateId, int version)
        {
            Id = id;
            AggregateId = aggregateId;
            Version = version;
        }
    }
}