#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class int4x3_OVERRIDE : Overridable<int4x3, int4x3_OVERRIDE>
    { public int4x3_OVERRIDE() : base(false, default){}
        public int4x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int4x3 value) : base(overrideEnabled, value)
        {
        }

        public int4x3_OVERRIDE(Overridable<int4x3, int4x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
