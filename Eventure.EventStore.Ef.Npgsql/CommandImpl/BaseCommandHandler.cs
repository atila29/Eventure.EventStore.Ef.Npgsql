using System;
using System.Threading.Tasks;
using Eventure.Command;
using Eventure.Command.CommandHandler;
using Eventure.Domain.DomainEvents;
using Eventure.EventStore.Ef.Npgsql.EventStore;
using Eventure.EventStore.Ef.Npgsql.Model;

namespace Eventure.EventStore.Ef.Npgsql.CommandImpl
{
    public abstract class BaseCommandHandler<TCommand> : BaseCommandHandler<TCommand, EventData, Guid, Guid> where TCommand : ICommand
    {
        protected BaseCommandHandler(IEventStore<EventData, Guid, Guid> eventStore) : base(eventStore)
        {
        }
    }
    
    
    public abstract class BaseCommandHandler<TCommand, TEventData, TEventId, TAggregateId> : ICommandHandler<TCommand> 
        where TCommand : ICommand 
        where TAggregateId : IComparable, IComparable<TAggregateId>, IEquatable<TAggregateId> 
        where TEventId : IComparable, IComparable<TEventId>, IEquatable<TEventId> 
        where TEventData : IEventData<TEventId, TAggregateId>
    {
        protected readonly IEventStore<TEventData, TEventId, TAggregateId> EventStore;

        protected abstract IEvent<TEventId, TAggregateId> CreateEvent(int version);
        protected abstract TAggregateId GetAggregateId();

        protected delegate Task AfterExecuteTaskDelegate(IEvent<TEventId, TAggregateId> @event);
        protected event AfterExecuteTaskDelegate AfterExecuteEvent;
        

        protected BaseCommandHandler(IEventStore<TEventData, TEventId, TAggregateId> eventStore)
        {
            EventStore = eventStore;
        }


        public async Task ExecuteAsync()
        {
            var aggregateId = GetAggregateId();
            var version = await EventStore.GetAggregateVersionAsync(aggregateId);
            var @event = CreateEvent(version);

            await EventStore.AddEventAsync(@event);

            await OnAfterExecuteAsync(@event);
        }

        protected virtual async Task OnAfterExecuteAsync(IEvent<TEventId, TAggregateId> @event)
        {
            await Task.Run(async () =>
            {
                if (AfterExecuteEvent != null) await AfterExecuteEvent.Invoke(@event); 
            });
        }
    }
}
