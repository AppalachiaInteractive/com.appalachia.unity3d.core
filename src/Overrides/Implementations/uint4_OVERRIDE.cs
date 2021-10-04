#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class uint4_OVERRIDE : Overridable<uint4, uint4_OVERRIDE>
    { public uint4_OVERRIDE() : base(false, default){}
        public uint4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint4 value) : base(overrideEnabled, value)
        {
        }

        public uint4_OVERRIDE(Overridable<uint4, uint4_OVERRIDE> value) : base(value)
        {
        }
    }
}
