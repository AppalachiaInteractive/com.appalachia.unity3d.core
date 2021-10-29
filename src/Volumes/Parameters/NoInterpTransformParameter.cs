using System;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    public sealed class NoInterpTransformParameter : AppaVolumeParameter<Transform>
    {
        public NoInterpTransformParameter(Transform value, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
        }
    }
}
