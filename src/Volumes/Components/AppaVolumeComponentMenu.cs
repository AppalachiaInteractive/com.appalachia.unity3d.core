using System;

namespace Appalachia.Core.Volumes.Components
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AppaVolumeComponentMenu : Attribute
    {
        // TODO: Add support for component icons

        public AppaVolumeComponentMenu(string menu)
        {
            this.menu = menu;
        }

        #region Fields and Autoproperties

        public readonly string menu;

        #endregion
    }
}
