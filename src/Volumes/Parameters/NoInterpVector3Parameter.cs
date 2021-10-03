using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpVector3Parameter : VolumeParameter<Vector3>
    {
        public NoInterpVector3Parameter(Vector3 value, bool overrideState = false) : base(value, overrideState)
        {
        }
    }
}