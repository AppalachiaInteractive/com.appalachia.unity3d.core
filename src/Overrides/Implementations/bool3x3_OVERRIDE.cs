#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class bool3x3_OVERRIDE : Overridable<bool3x3, bool3x3_OVERRIDE>
    { public bool3x3_OVERRIDE() : base(false, default){}
        public bool3x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool3x3 value) : base(overrideEnabled, value)
        {
        }

        public bool3x3_OVERRIDE(Overridable<bool3x3, bool3x3_OVERRIDE> value) : base(value)
        {
        }
    }
}