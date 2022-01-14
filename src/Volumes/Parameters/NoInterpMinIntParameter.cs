using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpMinIntParameter : AppaVolumeParameter<int>
    {
        public NoInterpMinIntParameter(int value, int min, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
            this.min = min;
        }

        #region Fields and Autoproperties

        public int min;

        #endregion

        public override int value
        {
            get => m_Value;
            set => m_Value = Mathf.Max(value, min);
        }
    }
}
