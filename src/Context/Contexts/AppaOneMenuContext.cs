using System.Collections.Generic;

namespace Appalachia.Core.Context.Contexts
{
    public abstract class AppaOneMenuContext<T1> : AppaMenuContext
    {
        public abstract IReadOnlyList<T1> MenuOneItems { get; }

        public abstract string GetMenuDisplayName(T1 item);

        public IEnumerable<T1> VisibleMenuOneItems => GetVisibleItems(0, MenuOneItems);

        public override int RequiredMenuCount => 1;

        public override int GetMenuItemCount(int menuIndex)
        {
            return MenuOneItems.Count;
        }
    }
}
