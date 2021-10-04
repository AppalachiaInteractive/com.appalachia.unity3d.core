using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpColorParameter : VolumeParameter<Color>
    {
        public bool hdr;
        public bool showAlpha = true;
        public bool showEyeDropper = true;

        public NoInterpColorParameter(Color value, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
        }

        public NoInterpColorParameter(
            Color value,
            bool hdr,
            bool showAlpha,
            bool showEyeDropper,
            bool overrideState = false) : base(value, overrideState)
        {
            this.hdr = hdr;
            this.showAlpha = showAlpha;
            this.showEyeDropper = showEyeDropper;
            this.overrideState = overrideState;
        }
    }
}
