using Unity.Profiling;

namespace Appalachia.Core.Volumes.Components
{
    public abstract class PropertyAppaVolumeComponent<X> : PropertyAppaVolumeComponentBase
        where X : PropertyAppaVolumeComponent<X>
    {
        private const string _PRF_PFX = nameof(PropertyAppaVolumeComponent<X>) + ".";

        private static readonly ProfilerMarker _PRF_PropertyAppaVolumeComponent =
            new(_PRF_PFX + nameof(PropertyAppaVolumeComponent<X>));

        static PropertyAppaVolumeComponent()
        {
            using (_PRF_PropertyAppaVolumeComponent.Auto())
            {
                PropertyMaster.componentTypes.Add(typeof(X));
            }
        }
    }
}
