#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Volumes.PropertyMaster
{
    [Serializable]
    public sealed class ExposedLightReferenceParameter : VolumeParameter<ExposedReference<Light>>
    {
        public ExposedLightReferenceParameter(ExposedReference<Light> value, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
        }
    }
}