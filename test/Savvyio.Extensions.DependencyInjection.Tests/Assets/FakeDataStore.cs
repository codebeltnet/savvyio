namespace Savvyio.Extensions.DependencyInjection.Assets
{
    public class FakeDataStore : IDataStore
    {
    }

    public class FakeDataStore<TMarker> : FakeDataStore, IDataStore<TMarker>
    {
    }
}
