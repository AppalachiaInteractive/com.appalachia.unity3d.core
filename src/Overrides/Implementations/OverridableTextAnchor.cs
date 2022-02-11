using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    /*
CameraClearFlags


    */

    [Serializable]
    public class OverridableTextAnchor : Overridable<TextAnchor, OverridableTextAnchor>
    {
        public OverridableTextAnchor(TextAnchor value) : base(false, value)
        {
        }

        public OverridableTextAnchor(bool overriding, TextAnchor value) : base(overriding, value)
        {
        }

        public OverridableTextAnchor(Overridable<TextAnchor, OverridableTextAnchor> value) : base(value)
        {
        }

        public OverridableTextAnchor() : base(false, default)
        {
        }
    }
}
