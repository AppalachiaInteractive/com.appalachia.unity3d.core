#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class half4_OVERRIDE : Overridable<half4, half4_OVERRIDE>
    { public half4_OVERRIDE() : base(false, default){}
        public half4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, half4 value) : base(overrideEnabled, value)
        {
        }

        public half4_OVERRIDE(Overridable<half4, half4_OVERRIDE> value) : base(value)
        {
        }
    }
}
