#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class uint4x2_OVERRIDE : Overridable<uint4x2, uint4x2_OVERRIDE>
    {
        public uint4x2_OVERRIDE() : base(false, default)
        {
        }

        public uint4x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint4x2 value) :
            base(overrideEnabled, value)
        {
        }

        public uint4x2_OVERRIDE(Overridable<uint4x2, uint4x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
