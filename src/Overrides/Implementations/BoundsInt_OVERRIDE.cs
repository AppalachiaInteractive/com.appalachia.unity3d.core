#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class BoundsInt_OVERRIDE : Overridable<BoundsInt, BoundsInt_OVERRIDE>
    {
        public BoundsInt_OVERRIDE() : base(false, default)
        {
        }

        public BoundsInt_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, BoundsInt value) :
            base(overrideEnabled, value)
        {
        }

        public BoundsInt_OVERRIDE(Overridable<BoundsInt, BoundsInt_OVERRIDE> value) : base(value)
        {
        }
    }
}
