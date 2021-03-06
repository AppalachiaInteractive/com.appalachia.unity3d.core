using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpClampedIntParameter : AppaVolumeParameter<int>
    {
        public NoInterpClampedIntParameter(int value, int min, int max, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
            this.min = min;
            this.max = max;
        }

        #region Fields and Autoproperties

        public int max;
        public int min;

        #endregion

        /// <inheritdoc />
        public override int value
        {
            get => m_Value;
            set => m_Value = Mathf.Clamp(value, min, max);
        }
    }
}
