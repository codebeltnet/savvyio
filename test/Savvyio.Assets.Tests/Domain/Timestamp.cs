using System;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public record Timestamp : ValueObject
    {
        public Timestamp(DateTimeOffset value)
        {
            Value = value;
        }

        public DateTimeOffset Value { get; }

        public string TimeZone { get; set; }

        public bool IsDaylightSavingTime { get; set; }
    }
}
