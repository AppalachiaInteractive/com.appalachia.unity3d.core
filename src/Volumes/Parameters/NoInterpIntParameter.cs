using System;
using System.Diagnostics;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpIntParameter : AppaVolumeParameter<int>
    {
        public NoInterpIntParameter(int value, bool overrideState = false) : base(value, overrideState)
        {
        }
    }
}
