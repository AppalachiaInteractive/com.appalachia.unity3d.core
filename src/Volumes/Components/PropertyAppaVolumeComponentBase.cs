using Appalachia.Core.Volumes.Parameters;

namespace Appalachia.Core.Volumes.Components
{
    public abstract class PropertyAppaVolumeComponentBase : AppaVolumeComponent
    {
        public abstract void OverrideProperties(PropertyMaster master);

        protected static void Override<T>(AppaVolumeParameter<T> parameter, ref T property)
        {
            if (parameter.overrideState)
            {
                property = parameter.value;
            }
        }
    }
}
