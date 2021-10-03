#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class uint3x4_OVERRIDE : Overridable<uint3x4, uint3x4_OVERRIDE>
    { public uint3x4_OVERRIDE() : base(false, default){}
        public uint3x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint3x4 value) : base(overrideEnabled, value)
        {
        }

        public uint3x4_OVERRIDE(Overridable<uint3x4, uint3x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
