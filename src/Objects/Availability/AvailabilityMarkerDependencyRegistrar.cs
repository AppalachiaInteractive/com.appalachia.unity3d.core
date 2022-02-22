using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Reflection.Extensions;

namespace Appalachia.Core.Objects.Availability
{
    [CallStaticConstructorInEditor]
    public sealed class
        AvailabilityMarkerDependencyRegistrar : SingletonAppalachiaBehaviour<
            AvailabilityMarkerDependencyRegistrar>
    {
        static AvailabilityMarkerDependencyRegistrar()
        {
            var implementations = typeof(IAvailabilityMarker).GetAllConcreteImplementors();

            foreach (var implementation in implementations)
            {
                RegisterDependency(implementation);
            }
        }
    }
}
