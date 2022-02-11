using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class OverridableCameraClearFlags : Overridable<CameraClearFlags, OverridableCameraClearFlags>
    {
        public OverridableCameraClearFlags(CameraClearFlags value) : base(false, value)
        {
        }

        public OverridableCameraClearFlags(bool overriding, CameraClearFlags value) : base(overriding, value)
        {
        }

        public OverridableCameraClearFlags(
            Overridable<CameraClearFlags, OverridableCameraClearFlags> value) : base(value)
        {
        }

        public OverridableCameraClearFlags() : base(false, default)
        {
        }
    }
}
