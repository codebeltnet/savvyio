using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savvyio.Domain.Assets
{
    public enum Currency
    {
        USD,
        MYR
    }

    public class Money : ValueObject
    {
        public Currency Currency { get; init; }
        public decimal Amount { get; init; }

        public Money(Currency currency, decimal amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            Currency = currency;
            Amount = amount;
        }
    }
}
