using System;
using System.Collections.Generic;
using Eventure.Domain.Aggregate;
using Eventure.Domain.DomainEvents;
using Eventure.EventStore.Ef.Npgsql.Test.Mocks.MockEvents;

namespace Eventure.EventStore.Ef.Npgsql.Test.Mocks
{
    public class TestAggregate : AggregateRoot, IApplyEvent<TestCreatedEvent>, IApplyEvent<UpdatedTestEvent>
    {
        public string TestProperty { get; private set; }

        public TestAggregate(Guid id) : base(id)
        {
        }

        public void Apply(TestCreatedEvent @event)
        {
            TestProperty = string.Empty;
        }

        public void Apply(UpdatedTestEvent @event)
        {
            TestProperty = @event.UpdatedValue;
        }
    }
}