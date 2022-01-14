#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableVector2Int : Overridable<Vector2Int, OverridableVector2Int>
    {
        public OverridableVector2Int() : base(false, default)
        {
        }

        public OverridableVector2Int(bool overrideEnabled, Vector2Int value) : base(overrideEnabled, value)
        {
        }

        public OverridableVector2Int(Overridable<Vector2Int, OverridableVector2Int> value) : base(value)
        {
        }
    }
}
