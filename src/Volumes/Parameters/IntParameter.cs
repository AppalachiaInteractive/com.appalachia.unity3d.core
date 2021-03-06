using System;
using System.Diagnostics;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public class IntParameter : AppaVolumeParameter<int>
    {
        public IntParameter(int value, bool overrideState = false) : base(value, overrideState)
        {
        }

        /// <inheritdoc />
        public sealed override void Interp(int from, int to, float t)
        {
            // Int snapping interpolation. Don't use this for enums as they don't necessarily have
            // contiguous values. Use the default interpolator instead (same as bool).
            m_Value = (int)(from + ((to - from) * t));
        }
    }
}
