using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpRenderTextureParameter : AppaVolumeParameter<RenderTexture>
    {
        public NoInterpRenderTextureParameter(RenderTexture value, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
        }
    }
}
