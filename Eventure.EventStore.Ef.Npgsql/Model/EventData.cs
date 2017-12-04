using System;

namespace Eventure.EventStore.Ef.Npgsql.Model
{
    public class EventData : IEventData
    {
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; }
        public int Version { get; set; }
        public byte[] Data { get; set; }
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
    }
}