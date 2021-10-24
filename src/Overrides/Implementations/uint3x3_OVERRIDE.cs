#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class uint3x3_OVERRIDE : Overridable<uint3x3, uint3x3_OVERRIDE>
    {
        public uint3x3_OVERRIDE() : base(false, default)
        {
        }

        public uint3x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint3x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public uint3x3_OVERRIDE(Overridable<uint3x3, uint3x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
