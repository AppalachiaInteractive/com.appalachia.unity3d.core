using System;

namespace Appalachia.Core.Volumes
{
    /// <summary>
    ///     This attribute allows you to add commands to the <strong>Add Override</strong> popup menu
    ///     on AppaVolumes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AppaVolumeComponentMenu : Attribute
    {
        // TODO: Add support for component icons

        /// <summary>
        ///     Creates a new <seealso cref="AppaVolumeComponentMenu" /> instance.
        /// </summary>
        /// <param name="menu">
        ///     The name of the entry in the override list. You can use slashes to
        ///     create sub-menus.
        /// </param>
        public AppaVolumeComponentMenu(string menu)
        {
            this.menu = menu;
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     The name of the entry in the override list. You can use slashes to create sub-menus.
        /// </summary>
        public readonly string menu;

        #endregion
    }
}
