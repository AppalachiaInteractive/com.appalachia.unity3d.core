#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class bool2x4_OVERRIDE : Overridable<bool2x4, bool2x4_OVERRIDE>
    { public bool2x4_OVERRIDE() : base(false, default){}
        public bool2x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool2x4 value) : base(overrideEnabled, value)
        {
        }

        public bool2x4_OVERRIDE(Overridable<bool2x4, bool2x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
