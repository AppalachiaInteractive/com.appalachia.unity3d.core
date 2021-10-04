#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class bool3x4_OVERRIDE : Overridable<bool3x4, bool3x4_OVERRIDE>
    {
        public bool3x4_OVERRIDE() : base(false, default)
        {
        }

        public bool3x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, bool3x4 value) :
            base(overrideEnabled, value)
        {
        }

        public bool3x4_OVERRIDE(Overridable<bool3x4, bool3x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
