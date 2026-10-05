#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.EventDriven;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.EventDriven.Messaging.CloudEvents.Cryptography;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.Text.Json.Converters;

public class CloudEventDiscoveryTest : Test
{
    public CloudEventDiscoveryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CloudEventDiscovery_ShouldIncludeConcreteTypesAndExcludeAbstractCandidates()
    {
        var discovered = ((Lazy<IList<TypeInfo>>)typeof(MessageConverter)
            .GetField("CloudEventTypes", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!).Value;

        Assert.Contains(discovered, type => type.AsType() == typeof(CloudEvent<>));
        Assert.Contains(discovered, type => type.AsType() == typeof(SignedCloudEvent<>));
        Assert.DoesNotContain(discovered, type => type.AsType() == typeof(AbstractCloudEvent));
        Assert.DoesNotContain(discovered, type => type.IsAbstract || type.IsInterface);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Deserialize_ShouldDiscoverConcreteCloudEventFromCloudEventInterface(bool signed)
    {
        var time = new DateTime(2023, 11, 16, 23, 24, 17, DateTimeKind.Utc);
        var payload = new MemberCreated("Jane Doe", "jd@office.com");
        var message = new Message<MemberCreated>("message-id", new Uri("https://example.test/members"), nameof(MemberCreated), payload, time);
        var cloudEvent = new CloudEvent<MemberCreated>(message);
        IMessage<MemberCreated> original = signed ? new SignedCloudEvent<MemberCreated>(cloudEvent, "signature") : cloudEvent;
        var marshaller = new JsonMarshaller();
        using var json = marshaller.Serialize(original);

        IMessage<MemberCreated> result = signed
            ? marshaller.Deserialize<ISignedCloudEvent<MemberCreated>>(json)
            : marshaller.Deserialize<ICloudEvent<MemberCreated>>(json);

        if (signed)
        {
            var discovered = Assert.IsType<SignedCloudEvent<MemberCreated>>(result);
            Assert.Equal("signature", discovered.Signature);
            Assert.Equal(cloudEvent.Specversion, discovered.Specversion);
        }
        else
        {
            var discovered = Assert.IsType<CloudEvent<MemberCreated>>(result);
            Assert.Equal(cloudEvent.Specversion, discovered.Specversion);
        }
        Assert.Equal(original.Id, result.Id);
        Assert.Equal(original.Source, result.Source);
        Assert.Equal(original.Type, result.Type);
        Assert.Equal(original.Time, result.Time);
        Assert.Equal("Jane Doe", result.Data.Name);
        Assert.Equal("jd@office.com", result.Data.EmailAddress);
    }

    // Assembly discovery must ignore abstract candidates even when they implement ICloudEvent<T>.
    private abstract record AbstractCloudEvent : CloudEvent<MemberCreated>
    {
        protected AbstractCloudEvent(IMessage<MemberCreated> message) : base(message)
        {
        }
    }
}
