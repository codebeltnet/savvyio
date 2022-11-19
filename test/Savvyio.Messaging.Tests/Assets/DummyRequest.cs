using System;

namespace Savvyio.Messaging.Assets
{
    public record DummyRequest(Guid Uuid, int Number) : Request
    {
    }
}
