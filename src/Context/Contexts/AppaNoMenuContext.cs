namespace Appalachia.Core.Context.Contexts
{
    public abstract class AppaNoMenuContext : AppaMenuContext
    {
        /// <inheritdoc />
        public override int RequiredMenuCount => 0;

        /// <inheritdoc />
        public override int GetMenuItemCount(int menuIndex)
        {
            return 0;
        }
    }
}
