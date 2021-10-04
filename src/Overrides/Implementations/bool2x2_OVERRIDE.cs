#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class bool2x2_OVERRIDE : Overridable<bool2x2, bool2x2_OVERRIDE>
    { public bool2x2_OVERRIDE() : base(false, default){}
        public bool2x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool2x2 value) : base(overrideEnabled, value)
        {
        }

        public bool2x2_OVERRIDE(Overridable<bool2x2, bool2x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
