using System;
using System.Diagnostics;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public sealed class BoolParameter : AppaVolumeParameter<bool>
    {
        public BoolParameter(bool value, bool overrideState = false) : base(value, overrideState)
        {
        }
    }
}
