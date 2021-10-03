using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpMaxFloatParameter : VolumeParameter<float>
    {
        public float max;

        public NoInterpMaxFloatParameter(float value, float max, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
            this.max = max;
        }

        public override float value
        {
            get => m_Value;
            set => m_Value = Mathf.Min(value, max);
        }
    }
}