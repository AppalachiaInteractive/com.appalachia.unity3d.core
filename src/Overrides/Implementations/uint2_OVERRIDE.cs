#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class uint2_OVERRIDE : Overridable<uint2, uint2_OVERRIDE>
    {
        public uint2_OVERRIDE() : base(false, default)
        {
        }

        public uint2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public uint2_OVERRIDE(Overridable<uint2, uint2_OVERRIDE> value) : base(value)
        {
        }
    }
}
