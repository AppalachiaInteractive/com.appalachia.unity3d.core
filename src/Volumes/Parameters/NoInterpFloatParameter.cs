using System;
using System.Diagnostics;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpFloatParameter : AppaVolumeParameter<float>
    {
        public NoInterpFloatParameter(float value, bool overrideState = false) : base(value, overrideState)
        {
        }
    }
}
