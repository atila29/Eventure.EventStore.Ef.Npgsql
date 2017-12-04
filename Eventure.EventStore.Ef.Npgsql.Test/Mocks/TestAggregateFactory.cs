using System;
using Eventure.Domain.Aggregate;

namespace Eventure.EventStore.Ef.Npgsql.Test.Mocks
{
    public class TestAggregateFactory : IAggregateRootCreater<TestAggregate>
    {
        public TestAggregate Create(Guid id) => new TestAggregate(id);
    }
}