#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Gradient_OVERRIDE : Overridable<Gradient, Gradient_OVERRIDE>
    {
        public Gradient_OVERRIDE() : base(false, default)
        {
        }

        public Gradient_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Gradient value) :
            base(overrideEnabled, value)
        {
        }

        public Gradient_OVERRIDE(Overridable<Gradient, Gradient_OVERRIDE> value) : base(value)
        {
        }
    }
}
