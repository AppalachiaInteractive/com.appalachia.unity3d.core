using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class CubemapParameter : VolumeParameter<Cubemap>
    {
        public CubemapParameter(Cubemap value, bool overrideState = false) : base(value, overrideState)
        {
        }

        // TODO: Cubemap interpolation
    }
}