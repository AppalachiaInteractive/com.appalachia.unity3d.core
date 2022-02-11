#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableRay(bool overriding, Ray value) : base(overriding, value)
        {
        }

        public OverridableRay(Overridable<Ray, OverridableRay> value) : base(value)
        {
        }
    }
}
