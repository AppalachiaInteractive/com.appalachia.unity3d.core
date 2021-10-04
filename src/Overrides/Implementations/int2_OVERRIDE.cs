#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class int2_OVERRIDE : Overridable<int2, int2_OVERRIDE>
    { public int2_OVERRIDE() : base(false, default){}
        public int2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int2 value) : base(overrideEnabled, value)
        {
        }

        public int2_OVERRIDE(Overridable<int2, int2_OVERRIDE> value) : base(value)
        {
        }
    }
}
