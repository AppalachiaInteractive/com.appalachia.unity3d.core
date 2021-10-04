#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class uint4x3_OVERRIDE : Overridable<uint4x3, uint4x3_OVERRIDE>
    { public uint4x3_OVERRIDE() : base(false, default){}
        public uint4x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint4x3 value) : base(overrideEnabled, value)
        {
        }

        public uint4x3_OVERRIDE(Overridable<uint4x3, uint4x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
