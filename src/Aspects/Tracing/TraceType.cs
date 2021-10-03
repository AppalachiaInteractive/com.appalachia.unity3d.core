namespace Appalachia.Core.Aspects.Tracing
{
    public enum TraceType
    {
        NOT_SET = -1,
        ENTRY = 0,
        EXIT = 100,
        ENTRY_EXIT = 101,
        ESCAPE = 150,
        TRY = 200,
        EXCEPTION = 1000,
        FINALLY = 2000,
        BREAK = 3000,
        CONTINUE = 4000,
        SAVE_ASSETS = 5000,
        SET_DIRTY = 5500,
        DISABLE = 6000,
        ENABLE = 6100
    }
}
