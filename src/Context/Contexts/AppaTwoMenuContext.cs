using System.Collections.Generic;

namespace Appalachia.Core.Context.Contexts
{
    public abstract class AppaTwoMenuContext<T1, T2> : AppaOneMenuContext<T1>
    {
        public abstract IReadOnlyList<T2> MenuTwoItems { get; }

        /// <inheritdoc />
        public override int RequiredMenuCount => 2;

        public IEnumerable<T2> VisibleMenuTwoItems => GetVisibleItems(1, MenuTwoItems);
        public abstract string GetMenuDisplayName(T2 item);

        /// <inheritdoc />
        public override int GetMenuItemCount(int menuIndex)
        {
            if (menuIndex == 0)
            {
                return MenuOneItems.Count;
            }

            return MenuTwoItems.Count;
        }
    }
}
