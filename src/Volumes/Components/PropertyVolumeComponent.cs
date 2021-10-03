#region

#endregion

using Unity.Profiling;

namespace Appalachia.Core.Volumes.PropertyMaster
{
    public abstract class PropertyVolumeComponent<X> : PropertyVolumeComponentBase
        where X : PropertyVolumeComponent<X>
    {
        private const string _PRF_PFX = nameof(PropertyVolumeComponent<X>) + ".";

        private static readonly ProfilerMarker _PRF_PropertyVolumeComponent =
            new ProfilerMarker(_PRF_PFX + nameof(PropertyVolumeComponent<X>));

        static PropertyVolumeComponent()
        {
            using (_PRF_PropertyVolumeComponent.Auto())
            {
                PropertyMaster.componentTypes.Add(typeof(X));
            }
        }
    }
}
