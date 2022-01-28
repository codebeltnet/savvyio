using System;

namespace Savvyio.Assets
{
    internal class FakeCommand : IRequest
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
