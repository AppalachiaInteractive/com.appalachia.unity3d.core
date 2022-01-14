#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableVector2 : Overridable<Vector2, OverridableVector2>
    {
        public OverridableVector2() : base(false, default)
        {
        }

        public OverridableVector2(bool overrideEnabled, Vector2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableVector2(Overridable<Vector2, OverridableVector2> value) : base(value)
        {
        }
    }
}
