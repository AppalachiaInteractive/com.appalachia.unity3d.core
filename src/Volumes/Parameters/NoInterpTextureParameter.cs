using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpTextureParameter : VolumeParameter<Texture>
    {
        public NoInterpTextureParameter(Texture value, bool overrideState = false) : base(value, overrideState)
        {
        }
    }
}