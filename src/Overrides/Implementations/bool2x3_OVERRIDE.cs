#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class bool2x3_OVERRIDE : Overridable<bool2x3, bool2x3_OVERRIDE>
    {
        public bool2x3_OVERRIDE() : base(false, default)
        {
        }

        public bool2x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool2x3 value) :
            base(overrideEnabled, value)
        {
        }

        public bool2x3_OVERRIDE(Overridable<bool2x3, bool2x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
