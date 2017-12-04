using System;
using Eventure.EventStore.Ef.Npgsql.Model;
using Microsoft.EntityFrameworkCore;

namespace Eventure.EventStore.Ef.Npgsql.Infrastructure
{
    public class EventDbContext : EventDbContext<EventData>
    {
        public EventDbContext(DbContextOptions options) : base(options)
        {
        }
    }
    
    public class EventDbContext<TEvent> : EventDbContext<TEvent, Guid, Guid> 
        where TEvent : class, IEventData<Guid, Guid>
    {
        public EventDbContext(DbContextOptions options) : base(options)
        {
        }
    }
    
    public class EventDbContext<TEvent, TEventId, TAggregateId> : DbContext 
        where TEvent : class, IEventData<TEventId, TAggregateId>
        where TEventId : IComparable, IComparable<TEventId>, IEquatable<TEventId>
        where TAggregateId : IComparable, IEquatable<TAggregateId>, IComparable<TAggregateId>

    {
        public DbSet<TEvent> Events { get; set; }

        public EventDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}