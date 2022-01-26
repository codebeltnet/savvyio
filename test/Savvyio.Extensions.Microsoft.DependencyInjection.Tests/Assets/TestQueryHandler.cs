using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Savvyio.Handlers;
using Savvyio.Queries;

namespace Savvyio.Extensions.Microsoft.DependencyInjection.Assets
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
