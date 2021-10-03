#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class half3_OVERRIDE : Overridable<half3, half3_OVERRIDE>
    { public half3_OVERRIDE() : base(false, default){}
        public half3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, half3 value) : base(overrideEnabled, value)
        {
        }

        public half3_OVERRIDE(Overridable<half3, half3_OVERRIDE> value) : base(value)
        {
        }
    }
}
