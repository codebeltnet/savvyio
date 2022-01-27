using Cuemon;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Savvyio.Assets
{
    public class LongValueGenerator : ValueGenerator<long>
    {
        public override long Next(EntityEntry entry)
        {
            return Generate.RandomNumber(1, 100000);
        }

        public override bool GeneratesTemporaryValues { get; }
    }
}
