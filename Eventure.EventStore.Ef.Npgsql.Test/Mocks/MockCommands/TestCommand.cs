using System;
using Eventure.Command;

namespace Eventure.EventStore.Ef.Npgsql.Test.Mocks.MockCommands
{
    public class TestCommand : ICommand
    {
        public Guid AggregateId { get; set; }
        public string TestValue { get; set; }
    }
}