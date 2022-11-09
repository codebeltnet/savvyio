using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Events;
using Savvyio.Assets.Domain.EventSourcing;
using Savvyio.Assets.Domain.Handlers;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing;
using Savvyio.Extensions.EFCore;
using Savvyio.Extensions.EFCore.Domain.EventSourcing;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Domain.EventSourcing.Tests
{
    public class TracedAggregateRootTest : Test
    {
        public TracedAggregateRootTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TracedAggregateRoot_ShouldApplyAccountInitiatedToAggregate()
        {
            var id = Guid.NewGuid();
            var providerId = new PlatformProviderId(Guid.NewGuid());
            var fullName = new FullName("Michael", "Mortensen");
            var emailAddress = new EmailAddress("root@gimlichael.dev");

            var sut = new TracedAccount(id, providerId, fullName, emailAddress);

            Assert.Equal(1, sut.Version);
            Assert.Equal(id, sut.Id);
            Assert.Equal(fullName, sut.FullName);
            Assert.Equal(emailAddress, sut.EmailAddress);
            Assert.Equal<Guid>(providerId, sut.PlatformProviderId);
            Assert.Equal(1, sut.Events.Count);
        }

        [Fact]
        public void TracedAggregateRoot_ShouldApplyAllEventsToAggregate()
        {
            var id = Guid.NewGuid();
            var providerId = new PlatformProviderId(Guid.NewGuid());
            var fullName = new FullName("Michael", "Mortennes");
            var emailAddress = new EmailAddress("root@gimlichael.ved");

            var sut = new TracedAccount(id, providerId, fullName, emailAddress);
            sut.ChangeEmailAddress(new EmailAddress("root@gimlichael.dev"));
            sut.ChangeFullName(new FullName("Michael Mortensen"));

            Assert.Equal(3, sut.Version);
            Assert.Equal(id, sut.Id);
            Assert.NotEqual<string>(fullName, sut.FullName);
            Assert.NotEqual<string>(emailAddress, sut.EmailAddress);
            Assert.Equal<Guid>(providerId, sut.PlatformProviderId);
            Assert.Equal(3, sut.Events.Count);
        }

        [Fact]
        public async Task EfCoreDataStore_ShouldRaiseDomainEventsAndDehydrateEventsToStorage()
        {
            string schema = null;
            var sc = new ServiceCollection();
            sc.AddSavvyIO(o => o.AddDomainEventDispatcher().AddDomainEventHandler<AccountDomainEventHandler>());
            sc.AddEfCoreAggregateDataSource(o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase("Dummy").EnableSensitiveDataLogging().EnableDetailedErrors().LogTo(Console.WriteLine, LogLevel.Trace);
                o.ModelConstructor = mb =>
                {
                    mb.AddEventSourcing<TracedAccount, Guid>(eo => eo.TableName = $"{nameof(TracedAccount)}_DomainEvents");
                    schema = mb.Model.ToDebugString(MetadataDebugStringOptions.LongDefault);
                    TestOutput.WriteLine(schema);
                };
            });
            sc.AddEfCoreTracedAggregateRepository<TracedAccount, Guid>();
            sc.AddScoped<ITestStore<IDomainEvent>, InMemoryTestStore<IDomainEvent>>();
            
            var sp = sc.BuildServiceProvider();

            var ds = sp.GetRequiredService<IEfCoreDataSource>();
            var sut4 = sp.GetRequiredService<ITracedAggregateRepository<TracedAccount, Guid>>();

            var id = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";

            var ta = new TracedAccount(id, providerId, name, email);

            for (var i = 0; i < 1000; i++)
            {
                ta.ChangeEmailAddress($"{Generate.RandomString(8)}@gimlichael.dev");
            }
            
            ta.ChangeEmailAddress("root@gimlichael.dev");

            sut4.Add(ta);


            await ds.SaveChangesAsync(); // should raise domain events

            var sut5 = await sut4.GetByIdAsync(id);


            Assert.Equal(id, sp.GetRequiredService<ITestStore<IDomainEvent>>().QueryFor<TracedAccountInitiated>().Single().Id);
            Assert.Equal(providerId, sp.GetRequiredService<ITestStore<IDomainEvent>>().QueryFor<TracedAccountInitiated>().Single().PlatformProviderId);
            Assert.Equal(name, sp.GetRequiredService<ITestStore<IDomainEvent>>().QueryFor<TracedAccountInitiated>().Single().FullName);
            Assert.Equal(email, sp.GetRequiredService<ITestStore<IDomainEvent>>().QueryFor<TracedAccountInitiated>().Single().EmailAddress);
            Assert.Equal("root@gimlichael.dev", sp.GetRequiredService<ITestStore<IDomainEvent>>().QueryFor<TracedAccountEmailAddressChanged>().Last().EmailAddress);

            Assert.Equal(id, sut5.Id);
            Assert.Equal(providerId, sut5.PlatformProviderId);
            Assert.Equal(name, sut5.FullName);
            Assert.Equal("root@gimlichael.dev", sut5.EmailAddress);

            Assert.Equal(@"Model: 
  EntityType: EfCoreTracedAggregateEntity<TracedAccount, Guid>
    Properties: 
      Id (_id, Guid) Required PK AfterSave:Throw
        Annotations: 
          Relational:ColumnName: id
          Relational:ColumnType: uniqueidentifier
      Version (_version, long) Required PK AfterSave:Throw
        Annotations: 
          Relational:ColumnName: version
          Relational:ColumnType: int
      Payload (_payload, byte[])
        Annotations: 
          Relational:ColumnName: payload
          Relational:ColumnType: varchar(max)
      Timestamp (_timestamp, DateTime) Required
        Annotations: 
          Relational:ColumnName: timestamp
          Relational:ColumnType: datetime
      Type (_type, string)
        Annotations: 
          Relational:ColumnName: clrtype
          Relational:ColumnType: varchar(1024)
    Keys: 
      Id, Version PK
    Annotations: 
      Relational:Schema: 
      Relational:TableName: TracedAccount_DomainEvents
      RelationshipDiscoveryConvention:NavigationCandidates: System.Collections.Immutable.ImmutableSortedDictionary`2[System.Reflection.PropertyInfo,System.ValueTuple`2[System.Type,System.Nullable`1[System.Boolean]]]
Annotations: 
  BaseTypeDiscoveryConvention:DerivedTypes: System.Collections.Generic.Dictionary`2[System.Type,System.Collections.Generic.List`1[Microsoft.EntityFrameworkCore.Metadata.IConventionEntityType]]
  NonNullableConventionState: System.Reflection.NullabilityInfoContext
  ProductVersion: 7.0.0
  RelationshipDiscoveryConvention:InverseNavigationCandidates: System.Collections.Generic.Dictionary`2[System.Type,System.Collections.Generic.SortedSet`1[System.Type]]", schema, ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void EfCoreDataStore_ShouldRehydrateFromProvidedEvents()
        {
            var id = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";
            var newName = "Michael Mortensen";
            var newEmail = "root@gimlichael.dev";
            var expectedVersion = 3;

            var sut = new TracedAccount(id, Arguments.ToEnumerableOf<ITracedDomainEvent>(
                new TracedAccountInitiated(id, providerId, name, email).SetAggregateVersion(1),
                new TracedAccountEmailAddressChanged(newEmail).SetAggregateVersion(2),
                new TracedAccountFullNameChanged(newName).SetAggregateVersion(3)));


            Assert.Empty(sut.Events);
            Assert.Equal(id, sut.Id);
            Assert.Equal(providerId, sut.PlatformProviderId);
            Assert.Equal(expectedVersion, sut.Version);
            Assert.Equal(newName, sut.FullName);
            Assert.Equal(newEmail, sut.EmailAddress);
        }
    }
}
