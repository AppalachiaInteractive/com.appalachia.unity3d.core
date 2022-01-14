using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class ColorParameter : AppaVolumeParameter<Color>
    {
        public ColorParameter(Color value, bool overrideState = false) : base(value, overrideState)
        {
        }

        public ColorParameter(
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

        #region Fields and Autoproperties

        public bool hdr;
        public bool showAlpha = true;
        public bool showEyeDropper = true;

        #endregion

        public override void Interp(Color from, Color to, float t)
        {
            // Lerping color values is a sensitive subject... We looked into lerping colors using
            // HSV and LCH but they have some downsides that make them not work correctly in all
            // situations, so we stick with RGB lerping for now, at least its behavior is
            // predictable despite looking desaturated when `t ~= 0.5` and it's faster anyway.
            m_Value.r = from.r + ((to.r - from.r) * t);
            m_Value.g = from.g + ((to.g - from.g) * t);
            m_Value.b = from.b + ((to.b - from.b) * t);
            m_Value.a = from.a + ((to.a - from.a) * t);
        }
    }
}
