#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Vector4_OVERRIDE : Overridable<Vector4, Vector4_OVERRIDE>
    { public Vector4_OVERRIDE() : base(false, default){}
        public Vector4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Vector4 value) : base(overrideEnabled, value)
        {
        }

        public Vector4_OVERRIDE(Overridable<Vector4, Vector4_OVERRIDE> value) : base(value)
        {
        }
    }
}
