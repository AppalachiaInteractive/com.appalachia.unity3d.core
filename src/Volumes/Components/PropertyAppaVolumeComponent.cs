using Appalachia.Core.Attributes;
using Unity.Profiling;

namespace Appalachia.Core.Volumes.Components
{
    [CallStaticConstructorInEditor]
    public abstract class PropertyAppaVolumeComponent<T> : PropertyAppaVolumeComponentBase
        where T : PropertyAppaVolumeComponent<T>
    {
        private const string _PRF_PFX = nameof(PropertyAppaVolumeComponent<T>) + ".";

        private static readonly ProfilerMarker _PRF_PropertyAppaVolumeComponent =
            new(_PRF_PFX + nameof(PropertyAppaVolumeComponent<T>));

        static PropertyAppaVolumeComponent()
        {
            using (_PRF_PropertyAppaVolumeComponent.Auto())
            {
                PropertyMaster.componentTypes.Add(typeof(T));
            }
        }
    }
}
