#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class half2_OVERRIDE : Overridable<half2, half2_OVERRIDE>
    { public half2_OVERRIDE() : base(false, default){}
        public half2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, half2 value) : base(overrideEnabled, value)
        {
        }

        public half2_OVERRIDE(Overridable<half2, half2_OVERRIDE> value) : base(value)
        {
        }
    }
}
