namespace Savvyio.Queries
{
    public interface IQueryHandler : IHandler<IQuery>
    {
        IResponseHandlerActivator<IQuery> Queries { get; }
    }
}
