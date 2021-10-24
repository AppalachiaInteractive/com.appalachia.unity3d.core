namespace Appalachia.Core.Context.Contexts
{
    public abstract class AppaNoMenuContext : AppaMenuContext
    {
        public override int RequiredMenuCount => 0;

        public override int GetMenuItemCount(int menuIndex)
        {
            return 0;
        }
    }
}
