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

        public TestCommandHandler(IEventStore<EventData, Guid, Guid> eventStore) : base(eventStore)
        {
            AfterExecuteEvent += Callback;
        }

        private async Task Callback(IEvent<Guid, Guid> @event) => await Task.Run(() => {});

        protected override IEvent<Guid, Guid> CreateEvent(int version, TestCommand command) =>
            new TestCreatedEvent(Guid.NewGuid(), command.AggregateId, version);

        protected override Guid GetAggregateId(TestCommand command) => command.AggregateId;

    }
}