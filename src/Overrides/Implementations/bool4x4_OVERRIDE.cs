#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class bool4x4_OVERRIDE : Overridable<bool4x4, bool4x4_OVERRIDE>
    { public bool4x4_OVERRIDE() : base(false, default){}
        public bool4x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool4x4 value) : base(overrideEnabled, value)
        {
        }

        public bool4x4_OVERRIDE(Overridable<bool4x4, bool4x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
