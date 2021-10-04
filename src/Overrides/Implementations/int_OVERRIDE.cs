#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class int_OVERRIDE : Overridable<int, int_OVERRIDE>
    { public int_OVERRIDE() : base(false, default){}
        public int_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, int value) : base(overrideEnabled, value)
        {
        }

        public int_OVERRIDE(Overridable<int, int_OVERRIDE> value) : base(value)
        {
        }
    }
}
