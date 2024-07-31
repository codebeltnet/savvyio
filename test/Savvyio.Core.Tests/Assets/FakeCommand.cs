using System;
using Savvyio.Commands;

namespace Savvyio.Assets
{
    internal record FakeCommand : Command
    {
        public FakeCommand()
        {
            Type = null;
        }

        public object Value { get; set; }

        public Type Type { get; protected set; }
    }

    internal record FakeCommand<T> : FakeCommand
    {
        public FakeCommand()
        {
            Type = typeof(T);
        }
    }
}
