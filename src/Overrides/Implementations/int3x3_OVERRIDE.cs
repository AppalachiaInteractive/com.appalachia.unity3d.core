#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class int3x3_OVERRIDE : Overridable<int3x3, int3x3_OVERRIDE>
    { public int3x3_OVERRIDE() : base(false, default){}
        public int3x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int3x3 value) : base(overrideEnabled, value)
        {
        }

        public int3x3_OVERRIDE(Overridable<int3x3, int3x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
