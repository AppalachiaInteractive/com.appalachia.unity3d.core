using Appalachia.Core.Volumes.Parameters;

namespace Appalachia.Core.Volumes.Components
{
    public abstract class PropertyVolumeComponentBase : VolumeComponent
    {
        public abstract void OverrideProperties(PropertyMaster master);

        protected static void Override<T>(VolumeParameter<T> parameter, ref T property)
        {
            if (parameter.overrideState)
            {
                property = parameter.value;
            }
        }
    }
}
