#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class uint4x4_OVERRIDE : Overridable<uint4x4, uint4x4_OVERRIDE>
    {
        public uint4x4_OVERRIDE() : base(false, default)
        {
        }

        public uint4x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint4x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public uint4x4_OVERRIDE(Overridable<uint4x4, uint4x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
