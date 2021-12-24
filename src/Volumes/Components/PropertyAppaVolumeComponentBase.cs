namespace Appalachia.Core.Volumes.Components
{
    public abstract class PropertyAppaVolumeComponentBase : AppaVolumeComponent
    {
        public abstract void OverrideProperties(PropertyMaster master);

        protected static void Override<TT, TP>(TP parameter, ref TT property)
            where TP : AppaVolumeParameter<TT>
        {
            if (parameter.overrideState)
            {
                property = parameter.value;
            }
        }
    }
}
