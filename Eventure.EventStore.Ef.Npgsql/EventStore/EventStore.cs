using System;
using Eventure.EventStore.Ef.Npgsql.Infrastructure;
using Eventure.EventStore.Ef.Npgsql.Model;
using Eventure.EventStore.Ef.Npgsql.Model.Factory;

namespace Eventure.EventStore.Ef.Npgsql.EventStore
{
    public class EventStore : BaseEventStore<EventData, Guid, Guid>
    {
        public EventStore(EventDbContext<EventData, Guid, Guid> dbContext, 
            IServiceProvider serviceProvider, 
            IEventDataFactory<EventData, Guid, Guid> eventDataFactory) 
            : base(dbContext, serviceProvider, eventDataFactory)
        {
        }
    }
}