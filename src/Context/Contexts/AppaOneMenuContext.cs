using System.Collections.Generic;

namespace Appalachia.Core.Context.Contexts
{
    public abstract class AppaOneMenuContext<T1> : AppaMenuContext
    {
        public abstract IReadOnlyList<T1> MenuOneItems { get; }

        /// <inheritdoc />
        public override int RequiredMenuCount => 1;

        public IEnumerable<T1> VisibleMenuOneItems => GetVisibleItems(0, MenuOneItems);

        public abstract string GetMenuDisplayName(T1 item);

        /// <inheritdoc />
        public override int GetMenuItemCount(int menuIndex)
        {
            return MenuOneItems.Count;
        }
    }
}
