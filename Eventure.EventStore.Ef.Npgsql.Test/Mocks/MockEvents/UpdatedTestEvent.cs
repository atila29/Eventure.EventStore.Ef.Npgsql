using System;
using Eventure.Domain.DomainEvents;

namespace Eventure.EventStore.Ef.Npgsql.Test.Mocks.MockEvents
{
    [Serializable]
    public class UpdatedTestEvent : IEvent
    {
        public Guid Id { get; }
        public Guid AggregateId { get; }
        public int Version { get; }
        public string UpdatedValue { get; }

        public UpdatedTestEvent(Guid id, Guid aggregateId, int version, string updatedValue)
        {
            Id = id;
            AggregateId = aggregateId;
            Version = version;
            UpdatedValue = updatedValue;
        }
    }
}