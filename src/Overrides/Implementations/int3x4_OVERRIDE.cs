#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class int3x4_OVERRIDE : Overridable<int3x4, int3x4_OVERRIDE>
    { public int3x4_OVERRIDE() : base(false, default){}
        public int3x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int3x4 value) : base(overrideEnabled, value)
        {
        }

        public int3x4_OVERRIDE(Overridable<int3x4, int3x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
