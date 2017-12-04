using System;
using System.Threading.Tasks;
using Eventure.EventStore.Ef.Npgsql.CommandImpl;
using Eventure.EventStore.Ef.Npgsql.EventStore;
using Eventure.EventStore.Ef.Npgsql.Model;
using Eventure.EventStore.Ef.Npgsql.Test.Mocks.MockEvents;

namespace Eventure.EventStore.Ef.Npgsql.Test.Mocks.MockCommands
{
    public class TestCommandHandler : BaseCommandHandler<TestCommand>
    {
        private readonly TestCommand _command;

        public TestCommandHandler(TestCommand command, IEventStore<EventData, Guid, Guid> eventStore) : base(eventStore)
        {
            _command = command;
        }

        public override async Task ExecuteAsync()
        {
            var aggregateId = _command.AggregateId;
            var version = await EventStore.GetAggregateVersionAsync(aggregateId);
            var @event = new TestCreatedEvent(Guid.NewGuid(), aggregateId, version);
            await EventStore.AddEventAsync(@event);
        }
    }
}