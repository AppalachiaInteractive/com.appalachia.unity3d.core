using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class MaxIntParameter : IntParameter
    {
        public MaxIntParameter(int value, int max, bool overrideState = false) : base(value, overrideState)
        {
            this.max = max;
        }

        #region Fields and Autoproperties

        public int max;

        #endregion

        /// <inheritdoc />
        public override int value
        {
            get => m_Value;
            set => m_Value = Mathf.Min(value, max);
        }
    }
}
