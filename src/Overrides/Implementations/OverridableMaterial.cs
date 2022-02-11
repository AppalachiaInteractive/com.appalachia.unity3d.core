using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableMaterial : Overridable<Material, OverridableMaterial>
    {
        public OverridableMaterial() : base(false, default)
        {
        }

        public OverridableMaterial(bool overriding, Material value) : base(overriding, value)
        {
        }

        public OverridableMaterial(Overridable<Material, OverridableMaterial> value) : base(value)
        {
        }
    }
}
