namespace Appalachia.Core.Attributes
{
    public static class ExecutionOrders
    {
        public const int FrameStart =                      -30000;
        public const int ApplicationLifetimeComponents =   -29000;
        public const int ApplicationManager =              -28500;
        public const int PrefabRenderingManager =          -27500;

        public const int AreaManagers =                    -26500;

        public const int CursorManager =                   -25500;
        
        public const int Water =                            -5000;
        public const int Buoyant =                          -4999;

        public const int FrameEnd =                        +30000;
    }
}
