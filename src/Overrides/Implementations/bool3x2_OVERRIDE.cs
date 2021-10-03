#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class bool3x2_OVERRIDE : Overridable<bool3x2, bool3x2_OVERRIDE>
    { public bool3x2_OVERRIDE() : base(false, default){}
        public bool3x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool3x2 value) : base(overrideEnabled, value)
        {
        }

        public bool3x2_OVERRIDE(Overridable<bool3x2, bool3x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
