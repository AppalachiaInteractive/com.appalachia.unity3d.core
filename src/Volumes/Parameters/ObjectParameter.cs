using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Appalachia.Utility.Reflection.Extensions;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public class ObjectParameter<T> : AppaVolumeParameter<T>
    {
        public ObjectParameter(T value)
        {
            m_OverrideState = true;

            // ReSharper disable once VirtualMemberCallInConstructor
            this.value = value;
        }

        #region Fields and Autoproperties

        internal ReadOnlyCollection<AppaVolumeParameter> parameters { get; private set; }

        #endregion

        // Force override state to true for container objects
        /// <inheritdoc />
        public override bool overrideState
        {
            get => true;

            // ReSharper disable once ValueParameterNotUsed
            set => m_OverrideState = true;
        }

        /// <inheritdoc />
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

                // Automatically grab all fields of type AppaVolumeParameter contained in this instance
                parameters = m_Value.GetType()
                                    .GetFields_CACHE(ReflectionExtensions.PublicInstance)
                                    .Where(t => t.FieldType.IsSubclassOf(typeof(AppaVolumeParameter)))
                                    .OrderBy(t => t.MetadataToken) // Guaranteed order
                                    .Select(t => (AppaVolumeParameter)t.GetValue(m_Value))
                                    .ToList()
                                    .AsReadOnly();
            }
        }

        /// <inheritdoc />
        internal override void Interp(AppaVolumeParameter from, AppaVolumeParameter to, float t)
        {
            if (m_Value == null)
            {
                return;
            }

            var paramOrigin = parameters;
            var paramFrom = ((ObjectParameter<T>)from).parameters;
            var paramTo = ((ObjectParameter<T>)to).parameters;

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
