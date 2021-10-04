#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Rect_OVERRIDE : Overridable<Rect, Rect_OVERRIDE>
    { public Rect_OVERRIDE() : base(false, default){}
        public Rect_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Rect value) : base(overrideEnabled, value)
        {
        }

        public Rect_OVERRIDE(Overridable<Rect, Rect_OVERRIDE> value) : base(value)
        {
        }
    }
}
