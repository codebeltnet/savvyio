using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public record Coordinates : ValueObject
    {
        public Coordinates(double latitude, double longitude)
        {
            Validator.ThrowIfLowerThan(latitude, -90, nameof(latitude));
            Validator.ThrowIfGreaterThan(latitude, 90, nameof(latitude));
            Validator.ThrowIfLowerThan(longitude, -180, nameof(longitude));
            Validator.ThrowIfGreaterThan(longitude, 180, nameof(longitude));
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }

        public double Longitude { get; }
    }
}
