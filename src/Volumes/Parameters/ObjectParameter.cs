using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public class ObjectParameter<T> : VolumeParameter<T>
    {
        public ObjectParameter(T value)
        {
            m_OverrideState = true;

            // ReSharper disable once VirtualMemberCallInConstructor
            this.value = value;
        }

        internal ReadOnlyCollection<VolumeParameter> parameters { get; private set; }

        // Force override state to true for container objects
        public override bool overrideState
        {
            get => true;

            // ReSharper disable once ValueParameterNotUsed
            set => m_OverrideState = true;
        }

        public override T value
        {
            get => m_Value;
            set
            {
                m_Value = value;

                if (m_Value == null)
                {
                    parameters = null;
                    return;
                }

                // Automatically grab all fields of type VolumeParameter contained in this instance
                parameters = m_Value.GetType()
                                    .GetFields(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(t => t.FieldType.IsSubclassOf(typeof(VolumeParameter)))
                                    .OrderBy(t => t.MetadataToken) // Guaranteed order
                                    .Select(t => (VolumeParameter) t.GetValue(m_Value))
                                    .ToList()
                                    .AsReadOnly();
            }
        }

        internal override void Interp(VolumeParameter from, VolumeParameter to, float t)
        {
            if (m_Value == null)
            {
                return;
            }

            var paramOrigin = parameters;
            var paramFrom = ((ObjectParameter<T>) from).parameters;
            var paramTo = ((ObjectParameter<T>) to).parameters;

            for (var i = 0; i < paramFrom.Count; i++)
            {
                if (paramOrigin[i].overrideState)
                {
                    paramOrigin[i].Interp(paramFrom[i], paramTo[i], t);
                }
            }
        }
    }
}