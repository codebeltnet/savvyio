using System;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Queries;
using Savvyio.Commands;
using Savvyio.Handlers;
using Savvyio.Queries;

namespace Savvyio.Assets
{
    public class AccountHandlers : ICommandHandler, IQueryHandler
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ITestStore<ICommand> _commandTestStore;
        private readonly ITestStore<IQuery> _queryTestStore;
        private readonly IQueryDispatcher _queryDispatcher;

        public AccountHandlers(ICommandDispatcher commandDispatcher = null, ITestStore<ICommand> commandTestStore = null, ITestStore<IQuery> queryTestStore = null, IQueryDispatcher queryDispatcher = null)
        {
            _commandDispatcher = commandDispatcher;
            _commandTestStore = commandTestStore;
            _queryTestStore = queryTestStore;
            _queryDispatcher = queryDispatcher;
        }

        IFireForgetActivator<ICommand> IFireForgetHandler<ICommand>.Delegates => HandlerFactory.CreateFireForget<ICommand>(registry =>
        {
            registry.Register<CreateAccount>(ca => _commandTestStore.Add(ca));
            registry.RegisterAsync<CreateAccount>(async ca =>
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                _commandTestStore.Add(ca);
            });
        });

        IRequestReplyActivator<IQuery> IRequestReplyHandler<IQuery>.Delegates => HandlerFactory.CreateRequestReply<IQuery>(registry =>
        {
            registry.Register<GetAccount, string>(ga =>
            {
                _queryTestStore.Add(ga);
                return $"{ga.Id}___{Generate.RandomString(16)}";
            });
            registry.RegisterAsync<GetAccount, string>(async ga =>
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                _queryTestStore.Add(ga);
                return $"{ga.Id}___{Generate.RandomString(16)}";
            });
        });
    }
}
