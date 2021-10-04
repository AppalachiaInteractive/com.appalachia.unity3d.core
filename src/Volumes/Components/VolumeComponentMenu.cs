using System;

namespace Appalachia.Core.Volumes.Components
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class VolumeComponentMenu : Attribute
    {
        public readonly string menu;

        // TODO: Add support for component icons

        public VolumeComponentMenu(string menu)
        {
            this.menu = menu;
        }
    }
}
