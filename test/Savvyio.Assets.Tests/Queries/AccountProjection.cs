namespace Savvyio.Assets.Queries
{
    public class AccountProjection
    {
        public AccountProjection()
        {
        }

        public AccountProjection(long id, string fullName, string emailAddress)
        {
            Id = id;
            FullName = fullName;
            EmailAddress = emailAddress;
        }

        public AccountProjection(string fullName, string emailAddress)
        {
            FullName = fullName;
            EmailAddress = emailAddress;
        }

        public long Id { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }
    }
}
