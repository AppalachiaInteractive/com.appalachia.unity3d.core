using System;
using UnityEngine;

namespace Appalachia.Core.Volumes.PropertyMaster
{
    [Serializable]
    public sealed class NoInterpTransformParameter : VolumeParameter<Transform>
    {
        public NoInterpTransformParameter(Transform value, bool overrideState = false) : base(value, overrideState)
        {
        }
    }
}