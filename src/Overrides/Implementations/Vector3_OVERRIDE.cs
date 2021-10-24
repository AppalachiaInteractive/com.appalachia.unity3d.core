#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Vector3_OVERRIDE : Overridable<Vector3, Vector3_OVERRIDE>
    {
        public Vector3_OVERRIDE() : base(false, default)
        {
        }

        public Vector3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Vector3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public Vector3_OVERRIDE(Overridable<Vector3, Vector3_OVERRIDE> value) : base(value)
        {
        }
    }
}
