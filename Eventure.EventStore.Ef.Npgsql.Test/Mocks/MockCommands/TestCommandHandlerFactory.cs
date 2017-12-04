using System;
using Eventure.Command.CommandHandler;
using Eventure.EventStore.Ef.Npgsql.EventStore;
using Eventure.EventStore.Ef.Npgsql.Model;

namespace Eventure.EventStore.Ef.Npgsql.Test.Mocks.MockCommands
{
    public class TestCommandHandlerFactory : ICommandHandlerCreater<TestCommand, ICommandHandler<TestCommand>>
    {
        private readonly IEventStore<EventData, Guid, Guid> _eventStore;

        public TestCommandHandlerFactory(IEventStore<EventData, Guid, Guid> eventStore)
        {
            _eventStore = eventStore;
        }

        public ICommandHandler<TestCommand> Create(TestCommand command) => new TestCommandHandler(command, _eventStore);
    }
}