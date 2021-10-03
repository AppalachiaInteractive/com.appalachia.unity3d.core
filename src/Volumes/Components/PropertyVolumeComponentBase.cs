namespace Appalachia.Core.Volumes.PropertyMaster
{
    public abstract class PropertyVolumeComponentBase : VolumeComponent
    {
        protected static void Override<T>(VolumeParameter<T> parameter, ref T property)
        {
            if (parameter.overrideState)
            {
                property = parameter.value;
            }
        }

        public abstract void OverrideProperties(PropertyMaster master);
    }
}
