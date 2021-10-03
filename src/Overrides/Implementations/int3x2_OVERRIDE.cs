#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class int3x2_OVERRIDE : Overridable<int3x2, int3x2_OVERRIDE>
    { public int3x2_OVERRIDE() : base(false, default){}
        public int3x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int3x2 value) : base(overrideEnabled, value)
        {
        }

        public int3x2_OVERRIDE(Overridable<int3x2, int3x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
