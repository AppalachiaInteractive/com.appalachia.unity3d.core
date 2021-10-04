#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class bool4x3_OVERRIDE : Overridable<bool4x3, bool4x3_OVERRIDE>
    { public bool4x3_OVERRIDE() : base(false, default){}
        public bool4x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool4x3 value) : base(overrideEnabled, value)
        {
        }

        public bool4x3_OVERRIDE(Overridable<bool4x3, bool4x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
