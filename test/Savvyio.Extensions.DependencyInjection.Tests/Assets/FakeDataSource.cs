namespace Savvyio.Extensions.DependencyInjection.Assets
{
    public class FakeDataSource : IDataSource
    {
    }

    public class FakeDataSource<TMarker> : FakeDataSource, IDataSource<TMarker>
    {
    }
}
