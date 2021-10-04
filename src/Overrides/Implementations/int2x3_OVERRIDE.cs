#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class int2x3_OVERRIDE : Overridable<int2x3, int2x3_OVERRIDE>
    {
        public int2x3_OVERRIDE() : base(false, default)
        {
        }

        public int2x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int2x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public int2x3_OVERRIDE(Overridable<int2x3, int2x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
