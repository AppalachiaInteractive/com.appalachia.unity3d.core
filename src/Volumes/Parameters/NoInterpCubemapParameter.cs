using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpCubemapParameter : AppaVolumeParameter<Cubemap>
    {
        public NoInterpCubemapParameter(Cubemap value, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
        }
    }
}
