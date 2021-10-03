using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpVector2Parameter : VolumeParameter<Vector2>
    {
        public NoInterpVector2Parameter(Vector2 value, bool overrideState = false) : base(value, overrideState)
        {
        }
    }
}