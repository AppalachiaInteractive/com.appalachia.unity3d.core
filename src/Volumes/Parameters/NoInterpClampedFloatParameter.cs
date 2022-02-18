using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpClampedFloatParameter : AppaVolumeParameter<float>
    {
        public NoInterpClampedFloatParameter(float value, float min, float max, bool overrideState = false) :
            base(value, overrideState)
        {
            this.min = min;
            this.max = max;
        }

        #region Fields and Autoproperties

        public float max;
        public float min;

        #endregion

        /// <inheritdoc />
        public override float value
        {
            get => m_Value;
            set => m_Value = Mathf.Clamp(value, min, max);
        }
    }
}
