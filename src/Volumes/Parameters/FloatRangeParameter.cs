using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class FloatRangeParameter : AppaVolumeParameter<Vector2>
    {
        public FloatRangeParameter(Vector2 value, float min, float max, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
            this.min = min;
            this.max = max;
        }

        #region Fields and Autoproperties

        public float max;
        public float min;

        #endregion

        /// <inheritdoc />
        public override Vector2 value
        {
            get => m_Value;
            set
            {
                m_Value.x = Mathf.Max(value.x, min);
                m_Value.y = Mathf.Min(value.y, max);
            }
        }

        /// <inheritdoc />
        public override void Interp(Vector2 from, Vector2 to, float t)
        {
            m_Value.x = from.x + ((to.x - from.x) * t);
            m_Value.y = from.y + ((to.y - from.y) * t);
        }
    }
}
