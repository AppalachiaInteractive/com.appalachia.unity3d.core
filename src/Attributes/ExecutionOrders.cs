namespace Appalachia.Core.Attributes
{
    public static class ExecutionOrders
    {
        #region Constants and Static Readonly

        public const int ApplicationManager = -30000;
        public const int AreaManagers = -26500;
        public const int Service = -25000;
        public const int Widget = -24000;
        public const int Subwidget = -23999;
        public const int Feature = -23000;
        public const int Buoyant = -04999;
        public const int DebugLogManager = +25001;
        public const int DebugLogPopup = +25002;
        public const int DebugLogRecycledListView = +25003;
        public const int FrameEnd = +30000;
        public const int FrameStart = -29998;
        public const int GizmoDrawerService = -29997;
        public const int LifetimeComponentManager = -29999;
        public const int PrefabRenderingManager = -27500;
        public const int RuntimeGraph = +25000;
        public const int Water = -05000;

        #endregion
    }
}
