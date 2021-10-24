using System;

namespace Appalachia.Core.Volumes.Components
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class VolumeComponentMenu : Attribute
    {
        // TODO: Add support for component icons

        public VolumeComponentMenu(string menu)
        {
            this.menu = menu;
        }

        public readonly string menu;
    }
}
