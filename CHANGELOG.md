# Changelog

## 0.8.0

### New Features

#### Savvyio.Core

- ADDED ISearchableDataStore interface in the Savvyio.Data namespace that defines a generic way of abstracting searchable data access objects (cRud)
- ADDED TaskExtensions class that in the Savvyio namespace that consist of extension methods for the Task{IEnumerable} class: SingleOrDefaultAsync

#### Savvyio.Extensions.DapperExtensions

- ADDED DapperExtensionsDataStore class in the Savvyio.Extensions.DapperExtensions namespace that provides a default implementation of the DapperDataStore{T,TOptions} class
- ADDED DapperExtensionsQueryOptions class in the Savvyio.Extensions.DapperExtensions namespace that provides configuration options for DapperExtensionsDataStore{T}

#### Savvyio.Extensions.DependencyInjection

- ADDED ISearchableDataStore interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of searchable data access objects (cRud)
- ADDED ServiceLocatorOptions class in the in the Savvyio.Extensions.DependencyInjection that provides configuration options for IServiceLocator
- ADDED ServiceOptions class in the in the Savvyio.Extensions.DependencyInjection that provides configuration options for Microsoft Dependency Injection

#### Savvyio.Extensions.Dispatchers

- EXTENDED SavvyioOptionsExtensions class in the Savvyio.Extensions namespace with two new static members: UseAutomaticDispatcherDiscovery, UseAutomaticHandlerDiscovery

#### Savvyio.Extensions.DependencyInjection.DapperExtensions

- ADDED DapperExtensionsDataStore class in the Savvyio.Extensions.DependencyInjection.DapperExtensions namespace that provides a default implementation of the IPersistentDataStore{T,TOptions,TMarker} interface to support multiple implementations that is tailored for Plain Old CLR Objects (POCO) usage by DapperExtensions
- ADDED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Dapper namespace that consist of extension methods for the IServiceCollection interface: AddDapperExtensionsDataStore

#### Savvyio.Extensions.DependencyInjection.EFCore

- ADDED DefaultEfCoreDataStore class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides a default implementation of the IPersistentDataStore{T,TOptions,TMarker} interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication with a source of data using Microsoft Entity Framework Core
- ADDED EfCoreServiceOptions class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides configuration options for Microsoft Dependency Injection

#### Savvyio.Extensions.EFCore

- ADDED DefaultEfCoreDataStore class in the Savvyio.Extensions.EFCore namespace that provides a default implementation of the EfCoreDataStore{T,TOptions} class
- ADDED EfCoreDataStore class in the Savvyio.Extensions.EFCore namespace that represents the base class from which all implementations of EfCoreDataStore{T,TOptions} should derive
- ADDED EfCoreQueryOptions class in the Savvyio.Extensions.EFCore namespace that provides configuration options for DefaultEfCoreDataStore{T}

### Breaking Changes

#### Savvyio.Core

- RENAMED IDataStore interface in the Savvyio namespace to IDataSource interface
- RENAMED IDataAccessObject interface in the Savvyio.Data namespace to IDataStore interface
- RENAMED IDeletableDataAccessObject interface in the Savvyio.Data namespace to IDeletableDataStore interface
- RENAMED IPersistentDataAccessObject interface in the Savvyio.Data namespace to IPersistentDataStore interface
- RENAMED IReadableDataAccessObject interface in the Savvyio.Data namespace to IReadableDataStore interface
- RENAMED IWritableDataAccessObject interface in the Savvyio.Data namespace to IWritableDataStore interface
- RENAMED AutomaticDispatcherDiscovery property on the SavvyioOptions class in the Savvyio namespace to AllowDispatcherDiscovery
- RENAMED AutomaticHandlerDiscovery property on the SavvyioOptions class in the Savvyio namespace to AllowHandlerDiscovery
- RENAMED EnableAutomaticDispatcherDiscovery method on the SavvyioOptions class in the Savvyio namespace to EnableDispatcherDiscovery
- RENAMED EnableAutomaticHandlerDiscovery method on the SavvyioOptions class in the Savvyio namespace to EnableHandlerDiscovery
- CHANGED SavvyioOptionsExtensions class in the Savvyio namespace to abide SRP for extension methods: AddDispatchers, AddHandlers

#### Savvyio.Extensions.Dapper

- RENAMED DapperDataAccessObject class in the Savvyio.Extensions.Dapper namespace to DapperDataStore class
- RENAMED DapperDataStore class in the Savvyio.Extensions.Dapper namespace to DapperDataSource class
- RENAMED DapperDataStoreOptions class in the Savvyio.Extensions.Dapper namespace to DapperDataSourceOptions class
- RENAMED DapperOptions class in the Savvyio.Extensions.Dapper namespace to DapperQueryOptions class
- RENAMED IDapperDataStore interface in the Savvyio.Extensions.Dapper namespace to IDapperDataSource interface

#### Savvyio.Extensions.DependencyInjection

- RENAMED IDataStore interface in the Savvyio.Extensions.DependencyInjection namespace to IDataSource interface
- RENAMED IDataAccessObject interface in the Savvyio.Extensions.DependencyInjection.Data namespace to IDataStore interface
- RENAMED IDeletableDataAccessObject interface in the Savvyio.Extensions.DependencyInjection.Data namespace to IDeletableDataStore interface
- RENAMED IPersistentDataAccessObject interface in the Savvyio.Extensions.DependencyInjection.Data namespace to IPersistentDataStore interface
- RENAMED IReadableDataAccessObject interface in the Savvyio.Extensions.DependencyInjection.Data namespace to IReadableDataStore interface
- RENAMED IWritableDataAccessObject interface in the Savvyio.Extensions.DependencyInjection.Data namespace to IWritableDataStore interface
- CHANGED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Data namespace that consist of extension methods for the IServiceCollection interface: AddDataStore
- CHANGED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection namespace that consist of extension methods for the IServiceCollection interface: AddDataSource

#### Savvyio.Extensions.DependencyInjection.Dapper

- REMOVED DapperDataAccessObject class from the Savvyio.Extensions.DependencyInjection.Dapper namespace
- RENAMED DapperDataStore class in the Savvyio.Extensions.DependencyInjection.Dapper namespace to DapperDataSource class
- RENAMED DapperDataStoreOptions class in the Savvyio.Extensions.DependencyInjection.Dapper namespace to DapperDataSourceOptions class
- RENAMED IDapperDataStore interface in the Savvyio.Extensions.DependencyInjection.Dapper namespace to IDapperDataSource interface
- CHANGED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Dapper namespace that consist of extension methods for the IServiceCollection interface: AddDapperDataSource, AddDapperDataStore

#### Savvyio.Extensions.DependencyInjection.EFCore

- REMOVED EfCoreDataAccessObject class from the Savvyio.Extensions.DependencyInjection.EFCore namespace
- RENAMED EfCoreDataStore class in the Savvyio.Extensions.DependencyInjection.EFCore namespace to EfCoreDataSource class
- RENAMED EfCoreDataStoreOptions class in the Savvyio.Extensions.DependencyInjection.EFCore to EfCoreDataSourceOptions class
- RENAMED IEfCoreDataStore interface in the Savvyio.Extensions.DependencyInjection.EFCore namespace to IEfCoreDataSource interface
- CHANGED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that consist of extension methods for the IServiceCollection interface: AddEfCoreDataSource, AddEfCoreDataStore, AddDefaultEfCoreDataStore

#### Savvyio.Extensions.DependencyInjection.EFCore.Domain

- RENAMED EfCoreAggregateDataStore class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain namespace to EfCoreAggregateDataSource class
- CHANGED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain namespace that consist of extension methods for the IServiceCollection interface: AddEfCoreAggregateDataSource

#### Savvyio.Extensions.EFCore

- RENAMED EfCoreDataAccessObject class in the Savvyio.Extensions.EFCore namespace to EfCoreDataSource class
- RENAMED EfCoreDataStoreOptions class in the Savvyio.Extensions.EFCore namespace to EfCoreDataSourceOptions class
- RENAMED IEfCoreDataStore interface in the Savvyio.Extensions.EFCore namespace to IEfCoreDataSource interface

#### Savvyio.Extensions.EFCore.Domain

- RENAMED EfCoreAggregateDataStore class in the Savvyio.Extensions.EFCore.Domain namespace to EfCoreAggregateDataSource class

## 0.7.0

### New Features

#### Savvyio.Core

- ADDED HandlerFactory class in the Savvyio namespace that provides access to factory methods for creating and configuring generic handlers that supports MEP
- ADDED HandlerServicesDescriptor class in the Savvyio namespace that provides information, in a developer friendly way, about implementations of the IHandler{TRequest} interface such as name, declared members and what type of request they handle
- ADDED IDataStore interface in the Savvyio namespace that defines a marker interface that specifies the actual I/O communication with a data store
- ADDED IHandler interface in the Savvyio namespace that defines a marker interface that specifies a handler
- ADDED IIdentity interface in the Savvyio namespace that an identity typically associated with a storage such as a database
- ADDED IMetadata interface in the Savvyio namespace that defines a generic way to associate metadata with any type of object
- ADDED IMetadataDictionary interface in the Savvyio namespace that defines a generic way to support metadata capabilities
- ADDED IRequest interface in the Savvyio namespace that defines a marker interface that specifies a request/model/event
- ADDED MetadataDictionary class in the Savvyio namespace that provides a default implementation of the IMetadataDictionary interface
- ADDED MetadataExtensions class in the Savvyio namespace that consist of extension methods for the IMetadata interface: GetCausationId, GetCorrelationId, SetCausationId, SetCorrelationId, SetEventId, SetTimestamp, SetMemberType, SaveMetadata, MergeMetadata
- ADDED MetadataFactory class in the Savvyio namespace that provides access to factory methods for maintaining metadata in models
- ADDED Request class in the Savvyio namespace that represents the base class from which all implementations of the IRequest interface should derive
- ADDED SavvyioOptions class in the Savvyio namespace that specifies options that is related to setting up Savvy I/O services
- ADDED SavvyioOptionsExtensions class in the Savvyio namespace that consist of extension methods for the SavvyioOptions class: AddDispatchers, AddHandlers
- ADDED IDataAccessObject interface in the Savvyio.Data namespace that defines a marker interface that specifies an abstraction of persistent data access based on the Data Access Object pattern
- ADDED IDeletableDataAccessObject interface in the Savvyio.Data namespace that defines a generic way of abstracting deletable data access objects (cruD)
- ADDED IPersistentDataAccessObject interface in the Savvyio.Data namespace that defines a generic way of abstracting persistent data access objects (CRUD)
- ADDED IReadableDataAccessObject interface in the Savvyio.Data namespace that defines a generic way of abstracting readable data access objects (cRud)
- ADDED IWritableDataAccessObject interface in the Savvyio.Data namespace that defines a generic way of abstracting writable data access objects (CrUd)
- ADDED Dispatcher class in the Savvyio.Dispatchers namespace that represents the base class from which all implementations of the dispatcher concept should derive
- ADDED FireForgetDispatcher class in the Savvyio.Dispatchers namespace that provides a generic dispatcher that uses Fire-and-Forget/In-Only MEP
- ADDED IDispatcher interface in the Savvyio.Dispatchers namespace that defines a marker interface that specifies a dispatcher that encapsulates how a set of objects interact
- ADDED IServiceLocator interface in the Savvyio.Dispatchers namespace that provides a generic way to locate implementations of service objects
- ADDED RequestReplyDispatcher class in the Savvyio.Dispatchers namespace that provides a generic dispatcher that uses Request-Reply/In-Out MEP
- ADDED ServiceLocator class in the Savvyio.Dispatchers namespace that provides a default implementation of the IServiceLocator interface
- ADDED IDeletableRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting deletable repositories (cruD)
- ADDED IPersistentRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting persistent repositories (CRUD)
- ADDED IReadableRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting readable repositories (cRud)
- ADDED IRepository interface in the Savvyio.Domain namespace that defines a marker interface that specifies an abstraction of persistent data access based on the Repository Pattern
- ADDED ISearchableRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting searchable repositories (cRud)
- ADDED IUnitOfWork interface in the Savvyio.Domain namespace that defines a transaction that bundles multiple IRepository{TEntity,TKey}" calls into a single unit
- ADDED IWritableRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting writable repositories (CrUd)
- ADDED FireForgetRegistryExtensions class in the Savvyio.Handlers namespace that consist of extension methods for the IFireForgetRegistry{TRequest} interface: RegisterAsync
- ADDED IFireForgetActivator interface in the Savvyio.Handlers namespace that specifies a way of invoking Fire-and-Forget/In-Only MEP delegates that handles TRequest
- ADDED IFireForgetHandler interface in the Savvyio.Handlers namespace that defines a generic handler that uses Fire-and-Forget/In-Only MEP
- ADDED IFireForgetRegistry interface in the Savvyio.Handlers namespace that specifies a Fire-and-Forget/In-Only MEP registry that store delegates responsible of handling type TRequest
- ADDED IRequestReplyActivator interface in the Savvyio.Handlers namespace that specifies a way of invoking Request-Reply/In-Out MEP delegates that handles TRequest
- ADDED IRequestReplyHandler interface in the Savvyio.Handlers namespace that defines a generic handler that uses Request-Reply/In-Out MEP
- ADDED IRequestReplyRegistry interface in the Savvyio.Handlers namespace that specifies a Request-Reply/In-Out MEP registry that store delegates responsible of handling type TRequest
- ADDED OrphanedHandlerException class in the Savvyio.Dispatchers namespace that provides the exception that is thrown when an IHandler{TRequest} implementation cannot be resolved
- ADDED RequestReplyRegistryExtensions class in the Savvyio.Handlers namespace that consist of extension methods for the IRequestReplyRegistry{TRequest} interface: RegisterAsync

#### Savvyio.Domain

- ADDED Aggregate class in the Savvyio.Domain namespace that represents the base class from which all implementations of an Aggregate Root (as specified in Domain Driven Design) should derive
- ADDED AggregateRoot class in the Savvyio.Domain namespace that provides a way to cover the pattern of an Aggregate Root as specified in Domain Driven Design
- ADDED DomainEvent class in the Savvyio.Domain namespace that provides a default implementation of the IDomainEvent interface
- ADDED DomainEventDispatcher class in the Savvyio.Domain namespace that provides a default implementation of the IDomainEventDispatcher interface
- ADDED DomainEventDispatcherExtensions class in the Savvyio.Domain namespace that consist of extension methods for the IDomainEventDispatcher interface: RaiseMany, RaiseManyAsync
- ADDED DomainEventExtensions class in the Savvyio.Domain namespace that consist of extension methods for the IDomainEvent interface: GetEventId, GetTimestamp
- ADDED DomainEventHandler class in the Savvyio.Domain namespace that provides a generic and consistent way of handling Domain Event (as specified in Domain Driven Design) objects that implements the IDomainEvent interface
- ADDED Entity class in the Savvyio.Domain namespace that provides a way to cover the pattern of an Entity as specified in Domain Driven Design
- ADDED IAggregateRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting persistent repositories (CRUD) that is optimized for Domain Driven Design
- ADDED IAggregateRoot interface in the Savvyio.Domain namespace that defines a marker interface of an Aggregate as specified in Domain Driven Design
- ADDED IDomainEvent interface in the Savvyio.Domain namespace that defines a marker interface that specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be made aware of
- ADDED IDomainEventDispatcher interface in the Savvyio.Domain namespace that defines a Domain Event dispatcher that uses Fire-and-Forget/In-Only MEP
- ADDED IDomainEventHandler interface in the Savvyio.Domain namespace that specifies a handler responsible for objects that implements the IDomainEvent interface
- ADDED IEntity interface in the Savvyio.Domain namespace that defines an Entity as specified in Domain Driven Design
- ADDED SavvyioOptionsExtensions class in the Savvyio.Domain namespace that consist of extension methods for the SavvyioOptions class: AddDomainEventHandler, AddDomainEventDispatcher
- ADDED SingleValueObject class in the Savvyio.Domain namespace that provides an implementation of ValueObject tailored for handling a single value
- ADDED ValueObject class in the Savvyio.Domain namespace that represents an object whose equality is based on the value rather than identity as specified in Domain Driven Design
- ADDED ITracedAggregateRepository interface in the Savvyio.Domain.EventSourcing namespace that defines a generic way of abstracting traced read- and writable repositories (CRud) that is optimized for Domain Driven Design
- ADDED ITracedAggregateRoot interface in the Savvyio.Domain.EventSourcing namespace that defines an Event Sourcing capable contract of an Aggregate as specified in Domain Driven Design
- ADDED IAggregateRoot interface in the Savvyio.Domain.EventSourcing namespace that specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of

#### Savvyio.Domain.EventSourcing

- ADDED TracedAggregateRoot class in the Savvyio.Domain.EventSourcing namespace that provides a way to cover the pattern of an Aggregate as specified in Domain Driven Design that is optimized for Event Sourcing
- ADDED TracedDomainEvent class in the Savvyio.Domain.EventSourcing namespace that provides a default implementation of something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of
- ADDED TracedDomainEventExtensions class in the Savvyio.Domain.EventSourcing namespace that consist of extension methods for the ITracedDomainEvent interface: SetAggregateVersion, GetAggregateVersion, GetMemberType

#### Savvyio.EventDriven

- ADDED IIntegrationEvent interface in the Savvyio.EventDriven namespace that defines a marker interface that specifies something that happened when an Aggregate was successfully persisted and you want other subsystems (out-process/inter-application) to be made aware of
- ADDED IIntegrationEventDispatcher interface in the Savvyio.EventDriven namespace that defines an Integration Event dispatcher that uses Fire-and-Forget/In-Only MEP
- ADDED IIntegrationEventHandler interface in the Savvyio.EventDriven namespace that specifies a handler responsible for objects that implements the IIntegrationEvent interface
- ADDED IntegrationEvent class in the Savvyio.EventDriven namespace that provides a default implementation of of the IIntegrationEvent interface
- ADDED IntegrationEventDispatcher class in the Savvyio.EventDriven namespace that provides a default implementation of of the IIntegrationEventDispatcher interface
- ADDED IntegrationEventExtensions class in the Savvyio.EventDriven namespace that consist of extension methods for the IIntegrationEvent interface: GetEventId, GetTimestamp, GetMemberType
- ADDED IntegrationEventHandler class in the Savvyio.EventDriven namespace that provides a generic and consistent way of handling Integration Event objects that implements the IIntegrationEvent interface
- ADDED SavvyioOptionsExtensions class in the Savvyio.EventDriven namespace that consist of extension methods for the SavvyioOptions class: AddIntegrationEventHandler, AddIntegrationEventDispatcher

#### Savvyio.Extensions.Dapper

- ADDED DapperDataAccessObject class in the Savvyio.Extensions.Dapper namespace that provides a default implementation of the IPersistentDataAccessObject{T,TOptions} interface to serve as an abstraction layer before the actual I/O communication towards a data store using Dapper
- ADDED DapperDataStore class in the Savvyio.Extensions.Dapper namespace that provides a default implementation of the IDapperDataStore interface to support the actual I/O communication towards a data store using Dapper
- ADDED DapperDataStoreOptions class in the Savvyio.Extensions.Dapper namespace that provides configuration options for IDapperDataStore
- ADDED DapperOptions class in the Savvyio.Extensions.Dapper namespace that specifies options that is related to Dapper concept of CommandDefinition
- ADDED IDapperDataStore interface in the Savvyio.Extensions.Dapper namespace that defines a generic way to support the actual I/O communication towards a data store optimized for Dapper

#### Savvyio.Extensions.DependencyInjection

- ADDED IDataStore interface in the Savvyio.Extensions.DependencyInjection namespace that defines a generic way to support multiple implementations that does the actual I/O communication towards a data store
- ADDED SavvyioDependencyInjectionOptions class in the Savvyio.Extensions.DependencyInjection namespace that specifies options that is related to setting up Savvy I/O services
- ADDED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection namespace that consist of extension methods for the IServiceCollection interface: AddDataStore, AddServiceLocator, AddSavvyIO
- ADDED IDataAccessObject interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of persistent data access based on the Data Access Object pattern
- ADDED IDeletableDataAccessObject interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of deletable data access objects (cruD)
- ADDED IPersistentDataAccessObject interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of the actual I/O communication with a data store that is responsible of persisting data (CRUD)
- ADDED IReadableDataAccessObject interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of readable data access objects (cRud)
- ADDED IWritableDataAccessObject interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of writable data access objects (CrUd)
- ADDED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Data namespace that consist of extension methods for the IServiceCollection interface: AddDataAccessObject
- ADDED IDeletableRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of deletable repositories (cruD)
- ADDED IPersistentRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of persistent repositories (CRUD)
- ADDED IReadableRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of readable repositories (cRud)
- ADDED IRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of persistent data access based on the Repository pattern
- ADDED ISearchableRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of searchable repositories (cRud)
- ADDED IUnitOfWork interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations that bundles transactions from multiple IPersistentRepository{TEntity,TKey} calls into a single unit
- ADDED IWritableRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of writable repositories (CrUd)
- ADDED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Domain namespace that consist of extension methods for the IServiceCollection interface: AddRepository

#### Savvyio.Extensions.DependencyInjection.Dapper

- ADDED DapperDataAccessObject class in the Savvyio.Extensions.DependencyInjection.Dapper namespace that provides a default implementation of the IPersistentDataAccessObject{T,TOptions} interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication towards a data store using Dapper
- ADDED DapperDataStore class in the Savvyio.Extensions.DependencyInjection.Dapper namespace that provides a default implementation of the IDapperDataStore{TMarker} interface to support the actual I/O communication towards a data store using Dapper
- ADDED DapperDataStoreOptions class in the Savvyio.Extensions.DependencyInjection.Dapper namespace that provides configuration options for IDapperDataStore{TMarker}
- ADDED IDapperDataStore interface in the Savvyio.Extensions.DependencyInjection.Dapper namespace that defines a generic way to support multiple implementations that does the actual I/O communication towards a data store optimized for Dapper
- ADDED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Dapper namespace that consist of extension methods for the IServiceCollection interface: AddDapperDataStore, AddDapperDataAccessObject

#### Savvyio.Extensions.DependencyInjection.Domain

- ADDED IAggregateRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of persistent repositories (CRUD) that is optimized for Domain Driven Design
- ADDED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Domain namespace that consist of extension methods for the IServiceCollection interface: AddAggregateRepository
- ADDED ITracedAggregateRepository interface in the Savvyio.Extensions.DependencyInjection.Domain.EventSourcing namespace that defines a generic way to support multiple implementations traced read- and writable repositories (CRud) that is optimized for Domain Driven Design
- ADDED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Domain.EventSourcing namespace that consist of extension methods for the IServiceCollection interface: AddTracedAggregateRepository

#### Savvyio.Extensions.DependencyInjection.EFCore

- ADDED EfCoreDataAccessObject class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides a default implementation of the IPersistentDataAccessObject{T,TOptions} interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core
- ADDED EfCoreDataStore class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides a default implementation of the IEfCoreDataStore{TMarker} interface to support the actual I/O communication towards a data store using Microsoft Entity Framework Core
- ADDED EfCoreDataStoreOptions class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides configuration options for IEfCoreDataStore{TMarker}
- ADDED EfCoreDbContext class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides a default implementation of the DbContext class to support Savvy I/O extensions of Microsoft Entity Framework Core in multiple implementations
- ADDED EfCoreRepository class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides a default implementation of the IPersistentRepository{TEntity,TKey,TMarker} interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core
- ADDED IEfCoreDataStore interface in the Savvyio.Extensions.DependencyInjection.EFCore namespace that defines a generic way to support multiple implementations that does the actual I/O communication towards a data store using Microsoft Entity Framework Core
- ADDED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that consist of extension methods for the IServiceCollection interface: AddEfCoreDataStore, AddEfCoreRepository, AddEfCoreDataAccessObject

#### Savvyio.Extensions.DependencyInjection.EFCore.Domain

- ADDED EfCoreAggregateDataStore class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain namespace that provides an implementation of the EfCoreDataStore that is optimized for Domain Driven Design and the concept of Aggregate Root
- ADDED EfCoreAggregateRepository class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain namespace that provides an implementation of the EfCoreRepository{TEntity,TKey} that is optimized for Domain Driven Design
- ADDED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain namespace that consist of extension methods for the IServiceCollection interface: AddEfCoreAggregateDataStore, AddEfCoreAggregateRepository

#### Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing

- ADDED EfCoreTracedAggregateRepository class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing namespace that provides an implementation of the EfCoreTracedAggregateRepository{TEntity,TKey} that is optimized for Domain Driven Design and Event Sourcing
- ADDED ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing namespace that consist of extension methods for the IServiceCollection interface: AddEfCoreTracedAggregateRepository

#### Savvyio.Extensions.Dispatchers

- ADDED IMediator interface in the Savvyio.Extensions namespace that defines a mediator to encapsulate requests (Fire-and-Forget/In-Only) and request/response (Request-Reply/In-Out) message exchange patterns
- ADDED Mediator class in the Savvyio.Extensions namespace that provides a default implementation of the IMediator interface
- ADDED SavvyioOptionsExtensions class in the Savvyio.Extensions namespace that consist of extension methods for the SavvyioOptions class: AddMediator

#### Savvyio.Extensions.EFCore

- ADDED EfCoreDataAccessObject class in the Savvyio.Extensions.EFCore namespace that provides a default implementation of the IPersistentDataAccessObject{T,TOptions} interface to serve as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core
- ADDED EfCoreDataStore class in the Savvyio.Extensions.EFCore namespace that provides a default implementation of the IEfCoreDataStore interface to support the actual I/O communication towards a data store using Microsoft Entity Framework Core
- ADDED EfCoreDataStoreOptions class in the Savvyio.Extensions.EFCore namespace that provides configuration options for IEfCoreDataStore
- ADDED EfCoreDbContext class in the Savvyio.Extensions.EFCore namespace that provides a default implementation of the DbContext class to support Savvy I/O extensions of Microsoft Entity Framework Core
- ADDED EfCoreRepository class in the Savvyio.Extensions.EFCore namespace that provides a default implementation of the IPersistentRepository{TEntity,TKey} interface to serve as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core
- ADDED IEfCoreDataStore interface in the Savvyio.Extensions.EFCore namespace that defines a generic way to support the actual I/O communication towards a data store using Microsoft Entity Framework Core

#### Savvyio.Extensions.EFCore.Domain

- ADDED DomainEventDispatcherExtensions class in the Savvyio.Extensions.EFCore.Domain namespace that consist of extension methods for the IDomainEventDispatcher interface: RaiseMany, RaiseManyAsync
- ADDED EfCoreAggregateDataStore class in the Savvyio.Extensions.EFCore.Domain namespace that provides an implementation of the EfCoreDataStore that is optimized for Domain Driven Design and the concept of Aggregate Root
- ADDED EfCoreAggregateRepository class in the Savvyio.Extensions.EFCore.Domain namespace that provides an implementation of the EfCoreRepository{TEntity,TKey} that is optimized for Domain Driven Design

#### Savvyio.Extensions.EFCore.Domain.EventSourcing

- ADDED EfCoreTracedAggregateEntity class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that provides a generic way for EF Core to surrogate and support an implementation of ITracedAggregateRoot{TKey}
- ADDED EfCoreTracedAggregateEntityExtensions class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that consist of extension methods for the EfCoreTracedAggregateEntity{TEntity,TKey} class: ToTracedDomainEvent
- ADDED EfCoreTracedAggregateEntityOptions class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that specifies configuration options for EfCoreTracedAggregateEntity
- ADDED EfCoreTracedAggregateRepository class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that provides an implementation of the EfCoreTracedAggregateRepository{TEntity,TKey} that is optimized for Domain Driven Design and Event Sourcing
- ADDED ModelBuilderExtensions class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that consist of extension methods for the IServiceCollection interface: AddEventSourcing
- ADDED TracedDomainEventExtensions class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that consist of extension methods for the ITracedDomainEvent interface: ToByteArray

#### Savvyio.Queries

- ADDED IQuery interface in the Savvyio.Queries namespace that defines a marker interface that specifies something that returns data
- ADDED IQueryDispatcher interface in the Savvyio.Queries namespace that defines a Query dispatcher that uses Request-Reply/In-Out MEP
- ADDED IQueryHandler interface in the Savvyio.Queries namespace that defines a handler responsible for objects that implements the IQuery interface
- ADDED Query class in the Savvyio.Queries namespace that provides a default implementation of the IQuery{TResult} interface
- ADDED QueryDispatcher class in the Savvyio.Queries namespace that provides a default implementation of the IQueryDispatcher interface
- ADDED QueryHandler class in the Savvyio.Queries namespace that defines a generic and consistent way of handling Query objects that implements the IQuery interface
- ADDED SavvyioOptionsExtensions class in the Savvyio.Queries namespace that consist of extension methods for the SavvyioOptions class: AddQueryHandler, AddQueryDispatcher