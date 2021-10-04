#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class int2x2_OVERRIDE : Overridable<int2x2, int2x2_OVERRIDE>
    { public int2x2_OVERRIDE() : base(false, default){}
        public int2x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int2x2 value) : base(overrideEnabled, value)
        {
        }

        public int2x2_OVERRIDE(Overridable<int2x2, int2x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
