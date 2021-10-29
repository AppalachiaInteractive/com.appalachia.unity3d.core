using System;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    public sealed class TransformParameter : AppaVolumeParameter<Transform>
    {
        public TransformParameter(Transform value, bool overrideState = false) : base(value, overrideState)
        {
        }

        // TODO: Transform interpolation
    }
}
