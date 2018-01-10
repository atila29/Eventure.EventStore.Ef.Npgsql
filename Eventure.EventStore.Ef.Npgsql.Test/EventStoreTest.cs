using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventure.Domain.DomainEvents;
using Eventure.Domain.Extensions;
using Eventure.EventStore.Ef.Npgsql.Infrastructure;
using Eventure.EventStore.Ef.Npgsql.Model;
using Eventure.EventStore.Ef.Npgsql.Model.Factory;
using Eventure.EventStore.Ef.Npgsql.Test.Mocks;
using Eventure.EventStore.Ef.Npgsql.EventStore;
using Eventure.EventStore.Ef.Npgsql.Test.Mocks.MockEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Eventure.EventStore.Ef.Npgsql.Test
{
    public class EventStoreTest
    {
        private readonly EventDbContext _context;
        private readonly ServiceProvider _provider;

        public EventStoreTest()
        {
            var builder = new DbContextOptionsBuilder<EventDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new EventDbContext(builder.Options);
            _context = context;
            
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.RegisterAggregateFactory<TestAggregate, TestAggregateFactory>();
            serviceCollection.AddTransient<IEventDataFactory<EventData, Guid, Guid>, EventDataFactory>();
            _provider = serviceCollection.BuildServiceProvider();
        }
        
        [Fact]
        public async Task TestCreateEvent()
        {
            // Arrange
            var eventStore = new EventStore.EventStore(_context, _provider, _provider.GetService<IEventDataFactory<EventData, Guid, Guid>>());
            var aggregateId = Guid.NewGuid();
            const int expectedVersion = 0;
            var expectedEventId = Guid.NewGuid();
            var creationEvent = new TestCreatedEvent(expectedEventId, aggregateId, expectedVersion);
            
            // Act
            await eventStore.AddEventAsync(creationEvent);
            var events = eventStore.GetAllEvents();
            var testEvents = events as IList<IEvent<Guid, Guid>> ?? events.ToList();
            var testEvent = testEvents.First();
            //var resultingAggregate = await eventStore.GetAsync<TestAggregate>(aggregateId);
            
            // Assert
            Assert.NotNull(eventStore);
            Assert.Equal(1, testEvents.Count);
            Assert.IsAssignableFrom<IEvent>(testEvent);
            Assert.IsType<TestCreatedEvent>(testEvent);
            Assert.Equal(aggregateId, testEvent.AggregateId);
            Assert.Equal(expectedVersion, testEvent.Version);
            Assert.Equal(expectedEventId, testEvent.Id);   
        }

        [Fact]
        public async Task TestAddEvent()
        {
            // Arrange
            var eventStore = new EventStore.EventStore(_context, _provider, _provider.GetService<IEventDataFactory<EventData, Guid, Guid>>());
            var aggregateId = Guid.NewGuid();
            const int version = 0;
            const int secondVersion = 1;
            const int expectedVersion = 2;
            var expectedEventId = Guid.NewGuid();
            var secondExpectedEventId = Guid.NewGuid();
            const string updatedValue = "test";
            var creationEvent = new TestCreatedEvent(expectedEventId, aggregateId, version);
            var updatedEvent = new UpdatedTestEvent(secondExpectedEventId, aggregateId, secondVersion, updatedValue);
            
            // Act
            await eventStore.AddEventAsync(creationEvent);
            await eventStore.AddEventAsync(updatedEvent);

            var aggregate = await eventStore.GetAggregateAsync<TestAggregate>(aggregateId);
            
            // Assert
            Assert.NotNull(eventStore);
            Assert.NotNull(aggregate);
            Assert.IsType<TestAggregate>(aggregate);
            Assert.Equal(updatedValue, aggregate.TestProperty);
            Assert.Equal(expectedVersion, aggregate.Version);
        }

        [Fact]
        public async Task TestGetVersion()
        {
            // Arrange
            var eventStore = new EventStore.EventStore(_context, _provider, _provider.GetService<IEventDataFactory<EventData, Guid, Guid>>());
            var aggregateId = Guid.NewGuid();
            const int version = 0;
            const int secondVersion = 1;
            const int expectedVersion = 2;
            var expectedEventId = Guid.NewGuid();
            var secondExpectedEventId = Guid.NewGuid();
            const string updatedValue = "test";
            var creationEvent = new TestCreatedEvent(expectedEventId, aggregateId, version);
            var updatedEvent = new UpdatedTestEvent(secondExpectedEventId, aggregateId, secondVersion, updatedValue);
            
            // Act
            await eventStore.AddEventAsync(creationEvent);
            await eventStore.AddEventAsync(updatedEvent);

            int resultingVersion = await eventStore.GetAggregateVersionAsync(aggregateId);
            
            // Assert
            Assert.Equal(expectedVersion, resultingVersion);
            
        }
        
        
        
    }
}