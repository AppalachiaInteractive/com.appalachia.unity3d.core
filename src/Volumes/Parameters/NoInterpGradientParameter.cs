using System;
using UnityEngine;

namespace Appalachia.Core.Volumes.PropertyMaster
{
    [Serializable]
    public sealed class NoInterpGradientParameter : VolumeParameter<Gradient>
    {
        public NoInterpGradientParameter(Gradient value, bool overrideState = false) : base(value, overrideState)
        {
        }
    }
}