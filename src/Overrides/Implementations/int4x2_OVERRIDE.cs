#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class int4x2_OVERRIDE : Overridable<int4x2, int4x2_OVERRIDE>
    {
        public int4x2_OVERRIDE() : base(false, default)
        {
        }

        public int4x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int4x2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public int4x2_OVERRIDE(Overridable<int4x2, int4x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
