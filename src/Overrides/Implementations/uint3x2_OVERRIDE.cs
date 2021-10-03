#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class uint3x2_OVERRIDE : Overridable<uint3x2, uint3x2_OVERRIDE>
    { public uint3x2_OVERRIDE() : base(false, default){}
        public uint3x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint3x2 value) : base(overrideEnabled, value)
        {
        }

        public uint3x2_OVERRIDE(Overridable<uint3x2, uint3x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
