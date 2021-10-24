#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class uint2x4_OVERRIDE : Overridable<uint2x4, uint2x4_OVERRIDE>
    {
        public uint2x4_OVERRIDE() : base(false, default)
        {
        }

        public uint2x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint2x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public uint2x4_OVERRIDE(Overridable<uint2x4, uint2x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
