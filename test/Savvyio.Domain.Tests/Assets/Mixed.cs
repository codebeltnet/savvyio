using System;
using Savvyio.Assets.Domain;

namespace Savvyio.Domain.Assets
{
    public record Mixed : ValueObject
    {
        public Mixed()
        {

        }

        public AccountId AccountId { get; set; }

        public Money Money { get; set; }

        public decimal SomeDecimal { get; set; } = Decimal.MaxValue;
    }
}
