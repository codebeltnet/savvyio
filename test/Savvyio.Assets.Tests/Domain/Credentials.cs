using Cuemon;
using Cuemon.Collections.Generic;
using Cuemon.Extensions;
using Cuemon.Security.Cryptography;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public class Credentials : ValueObject
    {
        public Credentials(string username, string password, string salt)
        {
            Validator.ThrowIfNullOrWhitespace(username, nameof(username));
            Validator.ThrowIfNullOrWhitespace(password, nameof(password));
            Validator.ThrowIfNullOrWhitespace(salt, nameof(salt));
            UserName = username;
            Hash = KeyedHashFactory.CreateHmacCryptoSha256(salt.ToByteArray()).ComputeHash(password).ToHexadecimalString();
            Salt = salt;
        }

        public string UserName { get; private set; }

        public string Hash { get; private set; }

        public string Salt { get; private set; }

        public override string ToString()
        {
            return DelimitedString.Create(Arguments.ToEnumerableOf(UserName, Hash), o => o.Delimiter = ":");
        }
    }
}
