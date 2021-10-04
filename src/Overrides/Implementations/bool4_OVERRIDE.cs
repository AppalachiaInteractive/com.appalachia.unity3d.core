#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class bool4_OVERRIDE : Overridable<bool4, bool4_OVERRIDE>
    {
        public bool4_OVERRIDE() : base(false, default)
        {
        }

        public bool4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public bool4_OVERRIDE(Overridable<bool4, bool4_OVERRIDE> value) : base(value)
        {
        }
    }
}
