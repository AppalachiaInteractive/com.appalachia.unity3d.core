#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Vector2Int_OVERRIDE : Overridable<Vector2Int, Vector2Int_OVERRIDE>
    {
        public Vector2Int_OVERRIDE() : base(false, default)
        {
        }

        public Vector2Int_OVERRIDE(
            bool isOverridingAllowed,
            bool overrideEnabled,
            Vector2Int value) : base(overrideEnabled, value)
        {
        }

        public Vector2Int_OVERRIDE(Overridable<Vector2Int, Vector2Int_OVERRIDE> value) : base(value)
        {
        }
    }
}
