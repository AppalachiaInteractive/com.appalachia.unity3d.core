#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class RangeInt_OVERRIDE : Overridable<RangeInt, RangeInt_OVERRIDE>
    { public RangeInt_OVERRIDE() : base(false, default){}
        public RangeInt_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, RangeInt value) : base(overrideEnabled, value)
        {
        }

        public RangeInt_OVERRIDE(Overridable<RangeInt, RangeInt_OVERRIDE> value) : base(value)
        {
        }
    }
}
