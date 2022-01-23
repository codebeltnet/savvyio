using Savvyio.Handlers;

namespace Savvyio.Queries
{
    /// <summary>
    /// Specifies a handler resposible for objects that implements the <see cref="IQuery"/> interface.
    /// </summary>
    /// <seealso cref="IRequestReplyHandler{TRequest}" />
    public interface IQueryHandler : IRequestReplyHandler<IQuery>
    {
    }
}
