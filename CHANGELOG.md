# Changelog

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

For more details, please refer to `PackageReleaseNotes.txt` on a per assembly basis in the `.nuget` folder.

## [2.1.0] - TBD

### Added

#### Savvyio.Core

- MessageExtensions class in the Savvyio.Messaging.Cryptography namespace that consist of extension methods for the IMessage{T} interface: Sign{T} and CheckSignature{T}
- MessageExtensions class in the Savvyio.Messaging namespace that consist of extension methods for the IMessage{T} interface: Clone{T}

#### Savvyio.Extensions.Newtonsoft.Json

- NewtonsoftJsonMarshaller class in the Savvyio.Extensions.Newtonsoft.Json namespace was extended to include a new static method: Default (that provides a default instance of the NewtonsoftJsonMarshaller class optimized for messaging)

#### Savvyio.Extensions.SimpleQueueService

- AmazonMessageOptions class in the Savvyio.Extensions.SimpleQueueService namespace was extended with a new read-only property, ClientConfigurations, that can be set using the ConfigureClient method
- ClientConfigExtensions class in the Savvyio.Extensions.SimpleQueueService namespace that consist of extension methods for the ClientConfig class: IsValid, SimpleQueueService and SimpleNotificationService

#### Savvyio.Extensions.Text.Json

- JsonMarshaller class in the Savvyio.Extensions.Text.Json namespace was extended to include a new static method: Default (that provides a default instance of the JsonMarshaller class optimized for messaging)

### Changed

#### Savvyio.Commands

- MessageExtensions class in the Savvyio.Commands.Messaging.Cryptography namespace was removed to favor the new generic equivalent in the Savvyio.Messaging.Cryptography namespace

#### Savvyio.EventDriven

- MessageExtensions class in the Savvyio.EventDriven.Messaging.Cryptography namespace was removed to favor the new generic equivalent in the Savvyio.Messaging.Cryptography namespace
- CloudEventExtensions class in the Savvyio.EventDriven.Messaging.CloudEvents.Cryptography namespace was extended with new extension methods for the ICloudEvent{T} interface: CheckSignature{T}

#### Savvyio.Extensions.SimpleQueueService

- AmazonCommandQueue class in the Savvyio.Extensions.SimpleQueueService.Commands namespace to use the ClientConfigurations property when configured; otherwise the Endpoint property is used as it has previously (both properties are part of AmazonMessageOptions)
- AmazonEventBus class in the Savvyio.Extensions.SimpleQueueService.EventDriven namespace to use the ClientConfigurations property when configured; otherwise the Endpoint property is used as it has previously (both properties are part of AmazonMessageOptions)
- AmazonMessage{TRequest} class in the Savvyio.Extensions.SimpleQueueService namespace to use the ClientConfigurations property when configured; otherwise the Endpoint property is used as it has previously (both properties are part of AmazonMessageOptions)

## [2.0.0] - 2024-02-11

### Added

#### Savvyio.Commands

- MessageExtensions class in the Savvyio.Commands.Messaging.Cryptography namespace that consist of extension methods for the IMessage{T} interface: Sign{T}

#### Savvyio.Core

- ISignedMessage{T} interface in the Savvyio.Messaging.Cryptography namespace that defines a generic way to wrap an IRequest inside a cryptographically signed message
- SignedMessage{T} record in the Savvyio.Messaging.Cryptography namespace that provides a default implementation of the ISignedMessage{T} interface
- SignedMessageOptions class in the Savvyio.Messaging.Cryptography namespace that specifies options that is related to wrapping an IRequest implementation inside a cryptographically signed message
- IMarshaller interface in the Savvyio namespace that defines methods for serializing and deserializing objects to and from a Stream
- MetadataExtensions class in the Savvyio namespace was extended with new extension methods for the IMetadata interface: GetMemberType{T}
- ICommand interface in the Savvyio.Commands namespace that defines a marker interface that specifies an intention to do something (e.g. change the state)
- ICommandDispatcher interface in the Savvyio.Commands namespace that defines a Command dispatcher that uses Fire-and-Forget/In-Only MEP
- ICommandHandler interface in the Savvyio.Commands namespace that defines a handler responsible for objects that implements the ICommand interface
- ITracedAggregateRepository interface in the Savvyio.Domain.EventSourcing namespace that defines a generic way of abstracting traced read- and writable repositories (CRud) that is optimized for Domain Driven Design
- ITracedAggregateRoot interface in the Savvyio.Domain.EventSourcing namespace that defines an Event Sourcing capable contract of an Aggregate as specified in Domain Driven Design
- ITracedDomainEvent interface in the Savvyio.Domain.EventSourcing namespace that specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of
- IAggregateRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting persistent repositories (CRUD) that is optimized for Domain Driven Design
- IAggregateRoot interface in the Savvyio.Domain namespace that defines a marker interface of an Aggregate as specified in Domain Driven Design
- IDomainEvent interface in the Savvyio.Domain namespace that defines a marker interface that specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be made aware of
- IDomainEventDispatcher interface in the Savvyio.Domain namespace that defines a Domain Event dispatcher that uses Fire-and-Forget/In-Only MEP
- IDomainEventHandler interface in the Savvyio.Domain namespace that specifies a handler responsible for objects that implements the IDomainEvent interface
- IEntity interface in the Savvyio.Domain namespace that defines an Entity as specified in Domain Driven Design
- IIntegrationEvent interface in the Savvyio.EventDriven namespace that defines a marker interface that specifies something that happened when an Aggregate was successfully persisted and you want other subsystems (out-process/inter-application) to be made aware of
- IIntegrationEventDispatcher interface in the Savvyio.EventDriven namespace that defines an Integration Event dispatcher that uses Fire-and-Forget/In-Only MEP
- IIntegrationEventHandler interface in the Savvyio.EventDriven namespace that specifies a handler responsible for objects that implements the IIntegrationEvent interface
- IQuery interface in the Savvyio.Queries namespace that defines a marker interface that specifies something that returns data
- IQueryDispatcher interface in the Savvyio.Queries namespace that defines a Query dispatcher that uses Request-Reply/In-Out MEP
- IQueryHandler interface in the Savvyio.Queries namespace that defines a handler responsible for objects that implements the IQuery interface
- ISignedCloudEvent{T} interface in the Savvyio.EventDriven.Messaging.CloudEvents.Cryptography namespace that defines a generic way to wrap an IRequest inside a CloudEvents compliant message format
- ICloudEvent{T} interface in the Savvyio.EventDriven.Messaging.CloudEvents namespace that defines a generic way to wrap an IRequest inside a CloudEvents compliant message format
- AssemblyContext class in the Savvyio.Reflection namespace that provides a set of static methods and properties to manage and filter assemblies in the current application domain

#### Savvio.EventDriven

- MessageExtensions class in the Savvyio.EventDriven.Messaging.Cryptography namespace that consist of extension methods for the IMessage{T} interface: Sign{T}
- CloudEventExtensions class in the Savvyio.EventDriven.Messaging.CloudEvents.Cryptography namespace that consist of extension methods for the ICloudEvent{T} interface: Sign{T}
- SignedCloudEvent{T} class in the Savvyio.EventDriven.Messaging.CloudEvents.Cryptography namespace that provides a default implementation of the ISignedCloudEvent{T} interface
- CloudEvent{T} class in the Savvyio.EventDriven.Messaging.CloudEvents namespace that provides a default implementation of the ICloudEvent{T} interface
- MessageExtensions class in the Savvyio.EventDriven.Messaging.CloudEvents namespace that consist of extension methods for the IMessage{T} interface: ToCloudEvent{T}

#### Savvyio.Extensions.DependencyInjection

- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection namespace was extended with new extension methods for the IServiceCollection interface: AddMarshaller{TService}

#### Savvyio.Extensions.Newtonsoft.Json

- MessageConverter class in the Savvyio.Extensions.Newtonsoft.Json.Converters namespace that converts an IMessage{T} (or derived) to or from JSON
- RequestConverter class in the Savvyio.Extensions.Newtonsoft.Json.Converters namespace that converts an IRequest to or from JSON
- JsonConverterExtensions class in the Savvyio.Extensions.Newtonsoft.Json namespace was extended with new extension methods for the JsonConverter class: AddMessageConverter, AddRequestConverter
- NewtonsoftJsonMarshaller class in the Savvyio.Extensions.Newtonsoft.Json namespace that provides a class for marshalling data using the Newtonsoft JSON library

#### Savvyio.Extensions.SimpleQueueService

- AmazonMessageReceiveOptions class in the Savvyio.Extensions.SimpleQueueService namespace that provides options that is related to receive operations on AWS SQS

#### Savvyio.Extensions.Text.Json

- DateTimeConverter class in the Savvyio.Extensions.Text.Json.Converters namespace that converts a DateTime value to or from JSON using ISO8601
- DateTimeOffsetConverter class in the Savvyio.Extensions.Text.Json.Converters namespace that converts a DateTimeOffset value to or from JSON using ISO8601
- MessageConverter class in the Savvyio.Extensions.Text.Json.Converters namespace that converts IMessage{T} (or derived) implementations to or from JSON
- MetadataDictionaryConverter class in the Savvyio.Extensions.Text.Json.Converters namespace that converts IMetadataDictionary implementations to or from JSON
- RequestConverter class in the Savvyio.Extensions.Text.Json.Converters namespace that converts IRequest implementations to or from JSON
- JsonConverterExtensions class in the Savvyio.Extensions.Text.Json namespace that consist of extension methods for the JsonConverter class: RemoveAllOf, RemoveAllOf{T}, AddMetadataDictionaryConverter, AddMessageConverter, AddRequestConverter, AddDateTimeConverter and AddDateTimeOffsetConverter
- JsonMarshaller class in the Savvyio.Extensions.Text.Json namespace that provides a class for marshalling data using native JSON support in .NET
- JsonSerializerOptionsExtensions class in the Savvyio.Extensions.Text.Json namespace that consist of extension methods for the JsonSerializerOptions class: Clone

### Changed

#### Savvyio.Commands

- EncloseToMessage{T} extension method on the CommandExtensions class in the Savvyio.Commands.Messaging namespace to ToMessage{T}
- ToMessage{T} extension method on the CommandExtensions class in the Savvyio.Commands.Messaging namespace to include a string that describes the type of command
- MemoryCommandQueue class in the Savvyio.Commands.Messaging namespace to InMemoryCommandQueue (consistency with Microsoft naming convention)

#### Savvyio.Core

- Time property on the IMessage{T} interface in the Savvyio.Messaging namespace from a string signature to a nullable DateTime signature
- Time property on the Message{T} class in the Savvyio.Messaging namespace from a string signature to a nullable DateTime signature
- SetTimestamp{T} extension method on the MetadataExtensions class in the Savvyio namespace to include an optional nullable DateTime parameter
- Request record in the Savvyio namespace to automatically set the member type of the current implementation in the default constructor
- ReceiveAsync method on the IReceiver{TRequest} interface in the Savvyio.Messaging namespace from Task{IEnumerable{IMessage{TRequest}}} to IAsyncEnumerable{IMessage{TRequest}}

#### Savvyio.Domain

- SerializableAttribute and ISerializable implementations was removed from all custom exceptions due to Microsoft decision on deprecating most of the legacy serialization infrastructure https://github.com/dotnet/docs/issues/34893

#### Savvyio.Domain.EventSourcing

- TracedDomainEvent record in the Savvyio.Domain.EventSourcing namespace to exclude optional Type parameter from the constructor

#### Savvyio.EventDriven

- EncloseToMessage{T} extension method on the IntegrationEventExtensions class in the Savvyio.EventDriven.Messaging namespace to ToMessage{T}
- ToMessage{T} extension method on the IntegrationEventExtensions class in the Savvyio.EventDriven.Messaging namespace to include a string that describes the type of event
- MemoryEventBus class in the Savvyio.EventDriven.Messaging namespace to InMemoryEventBus (consistency with Microsoft naming convention)

#### Savvyio.Extensions.EFCore

- EfCoreRepository{TEntity, TKey} class in the Savvyio.Extensions.EFCore namespace to allow access to a new protected property, Set, that returns DbSet{TEntity} and also marked remaining members as virtual
- EfCoreDataSource class in the Savvyio.Extensions.EFCore namespace to use EfCoreDataSourceOptions instead of IOptions{EfCoreDataSourceOptions} on the constructor
- EfCoreDbContext class in the Savvyio.Extensions.EFCore namespace to use EfCoreDataSourceOptions instead of IOptions{EfCoreDataSourceOptions} on the constructor

#### Savvyio.Extensions.EFCore.Domain

- EfCoreAggregateDataSource class in the Savvyio.Extensions.EFCore.Domain namespace to use EfCoreDataSourceOptions instead of IOptions{EfCoreDataSourceOptions} on the constructor

#### Savvyio.Extensions.DependencyInjection.EFCore

- EfCoreDataSource class in the Savvyio.Extensions.EFCore namespace to use EfCoreDataSourceOptions instead of IOptions{EfCoreDataSourceOptions} on the constructor
- EfCoreDbContext class in the Savvyio.Extensions.EFCore namespace to use EfCoreDataSourceOptions instead of IOptions{EfCoreDataSourceOptions} on the constructor

#### Savvyio.Extensions.DependencyInjection.EFCore.Domain

- EfCoreAggregateDataSource{TMarker} class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain namespace to use EfCoreDataSourceOptions instead of IOptions{EfCoreDataSourceOptions} on the constructor

#### Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing

- EfCoreTracedAggregateRepository{TEntity, TKey, TMarker} class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing namespace to include an IMarshaller interface in the constructor that is used when converting between ITracedDomainEvent and arbitrary data
- ToTracedDomainEvent{TEntity, TKey} extension method on the EfCoreTracedAggregateEntityExtensions class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace to include an IMarshaller interface that is used when converting a traced domain event type into a deserialized version of ITracedDomainEvent
- EfCoreTracedAggregateRepository{TEntity, TKey} class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace to include an IMarshaller interface in the constructor that is used when converting between ITracedDomainEvent and arbitrary data
- ToByteArray extension method on the TracedDomainEventExtensions class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace to include an IMarshaller interface that is used when converting an ITracedDomainEvent into an array of bytes

#### Savvyio.Extensions.DependencyInjection.SimpleQueueService

- AmazonCommandQueue{TMarker} class in the Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands namespace was extended to include an IMarshaller interface in the constructor that is used when converting ICommand implementations to messages
- AmazonEventBus{TMarker} class in the Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven namespace was extended to include an IMarshaller interface in the constructor that is used when converting IIntegrationEvent implementations to messages
- AmazonCommandQueue{TMarker} class in the Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands namespace to use AmazonCommandQueueOptions{TMarker} instead of IOptions{AmazonCommandQueueOptions{TMarker}} on the constructor
- AmazonEventBus{TMarker} class in the Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands namespace to use AmazonEventBusOptions{TMarker} instead of IOptions{AmazonEventBusOptions{TMarker}} on the constructor

#### Savvyio.Extensions.EFCore.Domain.EventSourcing

- EfCoreTracedAggregateEntity{TEntity, TKey} class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace to include an IMarshaller interface in the constructor that is used when converting between ITracedDomainEvent into a serialized format

#### Savvyio.Extensions.SimpleQueueService

- AmazonCommandQueue class in the Savvyio.Extensions.SimpleQueueService.Commands namespace was extended to include an IMarshaller interface in the constructor that is used when converting ICommand implementations to messages
- AmazonEventBus class in the Savvyio.Extensions.SimpleQueueService.EventDriven namespace was extended to include an IMarshaller interface in the constructor that is used when converting IIntegrationEvent implementations to messages
- AmazonBus{TRequest} class in the Savvyio.Extensions.SimpleQueueService namespace to include an IMarshaller interface in the constructor that is used when converting models to messages
- AmazonMessage{TRequest} class in the Savvyio.Extensions.SimpleQueueService namespace to include an IMarshaller interface in the constructor that is used when converting models to messages
- AmazonQueue{TRequest} class in the Savvyio.Extensions.SimpleQueueService namespace to include an IMarshaller interface in the constructor that is used when converting models to messages
- AmazonMessageOptions class in the Savvyio.Extensions.SimpleQueueService namespace to include a ReceiveContext property as well as two constants (both of type Int32); MaxNumberOfMessages and MaxPollingWaitTimeInSeconds
- AmazonCommandQueue class in the Savvyio.Extensions.SimpleQueueService.Commands namespace to use AmazonCommandQueueOptions instead of IOptions{AmazonCommandQueueOptions} on the constructor
- AmazonEventBus class in the Savvyio.Extensions.SimpleQueueService.Commands namespace to use AmazonEventBusOptions instead of IOptions{AmazonEventBusOptions} on the constructor
- AmazonBus{TRequest} class in the Savvyio.Extensions.SimpleQueueService namespace to use AmazonMessageOptions instead of Action{AmazonMessageOptions} on the constructor

#### Savvyio.Extensions.Dispatchers

- UseAutomaticDispatcherDiscovery extension method on the SavvyioOptionsExtensions class in the Savvyio.Extensions namespace to include an optional boolean parameter; bruteAssemblyScanning that defaults to false (former implementation would be equivalent to true)
- UseAutomaticHandlerDiscovery extension method on the SavvyioOptionsExtensions class in the Savvyio.Extensions namespace to include an optional boolean parameter; bruteAssemblyScanning that defaults to false (former implementation would be equivalent to true)

### Removed

#### Savvyio.Commands

- ICommand interface in the Savvyio.Commands namespace was moved to the namespace equivalent in the Savvyio.Core assembly
- ICommandDispatcher interface in the Savvyio.Commands namespace was moved to the namespace equivalent in the Savvyio.Core assembly
- ICommandHandler interface in the Savvyio.Commands namespace was moved to the namespace equivalent in the Savvyio.Core assembly

### Savvyio.Core

- ReceiveAsyncOptions class from the Savvyio.Messaging namespace

#### Savvyio.Domain

- SerializableAttribute and ISerializable implementations was removed from all custom exceptions due to Microsoft decision on deprecating most of the legacy serialization infrastructure https://github.com/dotnet/docs/issues/34893
- ITracedAggregateRepository interface in the Savvyio.Domain.EventSourcing was moved to the namespace to the namespace equivalent in the Savvyio.Core assembly
- ITracedAggregateRoot interface in the Savvyio.Domain.EventSourcing was moved to the namespace to the namespace equivalent in the Savvyio.Core assembly
- ITracedDomainEvent interface in the Savvyio.Domain.EventSourcing was moved to the namespace to the namespace equivalent in the Savvyio.Core assembly
- IAggregateRepository interface in the Savvyio.Domain namespace was moved to the to the namespace equivalent in the Savvyio.Core assembly
- IAggregateRoot interface in the Savvyio.Domain namespace was moved to the to the namespace equivalent in the Savvyio.Core assembly
- IDomainEvent interface in the Savvyio.Domain namespace was moved to the to the namespace equivalent in the Savvyio.Core assembly
- IDomainEventDispatcher interface in the Savvyio.Domain namespace was moved to the to the namespace equivalent in the Savvyio.Core assembly
- IDomainEventHandler interface in the Savvyio.Domain namespace was moved to the to the namespace equivalent in the Savvyio.Core assembly
- IEntity interface in the Savvyio.Domain namespace was moved to the to the namespace equivalent in the Savvyio.Core assembly

#### Savvyio.EventDriven

- IIntegrationEvent interface in the Savvyio.EventDriven namespace to the was moved to the namespace equivalent in the Savvyio.Core assembly
- IIntegrationEventDispatcher interface in the Savvyio.EventDriven was moved to the namespace to the namespace equivalent in the Savvyio.Core assembly
- IIntegrationEventHandler interface in the Savvyio.EventDriven was moved to the namespace to the namespace equivalent in the Savvyio.Core assembly

#### Savvyio.Queries

- IQuery interface in the Savvyio.Queries was moved to the namespace to the namespace equivalent in the Savvyio.Core assembly
- IQueryDispatcher interface in the Savvyio.Queries was moved to the namespace to the namespace equivalent in the Savvyio.Core assembly
- IQueryHandler interface in the Savvyio.Queries was moved to the namespace to the namespace equivalent in the Savvyio.Core assembly

## [1.1.0] - 2022-12-06

### Added

#### Savvyio.Core

- IMessage{T} interface in the Savvyio.Messaging namespace that defines a generic way to wrap an IRequest inside a message
- IPointToPointChannel{TRequest} interface in the Savvyio.Messaging namespace that defines an interface for a bus that is used for interacting with other subsystems (out-process/inter-application) to do something (e.g., change the state)
- IPublisher{TRequest} interface in the Savvyio.Messaging namespace that defines a publisher/sender channel for interacting with other subsystems (out-process/inter-application) to be notified (e.g., made aware of something that has happened)
- IPublishSubscribeChannel{TRequest} interface in the Savvyio.Messaging namespace that defines an interface for a bus that is used for interacting with other subsystems (out-process/inter-application) to be notified (e.g., made aware of something that has happened)
- IReceiver{TRequest} interface in the Savvyio.Messaging namespace that defines a consumer/receiver channel used by subsystems to receive a command and perform one or more actions (e.g., change the state)
- ISender{TRequest} interface in the Savvyio.Messaging namespace that defines a producer/sender channel used for interacting with other subsystems (out-process/inter-application) to do something (e.g., change the state)
- ISubscriber{TRequest} interface in the Savvyio.Messaging namespace that defines a subscriber/receiver channel used by subsystems to subscribe to messages (typically events) to be made aware of something that has happened
- Message record in the Savvyio.Messaging namespace that provides a default implementation of the IMessage{T} interface
- MessageOptions class in the Savvyio.Messaging namespace that specifies options that is related to wrapping an IRequest implementation inside a message
- ReceiveAsyncOptions class in the Savvyio.Messaging namespace that specifies options that is related to implementations of the IReceiver{TRequest} interface
- SubscribeAsyncOptions class in the Savvyio.Messaging namespace that specifies options that is related to implementations of the ISubscriber{TRequest} interface

#### Savvyio.Commands

- CommandExtensions class in the Savvyio.Commands.Messaging namespace that consist of extension methods for the ICommand interface: EncloseToMessage{T}
- MemoryCommandQueue class in the Savvyio.Commands.Messaging namespace that provides an in-memory implementation of the IPointToPointChannel{TRequest} interface useful for unit testing and the likes thereof
- Command record in the Savvyio.Commands namespace to include a default/fallback correlation identifier

#### Savvyio.EventDriven

- IntegrationEventExtensions class in the Savvyio.EventDriven.Messaging namespace that consist of extension methods for the IIntegrationEvent interface: EncloseToMessage{T}
- MemoryEventBus class in the Savvyio.EventDriven.Messaging namespace that provides an in-memory implementation of the IPublishSubscribeChannel{TRequest} interface useful for unit testing and the likes thereof
- IntegrationEvent record in the Savvyio.EventDriven namespace to not care for member type (retrospective: this is only relevant for TracedDomainEvents)

#### Savvyio.Extensions.SimpleQueueService

- AmazonCommandQueue class in the Savvyio.Extensions.SimpleQueueService.Commands namespace that specifies options that provides a default implementation of the AmazonQueue{TRequest} class tailored for messages holding an ICommand implementation
- AmazonCommandQueueOptions class in the Savvyio.Extensions.SimpleQueueService.Commands namespace that specifies options that is related to AWS SQS
- AmazonEventBus class in the Savvyio.Extensions.SimpleQueueService.EventDriven namespace that specifies options that provides a default implementation of the AmazonBus{TRequest} class tailored for messages holding an IIntegrationEvent implementation
- StringExtensions class in the Savvyio.Extensions.SimpleQueueService.EventDriven namespace that consist of extension methods for the string class: ToSnsUri
- AmazonBus{TRequest} class in the Savvyio.Extensions.SimpleQueueService namespace that represents the base class from which all implementations in need of bus capabilities should derive
- AmazonMessage{TRequest} class in the Savvyio.Extensions.SimpleQueueService namespace that represents the base class from which all implementations of AWS SQS should derive
- AmazonMessageOptions class in the Savvyio.Extensions.SimpleQueueService namespace that specifies options that is related to AWS SQS and AWS SNS
- AmazonQueue{TRequest} class in the Savvyio.Extensions.SimpleQueueService namespace that represents the base class from which all implementations in need of queue capabilities should derive
- AmazonResourceNameOptions class in the Savvyio.Extensions.SimpleQueueService namespace that specifies options that is related to Amazon Resource Name (ARN)

#### Savvyio.Extensions.Newtonsoft.Json

- AggregateRootConverter{TKey} class in the Savvyio.Extensions.Newtonsoft.Json.Converters namespace that converts an AggregateRoot{TKey} to or from JSON
- ValueObjectConverter class in the Savvyio.Extensions.Newtonsoft.Json.Converters namespace that converts a ValueObject to or from JSON
- JsonConverterExtensions class in the Savvyio.Extensions.Newtonsoft.Json namespace that consist of extension methods for the JsonConverter class: AddValueObjectConverter, AddAggregateRootConverter{TKey}, AddMetadataDictionaryConverter

#### Savvyio.Extensions.DependencyInjection

- IPointToPointChannel{TRequest, TMarker} interface in the Savvyio.Extensions.DependencyInjection.Messaging namespace that defines a generic way to support multiple implementations of a bus that is used for interacting with other subsystems (out-process/inter-application) to do something (e.g., change the state)
- IPublisher{TRequest, TMarker} interface in the Savvyio.Extensions.DependencyInjection.Messaging namespace that defines a generic way to support multiple implementations of a publisher/sender channel for interacting with other subsystems (out-process/inter-application) to be notified (e.g., made aware of something that has happened)
- IPublishSubscribeChannel{TRequest, TMarker} interface in the Savvyio.Extensions.DependencyInjection.Messaging namespace that defines a generic way to support multiple implementations of a bus that is used for interacting with other subsystems (out-process/inter-application) to be notified (e.g., made aware of something that has happened)
- IReceiver{TRequest, TMarker} interface in the Savvyio.Extensions.DependencyInjection.Messaging namespace that defines a generic way to support multiple implementations of a consumer/receiver channel used by subsystems to receive a command and perform one or more actions (e.g., change the state)
- ISender{TRequest, TMarker} interface in the Savvyio.Extensions.DependencyInjection.Messaging namespace that defines a generic way to support multiple implementations of a producer/sender channel used for interacting with other subsystems (out-process/inter-application) to do something (e.g., change the state)
- ISubscriber{TRequest, TMarker} interface in the Savvyio.Extensions.DependencyInjection.Messaging namespace that defines a generic way to support multiple implementations of a subscriber/receiver channel used by subsystems to subscribe to messages (typically events) to be made aware of something that has happened
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Messaging namespace that consist of extension methods for the IServiceCollection interface: AddMessageQueue, AddMessageBus

#### Savvyio.Extensions.DependencyInjection.SimpleQueueService

- AmazonCommandQueue{TMarker} class in the Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands namespace that provides a default implementation of the AmazonQueue{TRequest} class tailored for messages holding an ICommand implementation
- AmazonCommandQueueOptions{TMarker} class in the Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands namespace that provides configuration options for AmazonCommandQueue{TMarker}
- AmazonEventBus{TMarker} class in the Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven namespace that provides a default implementation of the AmazonBus{TRequest} class tailored for messages holding an IIntegrationEvent implementation
- AmazonEventBusOptions{TMarker} class in the Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven namespace that provides configuration options for AmazonEventBus{TMarker}
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.SimpleQueueService namespace that consist of extension methods for the IServiceCollection interface: AddAmazonCommandQueue, AddAmazonEventBus

#### Savvyio.Extensions.EFCore.Domain.EventSourcing

- ToTracedDomainEvent extension method on the EfCoreTracedAggregateEntityExtensions class in the Savvyio.Extensions.EFCore.Domain.EventSourcing to not interfere with the casing of dictionary keys

## [1.0.0] - 2022-11-09

### Added

#### Savvyio.Commands

- Command record in the Savvyio.Commands namespace that provides a default implementation of the ICommand interface
- CommandDispatcher class in the Savvyio.Commands namespace that provides a default implementation of the ICommandDispatcher interface
- CommandHandler class in the Savvyio.Commands namespace that defines a generic and consistent way of handling Command objects that implements the ICommand interface
- ICommand interface in the Savvyio.Commands namespace that defines a marker interface that specifies an intention to do something (e.g. change the state)
- ICommandDispatcher interface in the Savvyio.Commands namespace that defines a Command dispatcher that uses Fire-and-Forget/In-Only MEP
- ICommandHandler interface in the Savvyio.Commands namespace that defines a handler responsible for objects that implements the ICommand interface
- SavvyioOptionsExtensions class in the Savvyio.Commands namespace that consist of extension methods for the SavvyioOptions class: AddCommandHandler, AddCommandDispatcher

#### Savvyio.Core

- HandlerFactory class in the Savvyio namespace that provides access to factory methods for creating and configuring generic handlers that supports MEP
- HandlerServicesDescriptor class in the Savvyio namespace that provides information, in a developer friendly way, about implementations of the IHandler{TRequest} interface such as name, declared members and what type of request they handle
- IDataSource interface in the Savvyio namespace that defines a marker interface that specifies the actual I/O communication with a source of data
- IHandler interface in the Savvyio namespace that defines a marker interface that specifies a handler
- IIdentity interface in the Savvyio namespace that an identity typically associated with a storage such as a database
- IMetadata interface in the Savvyio namespace that defines a generic way to associate metadata with any type of object
- IMetadataDictionary interface in the Savvyio namespace that defines a generic way to support metadata capabilities
- IRequest interface in the Savvyio namespace that defines a marker interface that specifies a request/model/event
- MetadataDictionary class in the Savvyio namespace that provides a default implementation of the IMetadataDictionary interface
- MetadataExtensions class in the Savvyio namespace that consist of extension methods for the IMetadata interface: GetCausationId, GetCorrelationId, SetCausationId, SetCorrelationId, SetEventId, SetTimestamp, SetMemberType, SaveMetadata, MergeMetadata
- MetadataFactory class in the Savvyio namespace that provides access to factory methods for maintaining metadata in models
- Request record in the Savvyio namespace that represents the base class from which all implementations of the IRequest interface should derive
- SavvyioOptions class in the Savvyio namespace that specifies options that is related to setting up Savvy I/O services
- SavvyioOptionsExtensions class in the Savvyio namespace that consist of extension methods for the SavvyioOptions class: AddDispatchers, AddHandlers
- TaskExtensions class in the Savvyio namespace that consist of extension methods for the Task{T} class: SingleOrDefaultAsync
- IDataStore interface in the Savvyio.Data namespace that defines a marker interface that specifies an abstraction of persistent data access based on the Data Access Object pattern
- IDeletableDataStore interface in the Savvyio.Data namespace that defines a generic way of abstracting deletable data access objects (cruD)
- IPersistentDataStore interface in the Savvyio.Data namespace that defines a generic way of abstracting persistent data access objects (CRUD)
- IReadableDataStore interface in the Savvyio.Data namespace that defines a generic way of abstracting readable data access objects (cRud)
- ISearchableDataStore interface in the Savvyio.Data namespace that defines a generic way of abstracting searchable data access objects (cRud)
- IWritableDataStore interface in the Savvyio.Data namespace that defines a generic way of abstracting writable data access objects (CrUd)
- Dispatcher class in the Savvyio.Dispatchers namespace that represents the base class from which all implementations of the dispatcher concept should derive
- FireForgetDispatcher class in the Savvyio.Dispatchers namespace that provides a generic dispatcher that uses Fire-and-Forget/In-Only MEP
- IDispatcher interface in the Savvyio.Dispatchers namespace that defines a marker interface that specifies a dispatcher that encapsulates how a set of objects interact
- IServiceLocator interface in the Savvyio.Dispatchers namespace that provides a generic way to locate implementations of service objects
- RequestReplyDispatcher class in the Savvyio.Dispatchers namespace that provides a generic dispatcher that uses Request-Reply/In-Out MEP
- ServiceLocator class in the Savvyio.Dispatchers namespace that provides a default implementation of the IServiceLocator interface
- DomainException class in the Savvyio.Domain namespace that is the exception that is thrown when a domain model is not in a valid state
- IDeletableRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting deletable repositories (cruD)
- IPersistentRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting persistent repositories (CRUD)
- IReadableRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting readable repositories (cRud)
- IRepository interface in the Savvyio.Domain namespace that defines a marker interface that specifies an abstraction of persistent data access based on the Repository Pattern
- ISearchableRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting searchable repositories (cRud)
- IUnitOfWork interface in the Savvyio.Domain namespace that defines a transaction that bundles multiple IRepository{TEntity,TKey}" calls into a single unit
- IWritableRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting writable repositories (CrUd)
- FireForgetRegistryExtensions class in the Savvyio.Handlers namespace that consist of extension methods for the IFireForgetRegistry{TRequest} interface: RegisterAsync
- IFireForgetActivator interface in the Savvyio.Handlers namespace that specifies a way of invoking Fire-and-Forget/In-Only MEP delegates that handles TRequest
- IFireForgetHandler interface in the Savvyio.Handlers namespace that defines a generic handler that uses Fire-and-Forget/In-Only MEP
- IFireForgetRegistry interface in the Savvyio.Handlers namespace that specifies a Fire-and-Forget/In-Only MEP registry that store delegates responsible of handling type TRequest
- IRequestReplyActivator interface in the Savvyio.Handlers namespace that specifies a way of invoking Request-Reply/In-Out MEP delegates that handles TRequest
- IRequestReplyHandler interface in the Savvyio.Handlers namespace that defines a generic handler that uses Request-Reply/In-Out MEP
- IRequestReplyRegistry interface in the Savvyio.Handlers namespace that specifies a Request-Reply/In-Out MEP registry that store delegates responsible of handling type TRequest
- OrphanedHandlerException class in the Savvyio.Dispatchers namespace that provides the exception that is thrown when an IHandler{TRequest} implementation cannot be resolved
- RequestReplyRegistryExtensions class in the Savvyio.Handlers namespace that consist of extension methods for the IRequestReplyRegistry{TRequest} interface: RegisterAsync

#### Savvyio.Domain

- Aggregate class in the Savvyio.Domain namespace that represents the base class from which all implementations of an Aggregate Root (as specified in Domain Driven Design) should derive
- AggregateRoot class in the Savvyio.Domain namespace that provides a way to cover the pattern of an Aggregate Root as specified in Domain Driven Design
- DomainEvent class in the Savvyio.Domain namespace that provides a default implementation of the IDomainEvent interface
- DomainEventDispatcher class in the Savvyio.Domain namespace that provides a default implementation of the IDomainEventDispatcher interface
- DomainEventDispatcherExtensions class in the Savvyio.Domain namespace that consist of extension methods for the IDomainEventDispatcher interface: RaiseMany, RaiseManyAsync
- DomainEventExtensions class in the Savvyio.Domain namespace that consist of extension methods for the IDomainEvent interface: GetEventId, GetTimestamp
- DomainEventHandler class in the Savvyio.Domain namespace that provides a generic and consistent way of handling Domain Event (as specified in Domain Driven Design) objects that implements the IDomainEvent interface
- Entity class in the Savvyio.Domain namespace that provides a way to cover the pattern of an Entity as specified in Domain Driven Design
- IAggregateRepository interface in the Savvyio.Domain namespace that defines a generic way of abstracting persistent repositories (CRUD) that is optimized for Domain Driven Design
- IAggregateRoot interface in the Savvyio.Domain namespace that defines a marker interface of an Aggregate as specified in Domain Driven Design
- IDomainEvent interface in the Savvyio.Domain namespace that defines a marker interface that specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be made aware of
- IDomainEventDispatcher interface in the Savvyio.Domain namespace that defines a Domain Event dispatcher that uses Fire-and-Forget/In-Only MEP
- IDomainEventHandler interface in the Savvyio.Domain namespace that specifies a handler responsible for objects that implements the IDomainEvent interface
- IEntity interface in the Savvyio.Domain namespace that defines an Entity as specified in Domain Driven Design
- SavvyioOptionsExtensions class in the Savvyio.Domain namespace that consist of extension methods for the SavvyioOptions class: AddDomainEventHandler, AddDomainEventDispatcher
- SingleValueObject record in the Savvyio.Domain namespace that provides an implementation of ValueObject tailored for handling a single value
- ValueObject record in the Savvyio.Domain namespace that represents an object whose equality is based on the value rather than identity as specified in Domain Driven Design
- ITracedAggregateRepository interface in the Savvyio.Domain.EventSourcing namespace that defines a generic way of abstracting traced read- and writable repositories (CRud) that is optimized for Domain Driven Design
- ITracedAggregateRoot interface in the Savvyio.Domain.EventSourcing namespace that defines an Event Sourcing capable contract of an Aggregate as specified in Domain Driven Design
- ITracedDomainEvent interface in the Savvyio.Domain.EventSourcing namespace that specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of

#### Savvyio.Domain.EventSourcing

- TracedAggregateRoot class in the Savvyio.Domain.EventSourcing namespace that provides a way to cover the pattern of an Aggregate as specified in Domain Driven Design that is optimized for Event Sourcing
- TracedDomainEvent record in the Savvyio.Domain.EventSourcing namespace that provides a default implementation of something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of
- TracedDomainEventExtensions class in the Savvyio.Domain.EventSourcing namespace that consist of extension methods for the ITracedDomainEvent interface: SetAggregateVersion, GetAggregateVersion, GetMemberType

#### Savvyio.EventDriven

- IIntegrationEvent interface in the Savvyio.EventDriven namespace that defines a marker interface that specifies something that happened when an Aggregate was successfully persisted and you want other subsystems (out-process/inter-application) to be made aware of
- IIntegrationEventDispatcher interface in the Savvyio.EventDriven namespace that defines an Integration Event dispatcher that uses Fire-and-Forget/In-Only MEP
- IIntegrationEventHandler interface in the Savvyio.EventDriven namespace that specifies a handler responsible for objects that implements the IIntegrationEvent interface
- IntegrationEvent record in the Savvyio.EventDriven namespace that provides a default implementation of of the IIntegrationEvent interface
- IntegrationEventDispatcher class in the Savvyio.EventDriven namespace that provides a default implementation of of the IIntegrationEventDispatcher interface
- IntegrationEventExtensions class in the Savvyio.EventDriven namespace that consist of extension methods for the IIntegrationEvent interface: GetEventId, GetTimestamp, GetMemberType
- IntegrationEventHandler class in the Savvyio.EventDriven namespace that provides a generic and consistent way of handling Integration Event objects that implements the IIntegrationEvent interface
- SavvyioOptionsExtensions class in the Savvyio.EventDriven namespace that consist of extension methods for the SavvyioOptions class: AddIntegrationEventHandler, AddIntegrationEventDispatcher

#### Savvyio.Extensions.Dapper

- DapperDataStore class in the Savvyio.Extensions.Dapper namespace that represents the base class from which all implementations of DapperDataStore{T,TOptions} should derive
- DapperDataSource class in the Savvyio.Extensions.Dapper namespace that provides a default implementation of the IDapperDataSource interface to support the actual I/O communication towards a data store using Dapper
- DapperDataSourceOptions class in the Savvyio.Extensions.Dapper namespace that provides configuration options for IDapperDataSource
- DapperQueryOptions class in the Savvyio.Extensions.Dapper namespace that specifies options that is related to DapperDataStore{T,TOptions}
- IDapperDataSource interface in the Savvyio.Extensions.Dapper namespace that defines a generic way to support the actual I/O communication towards a data store optimized for Dapper

#### Savvyio.Extensions.DapperExtensions

- DapperExtensionsDataStore class in the Savvyio.Extensions.DapperExtensions namespace that provides a default implementation of the DapperDataStore{T,TOptions} class
- DapperExtensionsQueryOptions class in the Savvyio.Extensions.DapperExtensions namespace that provides configuration options for DapperExtensionsDataStore{T}

#### Savvyio.Extensions.DependencyInjection

- IDataSource interface in the Savvyio.Extensions.DependencyInjection namespace that defines a generic way to support multiple implementations that does the actual I/O communication with a source of data
- SavvyioDependencyInjectionOptions class in the Savvyio.Extensions.DependencyInjection namespace that specifies options that is related to setting up Savvy I/O services
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection namespace that consist of extension methods for the IServiceCollection interface: AddDataSource, AddServiceLocator, AddSavvyIO
- ServiceLocatorOptions class in the in the Savvyio.Extensions.DependencyInjection that provides configuration options for IServiceLocator
- IDataStore interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of persistent data access based on the Data Access Object pattern
- IDeletableDataStore interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of deletable data access objects (cruD)
- IPersistentDataStore interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of the actual I/O communication with a data store that is responsible of persisting data (CRUD)
- IReadableDataStore interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of readable data access objects (cRud)
- ISearchableDataStore interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of searchable data access objects (cRud)
- IWritableDataStore interface in the Savvyio.Extensions.DependencyInjection.Data namespace that defines a generic way to support multiple implementations of writable data access objects (CrUd)
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Data namespace that consist of extension methods for the IServiceCollection interface: AddDataStore
- IDeletableRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of deletable repositories (cruD)
- IPersistentRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of persistent repositories (CRUD)
- IReadableRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of readable repositories (cRud)
- IRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of persistent data access based on the Repository pattern
- ISearchableRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of searchable repositories (cRud)
- IUnitOfWork interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations that bundles transactions from multiple IPersistentRepository{TEntity,TKey} calls into a single unit
- IWritableRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of writable repositories (CrUd)
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Domain namespace that consist of extension methods for the IServiceCollection interface: AddRepository

#### Savvyio.Extensions.DependencyInjection.Dapper

- DapperDataSource class in the Savvyio.Extensions.DependencyInjection.Dapper namespace that provides a default implementation of the IDapperDataSource{TMarker} interface to support the actual I/O communication towards a data store using Dapper
- DapperDataSourceOptions class in the Savvyio.Extensions.DependencyInjection.Dapper namespace that provides configuration options for IDapperDataSource{TMarker}
- IDapperDataSource interface in the Savvyio.Extensions.DependencyInjection.Dapper namespace that defines a generic way to support multiple implementations that does the actual I/O communication towards a data store optimized for Dapper
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Dapper namespace that consist of extension methods for the IServiceCollection interface: AddDapperDataSource, AddDapperDataStore

#### Savvyio.Extensions.DependencyInjection.DapperExtensions

- DapperExtensionsDataStore class in the Savvyio.Extensions.DependencyInjection.DapperExtensions namespace that provides a default implementation of the IPersistentDataStore{T,TOptions,TMarker} interface to support multiple implementations that is tailored for Plain Old CLR Objects (POCO) usage by DapperExtensions
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Dapper namespace that consist of extension methods for the IServiceCollection interface: AddDapperExtensionsDataStore

#### Savvyio.Extensions.DependencyInjection.Domain

- IAggregateRepository interface in the Savvyio.Extensions.DependencyInjection.Domain namespace that defines a generic way to support multiple implementations of persistent repositories (CRUD) that is optimized for Domain Driven Design
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Domain namespace that consist of extension methods for the IServiceCollection interface: AddAggregateRepository
- ITracedAggregateRepository interface in the Savvyio.Extensions.DependencyInjection.Domain.EventSourcing namespace that defines a generic way to support multiple implementations traced read- and writable repositories (CRud) that is optimized for Domain Driven Design
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.Domain.EventSourcing namespace that consist of extension methods for the IServiceCollection interface: AddTracedAggregateRepository

#### Savvyio.Extensions.DependencyInjection.EFCore

- DefaultEfCoreDataStore class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides a default implementation of the IPersistentDataStore{T,TOptions,TMarker} interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication with a source of data using Microsoft Entity Framework Core
- EfCoreDataSource class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides a default implementation of the IEfCoreDataSource{TMarker} interface to support multiple implementations that does the actual I/O communication with a source of data using Microsoft Entity Framework Core
- EfCoreDataSourceOptions class in the Savvyio.Extensions.EFCore namespace that provides configuration options for IEfCoreDataStore{TMarker}
- EfCoreDbContext class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides a default implementation of the DbContext class to support Savvy I/O extensions of Microsoft Entity Framework Core in multiple implementations
- EfCoreRepository class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides a default implementation of the IPersistentRepository{TEntity,TKey,TMarker} interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core
- EfCoreServiceOptions class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that provides configuration options for Microsoft Dependency Injection
- IEfCoreDataSource interface in the Savvyio.Extensions.DependencyInjection.EFCore namespace that defines a generic way to support multiple implementations that does the actual I/O communication towards a data store using Microsoft Entity Framework Core
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.EFCore namespace that consist of extension methods for the IServiceCollection interface: AddEfCoreDataSource, AddEfCoreRepository, AddEfCoreDataStore, AddDefaultEfCoreDataStore

#### Savvyio.Extensions.DependencyInjection.EFCore.Domain

- EfCoreAggregateDataSource class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain namespace that provides an implementation of the EfCoreDataSource that is optimized for Domain Driven Design and the concept of Aggregate Root
- EfCoreAggregateRepository class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain namespace that provides an implementation of the EfCoreRepository{TEntity,TKey} that is optimized for Domain Driven Design
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain namespace that consist of extension methods for the IServiceCollection interface: AddEfCoreAggregateDataSource, AddEfCoreAggregateRepository

#### Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing

- EfCoreTracedAggregateRepository class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing namespace that provides an implementation of the EfCoreTracedAggregateRepository{TEntity,TKey} that is optimized for Domain Driven Design and Event Sourcing
- ServiceCollectionExtensions class in the Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing namespace that consist of extension methods for the IServiceCollection interface: AddEfCoreTracedAggregateRepository

#### Savvyio.Extensions.Dispatchers

- IMediator interface in the Savvyio.Extensions namespace that defines a mediator to encapsulate requests (Fire-and-Forget/In-Only) and request/response (Request-Reply/In-Out) message exchange patterns
- Mediator class in the Savvyio.Extensions namespace that provides a default implementation of the IMediator interface
- SavvyioOptionsExtensions class in the Savvyio.Extensions namespace that consist of extension methods for the SavvyioOptions class: AddMediator, UseAutomaticDispatcherDiscovery, UseAutomaticHandlerDiscovery

#### Savvyio.Extensions.EFCore

- DefaultEfCoreDataStore class in the Savvyio.Extensions.EFCore namespace that provides a default implementation of the EfCoreDataStore{T,TOptions} class
- EfCoreDataSource class in the Savvyio.Extensions.EFCore namespace that provides a default implementation of the IEfCoreDataSource interface to support the actual I/O communication towards a data store using Microsoft Entity Framework Core
- EfCoreDataSourceOptions class in the Savvyio.Extensions.EFCore namespace that provides configuration options for IEfCoreDataStore
- EfCoreDataStore class in the Savvyio.Extensions.EFCore namespace that represents the base class from which all implementations of EfCoreDataStore{T,TOptions} should derive
- EfCoreDbContext class in the Savvyio.Extensions.EFCore namespace that provides a default implementation of the DbContext class to support Savvy I/O extensions of Microsoft Entity Framework Core in multiple implementations
- EfCoreQueryOptions class in the Savvyio.Extensions.EFCore namespace that provides configuration options that is related to DefaultEfCoreDataStore{T}
- EfCoreRepository class in the Savvyio.Extensions.EFCore namespace that provides a default implementation of the IPersistentRepository{TEntity,TKey,TMarker} interface to serve as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core
- IEfCoreDataStore interface in the Savvyio.Extensions.EFCore namespace that defines a generic way to support the actual I/O communication with a source of data - tailored to Microsoft Entity Framework Core

#### Savvyio.Extensions.EFCore.Domain

- DomainEventDispatcherExtensions class in the Savvyio.Extensions.EFCore.Domain namespace that consist of extension methods for the IDomainEventDispatcher interface: RaiseMany, RaiseManyAsync
- EfCoreAggregateDataSource class in the Savvyio.Extensions.EFCore.Domain namespace that provides an implementation of the EfCoreDataSource that is optimized for Domain Driven Design and the concept of Aggregate Root
- EfCoreAggregateRepository class in the Savvyio.Extensions.EFCore.Domain namespace that provides an implementation of the EfCoreRepository{TEntity,TKey} that is optimized for Domain Driven Design

#### Savvyio.Extensions.EFCore.Domain.EventSourcing

- EfCoreTracedAggregateEntity class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that provides a generic way for EF Core to surrogate and support an implementation of ITracedAggregateRoot{TKey}
- EfCoreTracedAggregateEntityExtensions class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that consist of extension methods for the EfCoreTracedAggregateEntity{TEntity,TKey} class: ToTracedDomainEvent
- EfCoreTracedAggregateEntityOptions class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that specifies configuration options for EfCoreTracedAggregateEntity
- EfCoreTracedAggregateRepository class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that provides an implementation of the EfCoreTracedAggregateRepository{TEntity,TKey} that is optimized for Domain Driven Design and Event Sourcing
- ModelBuilderExtensions class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that consist of extension methods for the IServiceCollection interface: AddEventSourcing
- TracedDomainEventExtensions class in the Savvyio.Extensions.EFCore.Domain.EventSourcing namespace that consist of extension methods for the ITracedDomainEvent interface: ToByteArray

#### Savvyio.Queries

- IQuery interface in the Savvyio.Queries namespace that defines a marker interface that specifies something that returns data
- IQueryDispatcher interface in the Savvyio.Queries namespace that defines a Query dispatcher that uses Request-Reply/In-Out MEP
- IQueryHandler interface in the Savvyio.Queries namespace that defines a handler responsible for objects that implements the IQuery interface
- Query record in the Savvyio.Queries namespace that provides a default implementation of the IQuery{TResult} interface
- QueryDispatcher class in the Savvyio.Queries namespace that provides a default implementation of the IQueryDispatcher interface
- QueryHandler class in the Savvyio.Queries namespace that defines a generic and consistent way of handling Query objects that implements the IQuery interface
- SavvyioOptionsExtensions class in the Savvyio.Queries namespace that consist of extension methods for the SavvyioOptions class: AddQueryHandler, AddQueryDispatcher

