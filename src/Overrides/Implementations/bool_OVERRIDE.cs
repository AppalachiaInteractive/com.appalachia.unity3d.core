#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class bool_OVERRIDE : Overridable<bool, bool_OVERRIDE>
    {
        public bool_OVERRIDE() : base(false, default)
        {
        }

        public bool_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public bool_OVERRIDE(Overridable<bool, bool_OVERRIDE> value) : base(value)
        {
        }
    }
}
