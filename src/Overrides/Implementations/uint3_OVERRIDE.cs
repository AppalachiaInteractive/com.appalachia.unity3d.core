#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class uint3_OVERRIDE : Overridable<uint3, uint3_OVERRIDE>
    {
        public uint3_OVERRIDE() : base(false, default)
        {
        }

        public uint3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public uint3_OVERRIDE(Overridable<uint3, uint3_OVERRIDE> value) : base(value)
        {
        }
    }
}
