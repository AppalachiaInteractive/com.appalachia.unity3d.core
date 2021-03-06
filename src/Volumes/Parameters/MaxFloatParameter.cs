using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class MaxFloatParameter : FloatParameter
    {
        public MaxFloatParameter(float value, float max, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
            this.max = max;
        }

        #region Fields and Autoproperties

        public float max;

        #endregion

        /// <inheritdoc />
        public override float value
        {
            get => m_Value;
            set => m_Value = Mathf.Min(value, max);
        }
    }
}
