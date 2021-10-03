using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpCubemapParameter : VolumeParameter<Cubemap>
    {
        public NoInterpCubemapParameter(Cubemap value, bool overrideState = false) : base(value, overrideState)
        {
        }
    }
}