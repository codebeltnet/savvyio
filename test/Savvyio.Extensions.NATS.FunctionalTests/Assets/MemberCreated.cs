﻿using Savvyio.EventDriven;

namespace Savvyio.Extensions.NATS.Assets
{
    public record MemberCreated : IntegrationEvent
    {
        public MemberCreated(string name, string emailAddress)
        {
            Name = name;
            EmailAddress = emailAddress;
        }

        public string Name { get; }

        public string EmailAddress { get; }
    }
}
