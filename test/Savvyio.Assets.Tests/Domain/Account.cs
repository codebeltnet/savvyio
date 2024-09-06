using System;
using Cuemon;
using Savvyio.Assets.Domain.Events;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public class Account : AggregateRoot<long>
    {
        Account() // efcore
        {
        }

        public Account(PlatformProviderId platformProviderId, FullName fullName, EmailAddress emailAddress)
        {
            Validator.ThrowIfNull(platformProviderId, nameof(platformProviderId));
            Validator.ThrowIfNull(fullName, nameof(fullName));
            Validator.ThrowIfNull(emailAddress, nameof(emailAddress));
            PlatformProviderId = platformProviderId;
            FullName = fullName;
            EmailAddress = emailAddress;
            AddEvent(new AccountInitiated(this));
        }

        public Account(AccountId id) : base(id)
        {
        }

        public Guid PlatformProviderId { get; private set; }

        public string FullName { get; private set; }

        public string EmailAddress { get; private set; }

        public void ChangeFullName(FullName fullName)
        {
            Validator.ThrowIfNull(fullName, nameof(fullName));
            if (fullName.Equals((FullName)FullName)) { return; }
            FullName = fullName;
            AddEvent(new AccountFullNameChanged(this));
        }

        public void ChangeEmailAddress(EmailAddress emailAddress)
        {
            Validator.ThrowIfNull(emailAddress, nameof(emailAddress));
            if (emailAddress.Equals((EmailAddress)EmailAddress)) { return; }
            EmailAddress = emailAddress;
            AddEvent(new AccountEmailAddressChanged(this));
        }

        public void Promote(Credentials credentials)
        {
            Validator.ThrowIfNull(credentials, nameof(credentials));
            var identity = new UserAccount(Id, credentials.UserName);
            AddEvent(new UserAccountInitiated(identity));
            ChangePassword(credentials);
        }

        public void ChangePassword(Credentials credentials)
        {
            Validator.ThrowIfNull(credentials, nameof(credentials));
            var password = new UserAccountPassword(Id, credentials);
            AddEvent(new UserAccountPasswordInitiated(password));
        }

        public void RegisterFailedLogonAttempt(string userHostAddress)
        {
            Validator.ThrowIfNullOrWhitespace(userHostAddress);
            var failedLogonAttempt = new UserAccountFailedLogonAttempt(Id, userHostAddress);
            AddEvent(new UserAccountFailedLogonAttemptCreated(failedLogonAttempt));
        }

        public void Demote()
        {
            AddEvent(new UserAccountRemoved(new UserAccount(Id)));
        }
    }
}
