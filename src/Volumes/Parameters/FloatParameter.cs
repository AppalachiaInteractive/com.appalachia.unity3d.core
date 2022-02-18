using System;
using System.Diagnostics;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public class FloatParameter : AppaVolumeParameter<float>
    {
        public FloatParameter(float value, bool overrideState = false) : base(value, overrideState)
        {
        }

        /// <inheritdoc />
        public sealed override void Interp(float from, float to, float t)
        {
            m_Value = from + ((to - from) * t);
        }
    }
}
