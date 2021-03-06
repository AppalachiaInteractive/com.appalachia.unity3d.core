using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class Vector2Parameter : AppaVolumeParameter<Vector2>
    {
        public Vector2Parameter(Vector2 value, bool overrideState = false) : base(value, overrideState)
        {
        }

        /// <inheritdoc />
        public override void Interp(Vector2 from, Vector2 to, float t)
        {
            m_Value.x = from.x + ((to.x - from.x) * t);
            m_Value.y = from.y + ((to.y - from.y) * t);
        }
    }
}
