#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class int4x4_OVERRIDE : Overridable<int4x4, int4x4_OVERRIDE>
    {
        public int4x4_OVERRIDE() : base(false, default)
        {
        }

        public int4x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int4x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public int4x4_OVERRIDE(Overridable<int4x4, int4x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
