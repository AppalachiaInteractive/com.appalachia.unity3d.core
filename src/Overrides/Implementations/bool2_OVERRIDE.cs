#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class bool2_OVERRIDE : Overridable<bool2, bool2_OVERRIDE>
    { public bool2_OVERRIDE() : base(false, default){}
        public bool2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool2 value) : base(overrideEnabled, value)
        {
        }

        public bool2_OVERRIDE(Overridable<bool2, bool2_OVERRIDE> value) : base(value)
        {
        }
    }
}
