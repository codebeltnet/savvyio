using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Collections.Generic;
using Cuemon.Extensions;
using Cuemon.Extensions.Xunit;
using Cuemon.Reflection;
using Cuemon.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Events;
using Savvyio.Commands;
using Savvyio.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Domain
{
    public class DomainEventDispatcherExtensionsTest : Test
    {
        public DomainEventDispatcherExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RaiseMany_ShouldRaiseOneEventFromAggregate()
        {
            var sc = new ServiceCollection()
                .AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(DomainEventDispatcher).Assembly, typeof(DomainEventDispatcherExtensionsTest).Assembly))
                .AddScoped<ITestStore<IDomainEvent>, InMemUnitTestStore<IDomainEvent>>();

            var firstName = "   Michael";
            var lastName = "Mortensen   ";
            var emailAddress = "root@gimlichael.dev";
            var providerId = Guid.NewGuid();

            var sp = sc.BuildServiceProvider();
            var sut = new DomainEventDispatcher(sp.GetRequiredService<Func<Type, IEnumerable<object>>>());
            var ar = new Account(new PlatformProviderId(providerId), new FullName(firstName, lastName), new EmailAddress(emailAddress));

            Assert.NotEmpty(ar.Events);

            sut.RaiseMany(ar);

            var des = sp.GetRequiredService<ITestStore<IDomainEvent>>();

            Assert.Empty(ar.Events);
            Assert.Equal("Michael Mortensen", des.QueryFor<AccountInitiated>().Single().FullName);
            Assert.Equal(emailAddress, des.QueryFor<AccountInitiated>().Single().EmailAddress);
            Assert.Equal(providerId, des.QueryFor<AccountInitiated>().Single().PlatformProviderId);
        }

        [Fact]
        public void RaiseMany_ShouldRaiseManyEventsFromAggregate()
        {
            var sc = new ServiceCollection()
                .AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(DomainEventDispatcher).Assembly, typeof(DomainEventDispatcherExtensionsTest).Assembly))
                .AddScoped<ITestStore<IDomainEvent>, InMemUnitTestStore<IDomainEvent>>();

            var sp = sc.BuildServiceProvider();
            var sut = new DomainEventDispatcher(sp.GetRequiredService<Func<Type, IEnumerable<object>>>());
            var ar = new Account(new AccountId(100));

            var fullName = "Michael Mortensen";
            var emailAddress = "root@gimlichael.dev";
            var providerId = Guid.NewGuid();
            var userName = "gimlichael";
            var password = "p@assw0rd";
            var salt = Generate.RandomString(32);

            // emulate orm
            var properties = ar.GetType().GetProperties(new MemberReflection());
            properties.Single(pi => pi.Name == "FullName").SetValue(ar, "Michael Motrensen");
            properties.Single(pi => pi.Name == "EmailAddress").SetValue(ar, "root@gimlichael.ved");
            properties.Single(pi => pi.Name == "PlatformProviderId").SetValue(ar, providerId);
            // done

            ar.ChangeFullName(new FullName(fullName));
            ar.ChangeEmailAddress(emailAddress);
            ar.Promote(new Credentials(userName, password, salt));

            Assert.NotEmpty(ar.Events);

            sut.RaiseMany(ar);

            var des = sp.GetRequiredService<ITestStore<IDomainEvent>>();

            Assert.Empty(ar.Events);
            Assert.Equal(fullName, ar.FullName);
            Assert.Equal(emailAddress, ar.EmailAddress);
            Assert.Equal(providerId, ar.PlatformProviderId);

            Assert.Equal(fullName, des.QueryFor<AccountFullNameChanged>().Single().FullName);
            Assert.Equal(emailAddress, des.QueryFor<AccountEmailAddressChanged>().Single().EmailAddress);
            Assert.Equal(userName, des.QueryFor<UserAccountInitiated>().Single().UserName);
            Assert.Equal(salt, des.QueryFor<UserAccountPasswordInitiated>().Single().Salt);
            Assert.Equal(KeyedHashFactory.CreateHmacCryptoSha256(salt.ToByteArray()).ComputeHash(password).ToHexadecimalString(), des.QueryFor<UserAccountPasswordInitiated>().Single().Hash);
        }

        [Fact]
        public async Task RaiseManyAsync_ShouldRaiseOneEventFromAggregateAsync()
        {
            var sc = new ServiceCollection()
                .AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(DomainEventDispatcher).Assembly, typeof(DomainEventDispatcherExtensionsTest).Assembly))
                .AddScoped<ITestStore<IDomainEvent>, InMemUnitTestStore<IDomainEvent>>();

            var firstName = "   Michael";
            var lastName = "Mortensen   ";
            var emailAddress = "root@gimlichael.dev";
            var providerId = Guid.NewGuid();

            var sp = sc.BuildServiceProvider();
            var sut = new DomainEventDispatcher(sp.GetRequiredService<Func<Type, IEnumerable<object>>>());
            var ar = new Account(new PlatformProviderId(providerId), new FullName(firstName, lastName), new EmailAddress(emailAddress));

            Assert.NotEmpty(ar.Events);

            await sut.RaiseManyAsync(ar);

            var des = sp.GetRequiredService<ITestStore<IDomainEvent>>();

            Assert.Empty(ar.Events);
            Assert.Equal("Michael Mortensen", des.QueryFor<AccountInitiated>().Single().FullName);
            Assert.Equal(emailAddress, des.QueryFor<AccountInitiated>().Single().EmailAddress);
            Assert.Equal(providerId, des.QueryFor<AccountInitiated>().Single().PlatformProviderId);
        }

        [Fact]
        public async Task RaiseManyAsync_ShouldRaiseManyEventsFromAggregateAsync()
        {
            var sc = new ServiceCollection()
                .AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(DomainEventDispatcher).Assembly, typeof(DomainEventDispatcherExtensionsTest).Assembly))
                .AddScoped<ITestStore<IDomainEvent>, InMemUnitTestStore<IDomainEvent>>();

            var sp = sc.BuildServiceProvider();
            var sut = new DomainEventDispatcher(sp.GetRequiredService<Func<Type, IEnumerable<object>>>());
            var ar = new Account(new AccountId(100));

            var fullName = "Michael Mortensen";
            var emailAddress = "root@gimlichael.dev";
            var providerId = Guid.NewGuid();
            var userName = "gimlichael";
            var password = "p@assw0rd";
            var salt = Generate.RandomString(32);

            // emulate orm
            var properties = ar.GetType().GetProperties(new MemberReflection());
            properties.Single(pi => pi.Name == "FullName").SetValue(ar, "Michael Motrensen");
            properties.Single(pi => pi.Name == "EmailAddress").SetValue(ar, "root@gimlichael.ved");
            properties.Single(pi => pi.Name == "PlatformProviderId").SetValue(ar, providerId);
            // done

            ar.ChangeFullName(new FullName(fullName));
            ar.ChangeEmailAddress(emailAddress);
            ar.Promote(new Credentials(userName, password, salt));

            Assert.NotEmpty(ar.Events);

            await sut.RaiseManyAsync(ar);

            var des = sp.GetRequiredService<ITestStore<IDomainEvent>>();

            Assert.Empty(ar.Events);
            Assert.Equal(fullName, ar.FullName);
            Assert.Equal(emailAddress, ar.EmailAddress);
            Assert.Equal(providerId, ar.PlatformProviderId);

            Assert.Equal(fullName, des.QueryFor<AccountFullNameChanged>().Single().FullName);
            Assert.Equal(emailAddress, des.QueryFor<AccountEmailAddressChanged>().Single().EmailAddress);
            Assert.Equal(userName, des.QueryFor<UserAccountInitiated>().Single().UserName);
            Assert.Equal(salt, des.QueryFor<UserAccountPasswordInitiated>().Single().Salt);
            Assert.Equal(KeyedHashFactory.CreateHmacCryptoSha256(salt.ToByteArray()).ComputeHash(password).ToHexadecimalString(), des.QueryFor<UserAccountPasswordInitiated>().Single().Hash);
        }
    }
}
