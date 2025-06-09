using Savvyio.Commands;

namespace Savvyio.Extensions.RabbitMQ.Assets
{
    internal record CreateMemberCommand : Command
    {
        public CreateMemberCommand(string name, byte age, string emailAddress)
        {
            Name = name;
            Age = age;
            EmailAddress = emailAddress;
        }

        public string Name { get; }

        public byte Age { get; }

        public string EmailAddress { get; }
    }
}
