#region

using System;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class float_OVERRIDE : Overridable<float, float_OVERRIDE>
    { public float_OVERRIDE() : base(false, default){}
        public float_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float value) : base(overrideEnabled, value)
        {
        }

        public float_OVERRIDE(Overridable<float, float_OVERRIDE> value) : base(value)
        {
        }
    }
}
