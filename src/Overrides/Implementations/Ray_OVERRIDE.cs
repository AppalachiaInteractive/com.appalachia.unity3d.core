#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Ray_OVERRIDE : Overridable<Ray, Ray_OVERRIDE>
    { public Ray_OVERRIDE() : base(false, default){}
        public Ray_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Ray value) : base(overrideEnabled, value)
        {
        }

        public Ray_OVERRIDE(Overridable<Ray, Ray_OVERRIDE> value) : base(value)
        {
        }
    }
}
