#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class uint2x2_OVERRIDE : Overridable<uint2x2, uint2x2_OVERRIDE>
    {
        public uint2x2_OVERRIDE() : base(false, default)
        {
        }

        public uint2x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint2x2 value) :
            base(overrideEnabled, value)
        {
        }

        public uint2x2_OVERRIDE(Overridable<uint2x2, uint2x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
