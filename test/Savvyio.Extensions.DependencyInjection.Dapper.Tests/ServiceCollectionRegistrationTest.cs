using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Data;
using Savvyio.Extensions.Dapper;
using Xunit;

namespace Savvyio.Extensions.DependencyInjection.Dapper
{
    public class ServiceCollectionRegistrationTest : Test
    {
        public ServiceCollectionRegistrationTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddDapperDataStore_ShouldTypeForwardPersistentDataStore()
        {
            var services = new ServiceCollection();
            services.AddDapperDataSource(o => o.ConnectionFactory = () => new SqliteConnection("Data Source=:memory:"));
            services.AddDapperDataStore<FakeDapperDataStore, FakeRecord>();
            var provider = services.BuildServiceProvider();

            Assert.IsType<FakeDapperDataStore>(provider.GetRequiredService<IPersistentDataStore<FakeRecord, DapperQueryOptions>>());
        }

        private sealed class FakeRecord
        {
        }

        private sealed class FakeDapperDataStore : DapperDataStore<FakeRecord, DapperQueryOptions>
        {
            public FakeDapperDataStore(Savvyio.Extensions.Dapper.IDapperDataSource source) : base(source)
            {
            }

            public override Task CreateAsync(FakeRecord dto, Action<Cuemon.Threading.AsyncOptions> setup = null)
            {
                return Task.CompletedTask;
            }

            public override Task UpdateAsync(FakeRecord dto, Action<Cuemon.Threading.AsyncOptions> setup = null)
            {
                return Task.CompletedTask;
            }

            public override Task<FakeRecord> GetByIdAsync(object id, Action<Cuemon.Threading.AsyncOptions> setup = null)
            {
                return Task.FromResult<FakeRecord>(null);
            }

            public override Task<IEnumerable<FakeRecord>> FindAllAsync(Action<DapperQueryOptions> setup = null)
            {
                return Task.FromResult<IEnumerable<FakeRecord>>(Array.Empty<FakeRecord>());
            }

            public override Task DeleteAsync(FakeRecord dto, Action<Cuemon.Threading.AsyncOptions> setup = null)
            {
                return Task.CompletedTask;
            }
        }
    }
}
