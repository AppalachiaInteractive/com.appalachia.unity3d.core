using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class TextureParameter : VolumeParameter<Texture>
    {
        public TextureParameter(Texture value, bool overrideState = false) : base(value, overrideState)
        {
        }

        // TODO: Texture interpolation
    }
}