#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Vector2_OVERRIDE : Overridable<Vector2, Vector2_OVERRIDE>
    {
        public Vector2_OVERRIDE() : base(false, default)
        {
        }

        public Vector2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Vector2 value) :
            base(overrideEnabled, value)
        {
        }

        public Vector2_OVERRIDE(Overridable<Vector2, Vector2_OVERRIDE> value) : base(value)
        {
        }
    }
}
