using System;
using Savvyio.Handlers;
using Savvyio.Queries;

namespace Savvyio.Extensions.Assets
{
    internal class TestQueryHandler : QueryHandler
    {
        protected override void RegisterDelegates(IRequestReplyRegistry<IQuery> handlers)
        {
            handlers.Register<TestQuery, Guid>(Handler);
        }

        private Guid Handler(TestQuery arg)
        {
            throw new NotImplementedException();
        }
    }
}
