using System;
using Cuemon;
using Cuemon.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Savvyio
{
    public class MediatorOptions
    {
        private Type _mediatorImplementationType;

        public MediatorOptions()
        {
            HandlersLifetime = ServiceLifetime.Transient;
            MediatorLifetime = ServiceLifetime.Scoped;
            MediatorImplementationType = typeof(Mediator);
            IncludeMediatorDescriptor = false;
        }

        public Type MediatorImplementationType
        {
            get => _mediatorImplementationType;
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                if (!value.HasInterfaces(typeof(IMediator))) { throw new ArgumentOutOfRangeException(nameof(value), $"Type must implement the {nameof(IMediator)} interface."); }
                _mediatorImplementationType = value;
            }
        }

        public bool IncludeMediatorDescriptor { get; set; }

        public ServiceLifetime HandlersLifetime { get; set; }

        public ServiceLifetime MediatorLifetime { get; set; }
    }
}
