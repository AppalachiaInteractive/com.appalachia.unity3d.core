using System;
using System.Diagnostics;

namespace Appalachia.Core.Volumes
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class NoInterpIntParameter : VolumeParameter<int>
    {
        public NoInterpIntParameter(int value, bool overrideState = false) : base(value, overrideState)
        {
        }
    }
}