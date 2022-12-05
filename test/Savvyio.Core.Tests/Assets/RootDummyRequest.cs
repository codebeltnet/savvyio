using System;

namespace Savvyio.Assets
{
    public record RootDummyRequest(Guid Uuid, int Number, bool Hidden = false) : DummyRequest(Uuid, Number)
    {
    }
}
