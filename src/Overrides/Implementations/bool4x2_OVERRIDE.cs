#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class bool4x2_OVERRIDE : Overridable<bool4x2, bool4x2_OVERRIDE>
    { public bool4x2_OVERRIDE() : base(false, default){}
        public bool4x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool4x2 value) : base(overrideEnabled, value)
        {
        }

        public bool4x2_OVERRIDE(Overridable<bool4x2, bool4x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
