using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpVector4Parameter : AppaVolumeParameter<Vector4>
    {
        public NoInterpVector4Parameter(Vector4 value, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
        }
    }
}
