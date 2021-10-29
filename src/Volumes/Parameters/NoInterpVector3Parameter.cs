using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpVector3Parameter : AppaVolumeParameter<Vector3>
    {
        public NoInterpVector3Parameter(Vector3 value, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
        }
    }
}
