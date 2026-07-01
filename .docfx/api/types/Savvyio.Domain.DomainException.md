---
uid: Savvyio.Domain.DomainException
example:
- *content
---
Throw a `DomainException` when a domain invariant is violated to surface a domain-meaningful error that the application layer can map to an appropriate response.

```csharp
using System;
using Savvyio.Domain;

namespace ExampleApp;

public sealed class DomainExceptionExample
{
    public void ValidateOrder(int quantity)
    {
        try
        {
            EnsurePositiveQuantity(quantity);
        }
        catch (DomainException exception)
        {
            Console.WriteLine($"Domain violation: {exception.Message}");
            throw;
        }
    }

    private static void EnsurePositiveQuantity(int quantity)
    {
        if (quantity <= 0) { throw new DomainException("Order quantity must be greater than zero."); }
    }
}
```

