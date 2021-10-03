using System;

namespace Appalachia.Core.Utilities
{
    [Flags]
    public enum ClearFlag
    {
        None = 0,
        Color = 1,
        Depth = 2,

        All = Depth | Color
    }
}