#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class int3_OVERRIDE : Overridable<int3, int3_OVERRIDE>
    { public int3_OVERRIDE() : base(false, default){}
        public int3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int3 value) : base(overrideEnabled, value)
        {
        }

        public int3_OVERRIDE(Overridable<int3, int3_OVERRIDE> value) : base(value)
        {
        }
    }
}
