using System;
using System.Collections.Generic;
using Cuemon;
using Savvyio.Assets.Domain.Events;
using Savvyio.Domain.EventSourcing;
using Savvyio.Handlers;

namespace Savvyio.Assets.Domain.EventSourcing
{
    public class TracedAccount : TracedAggregateRoot<Guid>
    {
        TracedAccount(Guid id, IEnumerable<ITracedDomainEvent> events) : this(new TracedAccountId(id), events) // EfCoreTracedAggregateRepository
        {
        }

        public TracedAccount(TracedAccountId id, IEnumerable<ITracedDomainEvent> events) : base(id, events)
        {
        }

        public TracedAccount(TracedAccountId id, PlatformProviderId platformProviderId, FullName fullName, EmailAddress emailAddress)
        {
            Validator.ThrowIfNull(platformProviderId, nameof(platformProviderId));
            Validator.ThrowIfNull(fullName, nameof(fullName));
            Validator.ThrowIfNull(emailAddress, nameof(emailAddress));
            AddEvent(new TracedAccountInitiated(id, platformProviderId, fullName, emailAddress));
        }

        protected override void RegisterDelegates(IFireForgetRegistry<ITracedDomainEvent> handler)
        {
            handler.Register<TracedAccountInitiated>(OnInitiated);
            handler.Register<TracedAccountFullNameChanged>(OnChangeFullName);
            handler.Register<TracedAccountEmailAddressChanged>(OnEmailChanged);
        }

        private void OnInitiated(TracedAccountInitiated e)
        {
            Id = e.Id;
            PlatformProviderId = e.PlatformProviderId;
            FullName = e.FullName;
            EmailAddress = e.EmailAddress;
        }

        private void OnChangeFullName(TracedAccountFullNameChanged e)
        {
            FullName = e.FullName;
        }

        private void OnEmailChanged(TracedAccountEmailAddressChanged e)
        {
            EmailAddress = e.EmailAddress;
        }

        public Guid PlatformProviderId { get; private set; }

        public string FullName { get; private set; }

        public string EmailAddress { get; private set; }

        public void ChangeFullName(FullName fullName)
        {
            Validator.ThrowIfNull(fullName, nameof(fullName));
            if (fullName.Equals((FullName)FullName)) { return; }
            AddEvent(new TracedAccountFullNameChanged(fullName));
        }

        public void ChangeEmailAddress(EmailAddress emailAddress)
        {
            Validator.ThrowIfNull(emailAddress, nameof(emailAddress));
            if (emailAddress.Equals((EmailAddress)EmailAddress)) { return; }
            AddEvent(new TracedAccountEmailAddressChanged(emailAddress));
        }
    }
}
