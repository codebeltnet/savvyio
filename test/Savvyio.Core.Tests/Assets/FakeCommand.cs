using System;
using Savvyio.Commands;

namespace Savvyio.Assets
{
    internal class FakeCommand : Command
    {
        public FakeCommand()
        {
            Type = null;
        }

        public object Value { get; set; }

        public Type Type { get; protected set; }
    }

    internal class FakeCommand<T> : FakeCommand
    {
        public FakeCommand()
        {
            Type = typeof(T);
        }
    }
}
