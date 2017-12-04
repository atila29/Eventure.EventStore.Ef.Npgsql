using System;
using Eventure.Domain.DomainEvents;
using Eventure.EventStore.Ef.Npgsql.Utility;

namespace Eventure.EventStore.Ef.Npgsql.Model.Factory
{
    public class EventDataFactory : IEventDataFactory<EventData,Guid, Guid>
    {
        public EventData Create(IEvent<Guid, Guid> @event)
        {

            var createdEvent = new EventData
            {
                AggregateId = @event.AggregateId,
                EventType = @event.GetType().FullName,
                Id = @event.Id,
                Timestamp = DateTime.Now,
                Version = @event.Version,
                Data = EventureSerializer.Serialize(@event)
            };
            return createdEvent;
        }
    }
}