using Codebelt.Extensions.Xunit;
using Cuemon;
using Cuemon.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Events;
using Savvyio.Assets.Domain.EventSourcing;
using Savvyio.Assets.Domain.Handlers;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Domain.EventSourcing;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing;
using Savvyio.Extensions.EFCore;
using Savvyio.Extensions.EFCore.Domain.EventSourcing;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Extensions.Text.Json;
using Savvyio.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Savvyio.Domain.EventSourcing
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

        [Theory]
        [InlineData(typeof(NewtonsoftJsonMarshaller))]
        [InlineData(typeof(JsonMarshaller))]
        public async Task EfCoreDataStore_ShouldRaiseDomainEventsAndDehydrateEventsToStorage(Type formatterType)
        {
            string schema = null;
            var sc = new ServiceCollection();
            var dbName = "Dummy_" + Generate.RandomString(10); // ensure unique in-memory database name per test run due to .NET 10 changes from Microsoft
            sc.AddSavvyIO(o => o.AddDomainEventDispatcher().AddDomainEventHandler<AccountDomainEventHandler>());

            switch (formatterType)
            {
                case Type newton when newton == typeof(NewtonsoftJsonMarshaller):
                    sc.AddMarshaller<NewtonsoftJsonMarshaller>();
                    sc.AddEfCoreAggregateDataSource<NewtonsoftJsonMarshaller>(o =>
                    {
                        o.ContextConfigurator = b => b.UseInMemoryDatabase(dbName).EnableSensitiveDataLogging().EnableDetailedErrors().LogTo(Console.WriteLine, LogLevel.Trace);
                        o.ModelConstructor = mb =>
                        {
                            mb.AddEventSourcing<TracedAccount, Guid>(eo => eo.TableName = $"{nameof(TracedAccount)}_DomainEvents");
                            schema = mb.Model.ToDebugString(MetadataDebugStringOptions.LongDefault);
                            TestOutput.WriteLine(schema);
                        };
                    });
                    sc.AddEfCoreTracedAggregateRepository<TracedAccount, Guid, NewtonsoftJsonMarshaller>();
                    break;
                case Type builtin when builtin == typeof(JsonMarshaller):
                    sc.AddMarshaller<JsonMarshaller>();
                    sc.AddEfCoreAggregateDataSource<JsonMarshaller>(o =>
                    {
                        o.ContextConfigurator = b => b.UseInMemoryDatabase(dbName).EnableSensitiveDataLogging().EnableDetailedErrors().LogTo(Console.WriteLine, LogLevel.Trace);
                        o.ModelConstructor = mb =>
                        {
                            mb.AddEventSourcing<TracedAccount, Guid>(eo => eo.TableName = $"{nameof(TracedAccount)}_DomainEvents");
                            schema = mb.Model.ToDebugString(MetadataDebugStringOptions.LongDefault);
                            TestOutput.WriteLine(schema);
                        };
                    });
                    sc.AddEfCoreTracedAggregateRepository<TracedAccount, Guid, JsonMarshaller>();
                    break;
            }

            sc.AddScoped<ITestStore<IDomainEvent>, InMemoryTestStore<IDomainEvent>>();

            var sp = sc.BuildServiceProvider();

            var ds = formatterType == typeof(NewtonsoftJsonMarshaller)
                ? sp.GetRequiredService<IEfCoreDataSource<NewtonsoftJsonMarshaller>>() as IEfCoreDataSource
                : sp.GetRequiredService<IEfCoreDataSource<JsonMarshaller>>() as IEfCoreDataSource;
            var sut4 = formatterType == typeof(NewtonsoftJsonMarshaller)
                ? sp.GetRequiredService<ITracedAggregateRepository<TracedAccount, Guid, NewtonsoftJsonMarshaller>>() as ITracedAggregateRepository<TracedAccount, Guid>
                : sp.GetRequiredService<ITracedAggregateRepository<TracedAccount, Guid, JsonMarshaller>>() as ITracedAggregateRepository<TracedAccount, Guid>;

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

            Assert.True(Match(@"Model: 
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
      RelationshipDiscoveryConvention:NavigationCandidates: Microsoft.EntityFrameworkCore.Utilities.OrderedDictionary`2[System.Reflection.PropertyInfo,System.ValueTuple`2[System.Type,System.Nullable`1[System.Boolean]]]
Annotations: 
  BaseTypeDiscoveryConvention:DerivedTypes: System.Collections.Generic.Dictionary`2[System.Type,System.Collections.Generic.List`1[Microsoft.EntityFrameworkCore.Metadata.IConventionEntityType]]
  InversePropertyAttributeConvention:InverseNavigations: System.Collections.Generic.Dictionary`2[System.Type,System.Collections.Generic.SortedSet`1[System.Type]]
  NonNullableConventionState: System.Reflection.NullabilityInfoContext
  ProductVersion: *".ReplaceLineEndings(), schema.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
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

        [Fact]
        public void EfCoreTracedAggregateEntity_ShouldExposeExplicitInterfaceMembers()
        {
            var id = Guid.NewGuid();
            var providerId = new PlatformProviderId(Guid.NewGuid());
            var fullName = new FullName("Test", "User");
            var emailAddress = new EmailAddress("test@unit.test");
            var ta = new TracedAccount(id, providerId, fullName, emailAddress);
            var domainEvent = ta.Events.First();
            var marshaller = new JsonMarshaller();

            var entity = new EfCoreTracedAggregateEntity<TracedAccount, Guid>(ta, domainEvent, marshaller);

            var metadata = ((IMetadata)entity).Metadata;
            var events = ((IAggregateRoot<ITracedDomainEvent>)entity).Events;
            ((IAggregateRoot<ITracedDomainEvent>)entity).RemoveAllEvents(); // no-op

            Assert.NotNull(metadata);
            Assert.Single(events);
            Assert.Single(((IAggregateRoot<ITracedDomainEvent>)entity).Events); // RemoveAllEvents is no-op
        }

        [Fact]
        public async Task EfCoreTracedAggregateRepository_AddRange_ShouldPersistMultipleAggregates()
        {
            var sc = new ServiceCollection();
            var dbName = "Dummy_AddRange_" + Generate.RandomString(10);
            sc.AddSavvyIO(o => o.AddDomainEventDispatcher().AddDomainEventHandler<AccountDomainEventHandler>());
            sc.AddMarshaller<JsonMarshaller>();
            sc.AddEfCoreAggregateDataSource<JsonMarshaller>(o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase(dbName).EnableSensitiveDataLogging().EnableDetailedErrors();
                o.ModelConstructor = mb => mb.AddEventSourcing<TracedAccount, Guid>(eo => eo.TableName = $"{nameof(TracedAccount)}_DomainEvents");
            });
            sc.AddEfCoreTracedAggregateRepository<TracedAccount, Guid, JsonMarshaller>();
            sc.AddScoped<ITestStore<IDomainEvent>, InMemoryTestStore<IDomainEvent>>();

            var sp = sc.BuildServiceProvider();
            var ds = sp.GetRequiredService<IEfCoreDataSource<JsonMarshaller>>() as IEfCoreDataSource;
            var sut4 = sp.GetRequiredService<ITracedAggregateRepository<TracedAccount, Guid, JsonMarshaller>>() as ITracedAggregateRepository<TracedAccount, Guid>;

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var providerId = Guid.NewGuid();

            var ta1 = new TracedAccount(id1, providerId, "Name1", "email1@test.com");
            var ta2 = new TracedAccount(id2, providerId, "Name2", "email2@test.com");

            sut4.AddRange([ta1, ta2]);
            await ds.SaveChangesAsync();

            var result1 = await sut4.GetByIdAsync(id1);
            var result2 = await sut4.GetByIdAsync(id2);

            Assert.Equal(id1, result1.Id);
            Assert.Equal("Name1", result1.FullName);
            Assert.Equal(id2, result2.Id);
            Assert.Equal("Name2", result2.FullName);
        }

        [Fact]
        public async Task EfCoreTracedAggregateRepository_GetByIdAsync_ShouldThrowMissingMethodException_WhenEntityLacksRehydrationConstructor()
        {
            var sc = new ServiceCollection();
            var dbName = "Dummy_NoCtor_" + Generate.RandomString(10);
            sc.AddSavvyIO(o => o.AddDomainEventDispatcher());
            sc.AddMarshaller<JsonMarshaller>();
            sc.AddEfCoreAggregateDataSource<JsonMarshaller>(o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase(dbName);
                o.ModelConstructor = mb => mb.AddEventSourcing<NoCtorTracedAccount, Guid>(eo => eo.TableName = "NoCtorTracedAccount_DomainEvents");
            });
            sc.AddEfCoreTracedAggregateRepository<NoCtorTracedAccount, Guid, JsonMarshaller>();

            var sp = sc.BuildServiceProvider();
            var sut = sp.GetRequiredService<ITracedAggregateRepository<NoCtorTracedAccount, Guid, JsonMarshaller>>() as ITracedAggregateRepository<NoCtorTracedAccount, Guid>;

            await Assert.ThrowsAsync<MissingMethodException>(() => sut.GetByIdAsync(Guid.NewGuid()));
        }

        private class NoCtorTracedAccount : TracedAggregateRoot<Guid>
        {
            protected override void RegisterDelegates(IFireForgetRegistry<ITracedDomainEvent> handler) { }
        }
    }
}
