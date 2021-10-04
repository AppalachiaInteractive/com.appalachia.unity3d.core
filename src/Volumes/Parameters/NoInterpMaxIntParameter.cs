using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpMaxIntParameter : VolumeParameter<int>
    {
        public int max;

        public NoInterpMaxIntParameter(int value, int max, bool overrideState = false) : base(value, overrideState)
        {
            this.max = max;
        }

        public override int value
        {
            get => m_Value;
            set => m_Value = Mathf.Min(value, max);
        }
    }
}