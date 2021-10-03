using System;
using UnityEngine;

namespace Appalachia.Core.Volumes.PropertyMaster
{
    [Serializable]
    public sealed class TransformParameter : VolumeParameter<Transform>
    {
        public TransformParameter(Transform value, bool overrideState = false) : base(value, overrideState)
        {
        }

        // TODO: Transform interpolation
    }
}