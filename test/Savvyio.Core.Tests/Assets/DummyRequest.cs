using System;

namespace Savvyio.Assets
{
    public record DummyRequest(Guid Uuid, int Number) : Request
    {
    }
}
