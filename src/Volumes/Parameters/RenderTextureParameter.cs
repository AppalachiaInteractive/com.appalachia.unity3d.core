using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class RenderTextureParameter : VolumeParameter<RenderTexture>
    {
        public RenderTextureParameter(RenderTexture value, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
        }

        // TODO: RenderTexture interpolation
    }
}
