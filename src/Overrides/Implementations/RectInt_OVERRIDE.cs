#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class RectInt_OVERRIDE : Overridable<RectInt, RectInt_OVERRIDE>
    { public RectInt_OVERRIDE() : base(false, default){}
        public RectInt_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, RectInt value) : base(overrideEnabled, value)
        {
        }

        public RectInt_OVERRIDE(Overridable<RectInt, RectInt_OVERRIDE> value) : base(value)
        {
        }
    }
}
