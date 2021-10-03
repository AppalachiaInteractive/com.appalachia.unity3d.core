using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpMinIntParameter : VolumeParameter<int>
    {
        public int min;

        public NoInterpMinIntParameter(int value, int min, bool overrideState = false) : base(value, overrideState)
        {
            this.min = min;
        }

        public override int value
        {
            get => m_Value;
            set => m_Value = Mathf.Max(value, min);
        }
    }
}