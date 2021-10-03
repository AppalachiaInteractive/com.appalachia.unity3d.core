#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class uint2x3_OVERRIDE : Overridable<uint2x3, uint2x3_OVERRIDE>
    { public uint2x3_OVERRIDE() : base(false, default){}
        public uint2x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint2x3 value) : base(overrideEnabled, value)
        {
        }

        public uint2x3_OVERRIDE(Overridable<uint2x3, uint2x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
