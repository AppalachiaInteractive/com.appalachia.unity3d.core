#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class int2x4_OVERRIDE : Overridable<int2x4, int2x4_OVERRIDE>
    { public int2x4_OVERRIDE() : base(false, default){}
        public int2x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int2x4 value) : base(overrideEnabled, value)
        {
        }

        public int2x4_OVERRIDE(Overridable<int2x4, int2x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
