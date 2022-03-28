using System;

namespace Appalachia.Core.Objects.Models.Selectable
{
    [Flags]
    public enum SelectableState
    {
        Normal = 0,
        Dragging = 1 << 0,
        Hovering = 1 << 1,
        Disabled = 1 << 2,
        Pressed = 1 << 3,
        Selected =  1 << 4
    }
}
