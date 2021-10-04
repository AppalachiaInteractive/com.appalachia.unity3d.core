using System;
using System.Diagnostics;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpFloatParameter : VolumeParameter<float>
    {
        public NoInterpFloatParameter(float value, bool overrideState = false) : base(
            value,
            overrideState
        )
        {
        }
    }
}
