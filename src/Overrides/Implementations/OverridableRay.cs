#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableRay : Overridable<Ray, OverridableRay>
    {
        public OverridableRay() : base(false, default)
        {
        }

        public OverridableRay(bool overrideEnabled, Ray value) : base(overrideEnabled, value)
        {
        }

        public OverridableRay(Overridable<Ray, OverridableRay> value) : base(value)
        {
        }
    }
}
