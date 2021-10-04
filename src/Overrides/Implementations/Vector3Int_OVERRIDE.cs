#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Vector3Int_OVERRIDE : Overridable<Vector3Int, Vector3Int_OVERRIDE>
    { public Vector3Int_OVERRIDE() : base(false, default){}
        public Vector3Int_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Vector3Int value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public Vector3Int_OVERRIDE(Overridable<Vector3Int, Vector3Int_OVERRIDE> value) : base(value)
        {
        }
    }
}
