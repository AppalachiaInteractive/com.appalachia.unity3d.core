using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class ClampedFloatParameter : FloatParameter
    {
        public float min;
        public float max;

        public ClampedFloatParameter(float value, float min, float max, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
            this.min = min;
            this.max = max;
        }

        public override float value
        {
            get => m_Value;
            set => m_Value = Mathf.Clamp(value, min, max);
        }
    }
}