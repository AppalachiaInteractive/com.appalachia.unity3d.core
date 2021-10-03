using System;
using System.Diagnostics;

namespace Appalachia.Core.Volumes
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public class IntParameter : VolumeParameter<int>
    {
        public IntParameter(int value, bool overrideState = false) : base(value, overrideState)
        {
        }

        public sealed override void Interp(int from, int to, float t)
        {
            // Int snapping interpolation. Don't use this for enums as they don't necessarily have
            // contiguous values. Use the default interpolator instead (same as bool).
            m_Value = (int) (from + ((to - from) * t));
        }
    }
}