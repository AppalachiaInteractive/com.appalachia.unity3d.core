#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class int4_OVERRIDE : Overridable<int4, int4_OVERRIDE>
    {
        public int4_OVERRIDE() : base(false, default)
        {
        }

        public int4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public int4_OVERRIDE(Overridable<int4, int4_OVERRIDE> value) : base(value)
        {
        }
    }
}
