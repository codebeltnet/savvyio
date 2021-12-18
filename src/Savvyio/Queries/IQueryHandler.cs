namespace Savvyio.Queries
{
    public interface IQueryHandler : IHandler<IQuery>
    {
        IRequestReplyActivator<IQuery> Queries { get; }
    }
}
