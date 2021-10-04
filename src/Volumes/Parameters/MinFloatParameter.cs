using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class MinFloatParameter : FloatParameter
    {
        public float min;

        public MinFloatParameter(float value, float min, bool overrideState = false) : base(value, overrideState)
        {
            this.min = min;
        }

        public override float value
        {
            get => m_Value;
            set => m_Value = Mathf.Max(value, min);
        }
    }
}