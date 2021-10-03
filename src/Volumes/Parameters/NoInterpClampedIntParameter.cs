using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpClampedIntParameter : VolumeParameter<int>
    {
        public int min;
        public int max;

        public NoInterpClampedIntParameter(int value, int min, int max, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
            this.min = min;
            this.max = max;
        }

        public override int value
        {
            get => m_Value;
            set => m_Value = Mathf.Clamp(value, min, max);
        }
    }
}