using System;
using System.Threading.Tasks;
using Eventure.Command.CommandDispatcher;
using Eventure.Command.Extensions;
using Eventure.Domain.Extensions;
using Eventure.EventStore.Ef.Npgsql.EventStore;
using Eventure.EventStore.Ef.Npgsql.Infrastructure;
using Eventure.EventStore.Ef.Npgsql.Model;
using Eventure.EventStore.Ef.Npgsql.Model.Factory;
using Eventure.EventStore.Ef.Npgsql.Test.Mocks;
using Eventure.EventStore.Ef.Npgsql.Test.Mocks.MockCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Eventure.EventStore.Ef.Npgsql.Test
{
    public class CommandHandlerTest
    {
        private readonly ServiceProvider _provider;

        public CommandHandlerTest()
        {
            var builder = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new EventDbContext(builder.Options);

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ICommandDispatcher, CommandDispatcher>();
            serviceCollection.RegisterAggregateFactory<TestAggregate, TestAggregateFactory>();
            serviceCollection.AddTransient<IEventDataFactory<EventData, Guid, Guid>, EventDataFactory>();
            serviceCollection.RegisterCommandHandler<TestCommand, TestCommandHandler>();
            serviceCollection.AddSingleton<EventDbContext<EventData, Guid, Guid>>(context);
            serviceCollection.AddScoped<IEventStore<EventData, Guid, Guid>, EventStore.EventStore>();
            _provider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task TestCommandDispatch()
        {
            // Arrange
            var command = new TestCommand(){AggregateId = Guid.NewGuid()};
            var dispatcher = _provider.GetService<ICommandDispatcher>();
            var eventStore = _provider.GetService<IEventStore<EventData, Guid, Guid>>();
            
            // Act
            await dispatcher.Dispatch(command);
            var aggregate = await eventStore.GetAggregateAsync<TestAggregate>(command.AggregateId);

            // Assert
            Assert.NotNull(aggregate);
            Assert.IsType<TestAggregate>(aggregate);
            Assert.Equal(1, aggregate.Version);
            Assert.Equal(command.AggregateId, aggregate.Id);
        }
        
    }
}