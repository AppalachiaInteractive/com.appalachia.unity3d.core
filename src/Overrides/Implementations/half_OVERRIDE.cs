#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class half_OVERRIDE : Overridable<half, half_OVERRIDE>
    { public half_OVERRIDE() : base(false, default){}
        public half_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, half value) : base(overrideEnabled, value)
        {
        }

        public half_OVERRIDE(Overridable<half, half_OVERRIDE> value) : base(value)
        {
        }
    }
}
