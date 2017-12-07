using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Eventure.Domain.DomainEvents;
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
            AfterExecuteEvent += Callback;
        }

        private async Task Callback(IEvent<Guid, Guid> @event) => await Task.Run(() => {});

        protected override Guid GetAggregateId() => _command.AggregateId;

        protected override IEvent<Guid, Guid> CreateEvent(int version) =>
            new TestCreatedEvent(Guid.NewGuid(), _command.AggregateId, version);
    }
}