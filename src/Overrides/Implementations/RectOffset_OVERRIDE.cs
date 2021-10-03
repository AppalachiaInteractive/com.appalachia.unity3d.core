#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class RectOffset_OVERRIDE : Overridable<RectOffset, RectOffset_OVERRIDE>
    { public RectOffset_OVERRIDE() : base(false, default){}
        public RectOffset_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, RectOffset value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public RectOffset_OVERRIDE(Overridable<RectOffset, RectOffset_OVERRIDE> value) : base(value)
        {
        }
    }
}
