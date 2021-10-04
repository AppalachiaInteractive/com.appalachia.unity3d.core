#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class bool3_OVERRIDE : Overridable<bool3, bool3_OVERRIDE>
    { public bool3_OVERRIDE() : base(false, default){}
        public bool3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool3 value) : base(overrideEnabled, value)
        {
        }

        public bool3_OVERRIDE(Overridable<bool3, bool3_OVERRIDE> value) : base(value)
        {
        }
    }
}
