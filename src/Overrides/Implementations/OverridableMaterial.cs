using System;
using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableMaterial : Overridable<Material, OverridableMaterial>
    {
        public OverridableMaterial() : base(false, default)
        {
        }

        public OverridableMaterial(bool overrideEnabled, Material value) : base(overrideEnabled, value)
        {
        }

        public OverridableMaterial(Overridable<Material, OverridableMaterial> value) : base(value)
        {
        }
    }
}
